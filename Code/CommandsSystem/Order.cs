using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandsSystem
{
    abstract class Order : IOrder
    {
        protected readonly Good Good;
        protected readonly int Count;

        public Order(Good good, int count)
        {
            if (count < 0) throw new ArgumentOutOfRangeException();

            this.Good = good;
            this.Count = count;
        }

        public bool IsAvailable {
            get
            {
                return Count <= Good.Count;
            }
        }

        public abstract int GetTotalPrice();

        public void Ship()
        {
            Good.Count -= Count;
        }
    }
}
