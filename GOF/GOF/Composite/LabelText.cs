namespace GOF.Composite;

public class LabelText : IXmlElement
{
    private readonly string _value;

    public LabelText(string value)
    {
        _value = value;
    }

    public string ConvertToString() =>
        $"<label value='{_value}'/>";
}
