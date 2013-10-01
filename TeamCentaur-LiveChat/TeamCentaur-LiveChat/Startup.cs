using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TeamCentaur_LiveChat.Startup))]
namespace TeamCentaur_LiveChat
{
    public partial class Startup 
    {
        public void Configuration(IAppBuilder app) 
        {
            ConfigureAuth(app);
        }
    }
}
