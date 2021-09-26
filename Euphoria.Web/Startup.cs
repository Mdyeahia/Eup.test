using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Euphoria.Web.Startup))]
namespace Euphoria.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
