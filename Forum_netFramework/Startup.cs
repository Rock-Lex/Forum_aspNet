using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Forum_netFramework.Startup))]
namespace Forum_netFramework
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
