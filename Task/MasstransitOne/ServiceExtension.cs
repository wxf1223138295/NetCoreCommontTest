using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace MasstransitOne
{
    public static class ServiceExtension
    {
        /// <summary>
        /// 添加健康检查路由地址
        /// </summary>
        /// <param name="app"></param>
        public static void AddHealthCheckMiddleware(this IApplicationBuilder app)
        {
            // 服务是否正常运行
            app.UseHealthChecks("/health/alive",
                new HealthCheckOptions()
                {
                    Predicate = reg => reg.Tags.Contains("alive")
                });

            // 服务的功能是否正常使用
            app.UseHealthChecks("/health/ready",
                new HealthCheckOptions()
                {
                    Predicate = reg => reg.Tags.Contains("ready")
                });
        }
    }
}
