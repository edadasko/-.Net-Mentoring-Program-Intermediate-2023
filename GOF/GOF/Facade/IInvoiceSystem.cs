using GOF.Facade.Models;

namespace GOF.Facade;

public interface IInvoiceSystem
{
    void SendInvoice(Invoice invoice);
}
