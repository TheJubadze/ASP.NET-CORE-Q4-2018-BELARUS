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
        public static HtmlString CreateProductsPageLink(this IHtmlHelper html)
        {
            var result = "<a href=\"\\products.html\">All Products</a>";
            return new HtmlString(result);
        }
        public static HtmlString CreateCategoriesPageLink(this IHtmlHelper html)
        {
            var result = "<a href=\"\\categories.html\">All Categories</a>";
            return new HtmlString(result);
        }
    }
}
