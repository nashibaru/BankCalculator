namespace BankCalculator.xModels.Estimate;

public class EstimateElements(string elementName, string elementGroup, string cost, string advance, string owned)
{
    public string ElementName { get; set; } = elementName;
    public string ElementGroup { get; set; } = elementGroup;
    public string Cost { get; set; } = cost;
    public string Advance { get; set; } = advance;
    public string Owned { get; set; } = owned;

}
