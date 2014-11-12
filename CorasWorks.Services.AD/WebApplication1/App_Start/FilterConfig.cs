using System.Web;
using System.Web.Mvc;

namespace CorasWorks.Services.AD
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
