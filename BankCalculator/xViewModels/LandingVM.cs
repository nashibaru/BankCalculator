using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BankCalculator.xViewModels;

public partial class LandingVM : ObservableObject
{
    [RelayCommand]
    private async Task GoToHoliCalc() => await Shell.Current.GoToAsync("//HoliCalc");

    [RelayCommand]
    private async Task GoToDepoCalc() => await Shell.Current.GoToAsync("//DepoCalc");

    [RelayCommand]
    private async Task GoToEstiCalc() => await Shell.Current.GoToAsync("//EstiCalc");
}
