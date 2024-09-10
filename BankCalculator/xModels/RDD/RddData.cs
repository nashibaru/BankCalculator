namespace BankCalculator.xModels;

public class RddData
{
    public string? Invment { get; set; }
    public string? Gain { get; set; }
    public double Rdd { get; set; }

    public void CheckRDD()
    {
        if (!string.IsNullOrEmpty(Invment) && !string.IsNullOrEmpty(Gain))
            Rdd = double.Parse(Invment) / double.Parse(Gain);
    }
}
