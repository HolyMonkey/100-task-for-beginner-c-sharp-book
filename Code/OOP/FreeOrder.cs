using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP
{
    class FreeOrder : IOrder
    {
        private Good _good;
        private int _count;

        public FreeOrder(Good good, int count)
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
            return 0;
        }

        public void Ship()
        {
            _good.Count -= _count;
        }
    }
}
