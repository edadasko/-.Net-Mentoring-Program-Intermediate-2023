using GOF.Facade.Models;

namespace GOF.Facade;

public class OrderFacade : IOrderFacade
{
    private readonly IProductCatalog _productCatalog;
    private readonly IPaymentSystem _paymentSystem;
    private readonly IInvoiceSystem _invoiceSystem;

    public OrderFacade(
        IProductCatalog productCatalog,
        IPaymentSystem paymentSystem,
        IInvoiceSystem invoiceSystem)
    {
        _productCatalog = productCatalog;
        _paymentSystem = paymentSystem;
        _invoiceSystem = invoiceSystem;
    }

    public void PlaceOrder(string productId, int quantity, string email)
    {
        Product product = _productCatalog.GetProductDetails(productId);

        Payment payment = new(product);

        if (_paymentSystem.MakePayment(payment))
        {
            Invoice invoice = new(payment);
            _invoiceSystem.SendInvoice(invoice);
        }
        else
        {
            throw new Exception("Payment failed");
        }
    }
}
