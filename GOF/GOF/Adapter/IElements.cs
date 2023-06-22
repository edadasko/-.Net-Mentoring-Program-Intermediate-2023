namespace GOF.Adapter;

public interface IElements<T>
{
    IEnumerable<T> GetElements();
}
