using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Marquee_Editor.Startup))]
namespace Marquee_Editor
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
