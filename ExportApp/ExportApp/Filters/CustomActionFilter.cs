using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Controllers;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using Microsoft.AspNetCore.Mvc.Abstractions;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace ExportApp.Filters
{
    public class CustomActionFilter : ActionFilterAttribute
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public CustomActionFilter(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var actionDescriptor = (ControllerActionDescriptor)context.ActionDescriptor;
            TypeInfo typeInfo = actionDescriptor.ControllerTypeInfo;

            var controllerBase = typeof(ControllerBase);
            var controller = typeof(Controller);

            // Api's implements ControllerBase but not Controller
            if (typeInfo.IsSubclassOf(controllerBase) && !typeInfo.IsSubclassOf(controller))
            {
                // Handle web api exception
            }

            // Pages implements ControllerBase and Controller
            if (typeInfo.IsSubclassOf(controllerBase) && typeInfo.IsSubclassOf(controller))
            {
                // Handle page exception
            }

            base.OnActionExecuting(context);
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            base.OnResultExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }

        public override void OnResultExecuted(ResultExecutedContext context)
        {
            base.OnResultExecuted(context);
        }
    }
}
