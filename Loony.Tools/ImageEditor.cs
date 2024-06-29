using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace Loony.Tools
{
    public static class ImageEditor
    {
        public static Image CreateAvatar(string text, int size)
        {
            Brush[] brushes = new Brush[] {
              Brushes.AliceBlue,
              Brushes.AntiqueWhite,
              Brushes.Bisque,
              Brushes.CadetBlue,
              Brushes.LightGray,
              Brushes.MistyRose,
              Brushes.YellowGreen
            };

            Image bitmap = new Bitmap(size, size);
            Point atpoint = new Point(bitmap.Width / 2, bitmap.Height / 2);

            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            Random rnd = new Random();
            Brush brush = brushes[rnd.Next(brushes.Length)];
            graphics.FillEllipse(brush, 0, 0, size, size);

            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;

            graphics.DrawString(text, new Font("Tahoma", 26), Brushes.Gray, atpoint, sf);

            graphics.Dispose();

            MemoryStream m = new MemoryStream();
            bitmap.Save(m, ImageFormat.Jpeg);

            return bitmap;
        }

        public static byte[] Resize(byte[] image, int width, int height)
        {
            using (var img = new Bitmap(byteArrayToImage(image)))
            {
                return imageToByteArray(Resize(img, width, height));
            }

        }

        public static Image Resize(Image image, int width, int height)
        {
            using (var img = new Bitmap(image))
            {
                var resized = new Bitmap(width, height);
                using (var graphics = Graphics.FromImage(resized))
                {
                    graphics.CompositingQuality = CompositingQuality.HighSpeed;
                    graphics.DrawImage(img, 0, 0, width, height);
                    return resized;
                }
            }

        }

        public static Image FixedSize(Image image, int Width, int Height)
        {
            int sourceWidth = image.Width;
            int sourceHeight = image.Height;
            int sourceX = 0;
            int sourceY = 0;

            double nScaleW = Convert.ToDouble(Width) / Convert.ToDouble(sourceWidth);
            double nScaleH = Convert.ToDouble(Height) / Convert.ToDouble(sourceHeight);
            double nScale = Math.Max(nScaleH, nScaleW);
            double destY = (Height - sourceHeight * nScale) / 2;
            double destX = (Width - sourceWidth * nScale) / 2;

            int destWidth = Convert.ToInt32(Math.Round(sourceWidth * nScale));
            int destHeight = Convert.ToInt32(Math.Round(sourceHeight * nScale));

            Bitmap bmPhoto;
            try
            {
                bmPhoto = new Bitmap(destWidth + Convert.ToInt32(Math.Round(2 * destX)), destHeight + Convert.ToInt32(Math.Round(2 * destY)));
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("destWidth:{0}, destX:{1}, destHeight:{2}, desxtY:{3}, Width:{4}, Height:{5}", destWidth, destX, destHeight, destY, Width, Height), ex);
            }
            using (Graphics grPhoto = Graphics.FromImage(bmPhoto))
            {
                Rectangle to = new Rectangle(Convert.ToInt32(Math.Round(destX)), Convert.ToInt32(Math.Round(destY)), destWidth, destHeight);
                Rectangle from = new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight);
                grPhoto.DrawImage(image, to, from, GraphicsUnit.Pixel);

                return bmPhoto;
            }
        }

        public static Image Crop(Image image, int Width, int Height)
        {
            var resized = new Bitmap(Width, Height);
            var graphic = Graphics.FromImage(resized);
            graphic.CompositingQuality = CompositingQuality.HighSpeed;
            graphic.FillRectangle(Brushes.White, 0, 0, Width, Height);

            var xStartPoint = image.Width > Width ? -(int)((image.Width - Width) / 2) : (int)((Width - image.Width) / 2);
            var yStartPoint = image.Height > Height ? -(int)((image.Height - Height) / 2) : (int)((Height - image.Height) / 2);

            graphic.DrawImage(new Bitmap(image), xStartPoint, yStartPoint, image.Width, image.Height);

            return resized;
        }

        public static Image SquareToShortEdge(Image image)
        {
            var shortEdge = image.Width > image.Height ? image.Height : image.Width;
            return Crop(image, shortEdge, shortEdge);
        }

        public static Image SquareToLongEdge(Image image)
        {
            var longEdge = image.Width < image.Height ? image.Height : image.Width;
            return Crop(image, longEdge, longEdge);
        }

        public static Image byteArrayToImage(byte[] byteArray)
        {
            using (var ms = new MemoryStream(byteArray))
            {
                return Image.FromStream(ms);
            }

        }

        public static byte[] imageToByteArray(Image image)
        {
            using (var ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }
    }
}
