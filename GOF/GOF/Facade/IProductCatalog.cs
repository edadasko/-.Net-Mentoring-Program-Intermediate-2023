using GOF.Facade.Models;

namespace GOF.Facade;

public interface IProductCatalog
{
    Product GetProductDetails(string productId);
}
