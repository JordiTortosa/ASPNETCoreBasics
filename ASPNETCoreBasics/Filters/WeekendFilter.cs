using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ASPNETCoreBasics.Filters
{
    public class WeekendFilterAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday || DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
            {
                System.Diagnostics.Debug.WriteLine("SE HA HECHO LA LLAMADA EN FIN DE SEMANA");
                return;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("SE HA HECHO LA LLAMADA ENTRE SEMANA");
            }
            base.OnActionExecuting(context);
        }
    }
}
