using Microsoft.AspNetCore.Mvc.Filters;

namespace Iris.AspNetCore.Filter
{
    public class CoreApiFilter : ActionFilterAttribute, IAsyncExceptionFilter
    {
        public Task OnExceptionAsync(ExceptionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
