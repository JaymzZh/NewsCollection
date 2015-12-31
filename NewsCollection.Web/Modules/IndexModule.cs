using Nancy;
using NewsCollention.Entity;
using XCode;

namespace NewsCollection.Web.Modules
{
    public class IndexModule : NancyModule
    {
        public IndexModule()
        {
            Get["/"] = parameters =>
            {
                var news = New.FindAll(null, New._.CreateTime.Desc(), null, 0, 100);
                return View["index", news];
            };
        }
    }
}