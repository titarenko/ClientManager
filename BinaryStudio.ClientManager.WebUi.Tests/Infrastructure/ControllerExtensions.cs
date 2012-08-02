using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NSubstitute;

namespace BinaryStudio.ClientManager.WebUi.Tests.Infrastructure
{
    public static class ControllerExtensions
    {
        public static T MockHttpContext<T>(this T controller) where T : Controller
        {
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = GetHttpContextMock()
            };
            controller.Url = new UrlHelper(controller.ControllerContext.RequestContext);
            return controller;
        }

        private static HttpContextBase GetHttpContextMock()
        {
            var requestContext = Substitute.For<RequestContext>();
            var actionExecutingContext = Substitute.For<ActionExecutingContext>();
            var httpContextBase = Substitute.For<HttpContextBase>();
            var httpServerUtilityBase = Substitute.For<HttpServerUtilityBase>();
            var httpResponseBase = Substitute.For<HttpResponseBase>();
            var httpRequestBase = Substitute.For<HttpRequestBase>();
            var httpSessionStateBase = Substitute.For<HttpSessionStateBase>();

            requestContext.HttpContext.Returns(httpContextBase);
            actionExecutingContext.HttpContext.Returns(httpContextBase);
            httpContextBase.Request.Returns(httpRequestBase);
            httpContextBase.Response.Returns(httpResponseBase);
            httpContextBase.Server.Returns(httpServerUtilityBase);
            httpContextBase.Session.Returns(httpSessionStateBase);
            httpRequestBase.Cookies.Returns(new HttpCookieCollection());

            return httpContextBase;
        }
    }
}