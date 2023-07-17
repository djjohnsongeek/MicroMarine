using System;
using System.Threading;

namespace Playground
{
    class FoodOrderingService
    {
        //// define the handler (deligate)
        //public delegate void FoodPreparedEventHandler(object src, FoodPreparedEventArgs args);

        //// define the event
        //public event FoodPreparedEventHandler FoodPrepared;

        public event EventHandler<FoodPreparedEventArgs> FoodPrepared;


        public FoodOrderingService()
        {

        }


        public void PrepareOrder(Order order)
        {
            Console.WriteLine($"Preparing Order: {order.Item}, please wait ...");
            Thread.Sleep(1000);
            OnFoodPrepared(order);
        }

        protected virtual void OnFoodPrepared(Order order)
        {
            //if (FoodPrepared != null)
            //{
            //    FoodPrepared(this, new FoodPreparedEventArgs { Order = order});
            //}

            FoodPrepared?.Invoke(this, new FoodPreparedEventArgs { Order = order });
        }
    }
}
