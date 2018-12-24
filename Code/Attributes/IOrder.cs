namespace CommandsSystem
{
    interface IOrder
    {
        bool IsAvailable { get; }

        int GetTotalPrice();
        void Ship();
    }
}