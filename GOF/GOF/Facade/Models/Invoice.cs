namespace GOF.Facade.Models;

public class Invoice
{
    public int Id { get; set; }

    public Payment Payment { get; set; }
}
