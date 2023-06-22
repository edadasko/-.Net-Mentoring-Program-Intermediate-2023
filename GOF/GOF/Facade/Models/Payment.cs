namespace GOF.Facade.Models;

public class Payment
{
    public int Id { get; set; }

    public Product Product { get; set; }

    public Payment(Product product)
    {
        Product = product;
    }
}
