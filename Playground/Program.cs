namespace Playground
{
    class Program
    {
        static void Main(string[] args)
        {
            var appService = new AppService();
            var mailService = new MailService();
            var orderService = new FoodOrderingService();

            // event += handler
            orderService.FoodPrepared += appService.OnFoodPrepared;
            orderService.FoodPrepared += mailService.OnFoodPrepared;

            var order = new Order { Item = "Pizza with extra cheese" };

            orderService.PrepareOrder(order);
        }
    }
}
