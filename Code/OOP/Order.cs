using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP
{
    class Order : IOrder
    {
        private Good _good;
        private int _count;

        public Order(Good good, int count)
        {
            if (count < 0) throw new ArgumentOutOfRangeException();

            _good = good;
            _count = count;
        }

        public bool IsAvailable
        {
            get
            {
                return _count <= _good.Count;
            }
        }

        public int GetTotalPrice()
        {
            return _good.Price * _count;
        }

        public void Ship()
        {
            _good.Count -= _count;
        }
    }
}
