namespace BankCalculator.xModels;

public class Applying
{
    public List<string> Pesels { get; set; }
    public string? LendAmount { get; set; }
    public DateTime StartDate { get; set; }
    public string? Children { get; set; }
    public List<RddData> RddData { get; set; }
    public double RddAvg { get; set; }
    public string? Decision { get; set; }
    public List<string> Reason { get; set; }

    public Applying()
    {
        Pesels = [];
        RddData = [];
        Reason = [];
    }

}
