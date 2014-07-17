using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MedicalCompoundManagement.Startup))]
namespace MedicalCompoundManagement
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
