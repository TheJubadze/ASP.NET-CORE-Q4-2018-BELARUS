using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace WebApp.Filters
{
    public class LoggingFilterAttribute : ActionFilterAttribute
    {
        private readonly ILogger _logger;

        public LoggingFilterAttribute(ILogger logger)
        {
            _logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation($"Begin:{context.ActionDescriptor.DisplayName}");
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation($"End: {context.ActionDescriptor.DisplayName}");
        }
    }
}
