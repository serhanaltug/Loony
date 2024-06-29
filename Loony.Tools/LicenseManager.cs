using DeviceId;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

namespace Loony.Tools
{
    public static class LicenseManager
    {
        public static bool CheckActivation()
        {
            bool result = false;

            var licenseId = GenerateLicenseId(); //ReadLicenseFile().LicenseId;
            var token = ReadLicenseFile().ActivationKey;
            result = Validate(licenseId, token);

            return result;
        }

        public static LicenseInfo LicenseInfo()
        {
            var license = new LicenseInfo();

            license.CompanyName = ReadLicenseFile().CompanyName;
            license.ActivationKey = ReadLicenseFile().ActivationKey;
            license.ApplicationName = ReadLicenseFile().ApplicationName;
            license.Version = ReadLicenseFile().Version;
            license.LicenseId = LicenseManager.GenerateLicenseId();

            if (!String.IsNullOrEmpty(license.ActivationKey))
            {
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadToken(license.ActivationKey) as JwtSecurityToken;

                var exp = Convert.ToInt32(token.Claims.FirstOrDefault(a => a.Type == "exp").Value);
                var expDate = new DateTime(1970, 01, 01).AddSeconds(exp);

                //license.ApplicationName = token.Claims.FirstOrDefault(a => a.Type == "aud").Value;
                license.ExpireDate = expDate.ToShortDateString();
                //license.Version = token.Claims.FirstOrDefault(a => a.Type == "Version").Value;
                license.LicenseType = token.Claims.FirstOrDefault(a => a.Type == "Licence Type").Value;

            }

            return license;
        }

        public static bool Validate(string licenseId, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters()
                {
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuer = true,
                    ValidIssuer = "Websiyon.com",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(licenseId))
                };

                SecurityToken validatedToken;
                IPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                return true;
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
                return false;
            }
        }

        public static void Activate(string activationKey)
        {
            var license = ReadLicenseFile();
            license.ActivationKey = activationKey;
            WriteLicenseFile(license);
        }

        static string LicensePath()
        {
            return Path.Combine(Environment.CurrentDirectory, "license.json");
        }

        static LicenseInfo ReadLicenseFile()
        {
            if (!File.Exists(LicensePath())) CreateLicenseFile();

            string text = File.ReadAllText(LicensePath());
            if (String.IsNullOrEmpty(text)) { CreateLicenseFile(); text = File.ReadAllText(LicensePath()); }

            var licenseInfo = JsonConvert.DeserializeObject<LicenseInfo>(text);

            return licenseInfo;
        }

        static void CreateLicenseFile()
        {
            if (!File.Exists(LicensePath())) File.Create(LicensePath()).Close();

            var licence = new LicenseInfo();
            licence.ApplicationName = System.Reflection.Assembly.GetEntryAssembly().GetName().Name.ToString();
            licence.Version = System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();
            licence.LicenseId = GenerateLicenseId();

            WriteLicenseFile(licence);
        }

        static void WriteLicenseFile(LicenseInfo licence)
        {
            string jsonData = JsonConvert.SerializeObject(licence);
            File.WriteAllText(LicensePath(), jsonData);
        }

        static string GenerateLicenseId()
        {
            string deviceId = new DeviceIdBuilder()
                .AddMachineName()
                .AddMacAddress()
                .ToString();

            return FormatLicenseKey(GetMd5Sum(deviceId));
        }

        static string GetMd5Sum(string productIdentifier)
        {
            Encoder enc = Encoding.Unicode.GetEncoder();
            byte[] unicodeText = new byte[productIdentifier.Length * 2];
            enc.GetBytes(productIdentifier.ToCharArray(), 0, productIdentifier.Length, unicodeText, 0, true);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(unicodeText);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                sb.Append(result[i].ToString("X2"));
            }
            return sb.ToString();
        }

        static string FormatLicenseKey(string productIdentifier)
        {
            productIdentifier = productIdentifier.Substring(0, 28).ToUpper();
            char[] serialArray = productIdentifier.ToCharArray();
            StringBuilder licenseKey = new StringBuilder();

            int j = 0;
            for (int i = 0; i < 28; i++)
            {
                for (j = i; j < 4 + i; j++)
                {
                    licenseKey.Append(serialArray[j]);
                }
                if (j == 28)
                {
                    break;
                }
                else
                {
                    i = (j) - 1;
                    licenseKey.Append("-");
                }
            }
            return licenseKey.ToString();
        }

        //public static string CreateToken(string licenseId)
        //{
        //    string securityKey = licenseId;

        //    var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
        //    var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

        //    var claims = new List<Claim>
        //    {
        //        new Claim("Customer ID", "1"),
        //        new Claim("Customer Name", "Loony"),
        //        new Claim("Licence Type", "1"),
        //        new Claim("Registration Date", DateTime.Today.ToShortDateString())
        //    };

        //    var token = new JwtSecurityToken(
        //            issuer: "Websiyon.com",
        //            expires: DateTime.UtcNow.AddMonths(12),
        //            signingCredentials: signingCredentials,
        //            claims: claims
        //        );

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}

    }

    public class LicenseInfo
    {
        public string CompanyName { get; set; }
        public string ApplicationName { get; set; }
        public string Version { get; set; }
        public string LicenseType { get; set; }
        public string LicenseId { get; set; }
        public string ActivationKey { get; set; }
        public string ExpireDate { get; set; }
    }
}
