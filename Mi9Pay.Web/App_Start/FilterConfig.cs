using Mi9Pay.Web.ActionFilters;
using System.Web;
using System.Web.Mvc;

namespace Mi9Pay.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new LoggingFilterAttribute());
            filters.Add(new CustomRequireHttpsAttribute());
        }
    }
}
