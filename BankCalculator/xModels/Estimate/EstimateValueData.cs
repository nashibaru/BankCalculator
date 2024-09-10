using CommunityToolkit.Mvvm.ComponentModel;

namespace BankCalculator.xModels.Estimate;

public partial class EstimateValueData : ObservableObject
{
    public double CostSum { get; set; }
    public double AdvanceSum { get; set; }
    public double OwnedSum { get; set; }
    public double AllValue { get; set; }
    public double ToLend { get; set; }
    public double MaxLend { get; set; }
    public double AreaValueEst { get; set; }

    [ObservableProperty]
    public string? costSumStr;
    [ObservableProperty]
    public string? advanceSumStr;
    [ObservableProperty]
    public string? ownedSumStr;
    [ObservableProperty]
    public string? allValueStr;
    [ObservableProperty]
    public string? maxLendStr;
    [ObservableProperty]
    public string? toLendStr;
    [ObservableProperty]
    public double ltv;
}
