using invMed.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using invMed.Data.Enums;
using Microsoft.Extensions.Hosting;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace invMed.Services
{
    public class NotificationsService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<NotificationsService> _logger;

        public NotificationsService(IServiceScopeFactory scopeFactory, ILogger<NotificationsService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var now = DateTime.Now;
                var timeToMidnight = (23 - now.Hour) * 3600 + (59 - now.Minute) * 60 + (59 - now.Second);
                await Task.Delay(TimeSpan.FromSeconds(timeToMidnight), cancellationToken);

                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        await TryCreateExpiredItemNotifications(db);
                    }
                    catch
                    {

                    }
                    await Task.Delay(TimeSpan.FromHours(24), cancellationToken);
                }
            };
        }

        private async Task TryCreateExpiredItemNotifications(ApplicationDbContext db)
        {
            var items = await db.Items.Where(x => x.ExpirationDate > DateTime.Now.AddDays(-30)).Where(x => x.Type != ItemTypeEnum.Over).OrderBy(x => x.ExpirationDate).ToListAsync();

            foreach (var item in items)
            {
                var notification = new Notification()
                {
                    CreationDate = DateTime.Now,
                    IsNew = true,
                    Item = item,
                    Type = NotificationTypeEnum.ExpirationDate
                };
                db.Notifications.Add(notification);
            }
            await db.SaveChangesAsync();
        }

    }
}
