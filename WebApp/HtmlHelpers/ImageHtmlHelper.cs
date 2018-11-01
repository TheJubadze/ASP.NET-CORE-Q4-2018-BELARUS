using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.HtmlHelpers
{
    public static class ImageHtmlHelper
    {
        public static HtmlString CreateImageLink(this IHtmlHelper html, string imageId, string content)
        {
            var result = $"<a href=\"Images/{imageId}\">{content}</a>";
            return new HtmlString(result);
        }
    }
}
