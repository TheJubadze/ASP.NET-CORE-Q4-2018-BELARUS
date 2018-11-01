using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WebApp.TagHelpers
{
    public class ImageTagHelper : TagHelper
    {
        public string NorthwindId { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            var address = "Images/" + NorthwindId;
            output.Attributes.SetAttribute("href", address);
            //output.Content.SetContent(address);
        }
    }
}
