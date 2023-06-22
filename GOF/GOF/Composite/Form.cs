using System.Text;

namespace GOF.Composite;

public class Form : IXmlElement
{
    private readonly string _name;
    private readonly List<IXmlElement> _xmlElements;

    public Form(string name)
    {
        _name = name;
        _xmlElements = new List<IXmlElement>();
    }

    public void AddComponent(IXmlElement xmlElement)
    {
        _xmlElements.Add(xmlElement);
    }

    public string ConvertToString()
    {
        StringBuilder stringBuilder = new();
        stringBuilder.Append(@$"<form name='{_name}'>");
        foreach (var xmlElement in _xmlElements)
        {
            stringBuilder.Append(xmlElement.ConvertToString());
        }
        stringBuilder.Append(@$"</form>");
        return stringBuilder.ToString();
    }
}

