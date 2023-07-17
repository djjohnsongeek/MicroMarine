using System;

namespace Playground
{
    class MailService
    {
        public void OnFoodPrepared(object src, FoodPreparedEventArgs args)
        {
            Console.WriteLine($"MailService: your food ({args.Order.Item}) is prepared.");
        }
    }
}
