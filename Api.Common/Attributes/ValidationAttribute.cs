using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http.Filters;

namespace Api.Common.Attributes
{
    public class ValidationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {

            if (actionContext.ModelState.IsValid) return;

            var errors = actionContext.ModelState
                .Where(e => e.Value.Errors.Count > 0)
                .Select(e => new Error
                {
                    Name = e.Key,
                    Message = e.Value.Errors.First().ErrorMessage ?? e.Value.Errors.First().Exception.Message
                }).ToArray();

            var strErrors = new StringBuilder();
            foreach (var error in errors)
            {
                var errorM = String.IsNullOrEmpty(error.Message) ? "Invalid Value" : error.Message;
                strErrors.Append(string.Format("[{0}]:{{{1}}}/", error.Name, errorM));
            }
            actionContext.Response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
                ReasonPhrase = strErrors.ToString(),
                Content = new StringContent(strErrors.ToString())
            };
        }

        private class Error
        {
            public string Name { get; set; }
            public string Message { get; set; }
        }
    }
}
