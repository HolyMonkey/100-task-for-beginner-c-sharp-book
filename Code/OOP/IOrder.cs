namespace OOP
{
    interface IOrder
    {
        bool IsAvailable { get; }

        int GetTotalPrice();
        void Ship();
    }
}