using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SGSv3.Startup))]
namespace SGSv3
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
