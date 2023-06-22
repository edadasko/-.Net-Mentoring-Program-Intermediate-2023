namespace GOF.Adapter
{
    public class ElementsToContainerAdapter<T> : IContainer<T>
    {
        private readonly IEnumerable<T> _elements;

        public ElementsToContainerAdapter(IElements<T> elements)
        {
            _elements = elements.GetElements();
        }

        public IEnumerable<T> Items => _elements;

        public int Count => _elements.Count();
    }
}
