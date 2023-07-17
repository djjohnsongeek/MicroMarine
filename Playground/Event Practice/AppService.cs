using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Playground
{
    class AppService
    {
        public void SendAppNotification()
        {
            Console.WriteLine("App Notification Sent");
        }

        public void OnFoodPrepared(object src, FoodPreparedEventArgs args)
        {
            Console.WriteLine($"AppService: Your food ({args.Order.Item}) has been prepared.");
        }
    }
}
