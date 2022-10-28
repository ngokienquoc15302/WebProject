using System.Web.Mvc;

namespace SpiderFoodStore.Areas.QWRtaW5TcGlkZXI
{
    public class QWRtaW5TcGlkZXIAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "QWRtaW5TcGlkZXI";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "QWRtaW5TcGlkZXI_default",
                "QWRtaW5TcGlkZXI/{controller}/{action}/{id}",
                new { controller = "Customers", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}