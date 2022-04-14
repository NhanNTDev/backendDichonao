using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DiCho.DataService.Services
{
    public class WorkerService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public WorkerService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();
                    await _orderService.StatusOrderByTime();
                }
                await Task.Delay(30000, cancellationToken);
            }
        }
    }
}
