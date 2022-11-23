namespace WaterCompany.Helpers
{
    using System.Net;
    using Microsoft.AspNetCore.Mvc;

    public class NotFoundViewResult : ViewResult
    {
        public NotFoundViewResult(string viewName)
        {
            ViewName = viewName;
            StatusCode = (int)HttpStatusCode.NotFound;
        }
    }
}
