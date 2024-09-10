using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BankCalculator.xViewModels;

public partial class DepoCalcVM : ObservableObject
{
    [ObservableProperty]
    private string? depoAmount;

    [ObservableProperty]
    private double percentage = 5.0;

    [ObservableProperty]
    private int duration = 4;

    [ObservableProperty]
    private double depoValue;

    [RelayCommand]
    private void DepositCalculate()
    {
        double depoAmount = 0;
        if (!string.IsNullOrEmpty(DepoAmount)) depoAmount = double.Parse(DepoAmount);

        DateTime today = DateTime.Now;

        DateTime startDate = new DateTime(today.Year, today.Month, today.Day);
        DateTime endDate = startDate.AddMonths(Duration);

        TimeSpan difference = endDate - startDate;
        int daysBetween = difference.Days - 1;

        double percentage = Math.Round(Percentage, 1);
        double value = depoAmount * (percentage / 100) * daysBetween / 365;

        DepoValue = Math.Round(value * 0.81, 2);
    }

    [RelayCommand]
    private static async Task Undo() => await Shell.Current.GoToAsync("//MainPage");
}
