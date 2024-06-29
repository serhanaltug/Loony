namespace Loony.Web.Extensions
{
    //[HtmlTargetElement("li", ParentTag = "ul", Attributes = "menu-item")]
    //public class ActiveMenuTagHelper : TagHelper
    //{
    //    [HtmlAttributeName("asp-action")]
    //    public string Action { get; set; }

    //    [HtmlAttributeName("asp-controller")]
    //    public string Controller { get; set; }

    //    [ViewContext]
    //    [HtmlAttributeNotBound]
    //    public ViewContext ViewContext { get; set; }

    //    public override void Process(TagHelperContext context, TagHelperOutput output)
    //    {
    //        string currentController = ViewContext.RouteData.Values["Controller"].ToString();
    //        string currentAction = ViewContext.RouteData.Values["Action"].ToString();

    //        if ((!string.IsNullOrWhiteSpace(Controller) && Controller.ToLower() != currentController.ToLower()) && (!string.IsNullOrWhiteSpace(Action) && Action.ToLower() != currentAction.ToLower()))
    //            output.Attributes.Add("class", "m-menu__item--active");
    //    }
    //}
}
