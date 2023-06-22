namespace GOF.Composite;

public class InputText : IXmlElement
{
    private readonly string _name;
    private readonly string _value;

    public InputText(string name, string value)
    {
        _name = name;
        _value = value;
    }

    public string ConvertToString() =>
        $"<inputText name='{_name}' value='{_value}'/>";
}
