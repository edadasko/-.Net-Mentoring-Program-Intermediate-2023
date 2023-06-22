namespace GOF.Facade;

public interface IOrderFacade
{
    void PlaceOrder(string productId, int quantity, string email);
}
