using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Loony.Web.Extensions
{
    [HtmlTargetElement("table-head")]
    public class TableTagHelper : TagHelper
    {
        public string Width { get; set; }
        public string Sortby { get; set; }
        public string Sortdir { get; set; }
        public string Sortorder { get; set; }
        public string Filter { get; set; }
        public string Search { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "th";
            output.TagMode = TagMode.StartTagAndEndTag;

            output.Attributes.SetAttribute("style", $"width:{Width}");
            output.Attributes.SetAttribute("class", $"sorting{(Sortorder == Sortby ? "_" + Sortdir : "")}");
            output.Attributes.SetAttribute("onclick", $"GetData(1,'{Sortby}','{(Sortorder == Sortby && Sortdir == "asc" ? "desc" : "asc")}','{Filter}','{Search}')");

            base.Process(context, output);
        }
    }
}
