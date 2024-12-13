using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(QLNS.App_Start.Startup))] // Chú ý thay thế bằng namespace thực tế của bạn

namespace QLNS.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Cấu hình OWIN tại đây
            app.MapSignalR(); // Ví dụ: sử dụng SignalR
        }
    }
}