using GOF.Facade.Models;

namespace GOF.Facade;

public interface IPaymentSystem
{
    bool MakePayment(Payment payment);
}
