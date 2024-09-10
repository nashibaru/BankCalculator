using BankCalculator.xModels;
using BankCalculator.xModels.Estimate;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BankCalculator.xViewModels;

public partial class EstiCalcVM : ObservableObject
{
    [ObservableProperty]
    private string? applicants;
    [ObservableProperty]
    private string? address;
    [ObservableProperty]
    private string? area;
    [ObservableProperty]
    private DateTime finish;

    [ObservableProperty]
    private string? areaValueEst;
    [ObservableProperty]
    private string? areaValue;
    [ObservableProperty]
    private string? lendValue;
    [ObservableProperty]
    private string? ownedValue;

    [ObservableProperty]
    private string? elementName;
    [ObservableProperty]
    private string? elementGroup;
    [ObservableProperty]
    private string? cost;
    [ObservableProperty]
    private string? advance;
    [ObservableProperty]
    private string? owned;

    public ObservableCollection<EstimateElements> Elements { get; } = [];

    [ObservableProperty]
    public List<string> options = ["Stan zerowy",
                                   "Stan surowy zamknięty",
                                   "Instalacje",
                                   "Stan wykończeniowy",
                                   "Prace zewnętrzne",
                                   "Pozostałe"];

    [ObservableProperty]
    private EstimateValueData estimate = new();

    public EstiCalcVM()
    {
        Finish = DateTime.Now;
    }

    [RelayCommand]
    private async Task AddElement()
    {
        if(!AreAnyNullOrEmpty(Applicants, Address, Area, AreaValue, AreaValueEst, LendValue, OwnedValue, ElementName, ElementGroup, Cost, Advance, Owned))
        {
            if(double.Parse(Advance) < double.Parse(Cost))
            {
                Elements.Add(new(ElementName, ElementGroup, Cost, Advance, Owned));
                if (Elements.Count > 0) GlobalVar.Flag = true;
                ElementName = string.Empty;
                ElementGroup = string.Empty;
                Cost = string.Empty;
                Advance = string.Empty;
                Owned = string.Empty;
            }
            else
                await Application.Current.MainPage.DisplayAlert("Błąd", "Nie można wprowadzić elementu, którego zaawansowanie wynosi 100%.", "OK");
        }
        else
            await Application.Current.MainPage.DisplayAlert("Błąd", "Nie wszystkie pola zostały wypełnione.", "OK");
    }

    [RelayCommand]
    private void RemoveElement(object obj)
    {
        var selectedElement = obj as EstimateElements;

        if (selectedElement != null)
        {
            Estimate.CostSum = 0;
            Estimate.AdvanceSum = 0;
            Estimate.OwnedSum = 0;

            Elements.Remove(selectedElement);
            if (Elements.Count == 0) GlobalVar.Flag = false;
        }
    }

    [RelayCommand]
    private async Task GetEstimation()
    {
        if (Elements.Count > 0)
        {
            foreach (var element in Elements)
            {
                Estimate.CostSum += double.Parse(element.Cost);
                Estimate.AdvanceSum += double.Parse(element.Advance);
                Estimate.OwnedSum += double.Parse(element.Owned);
            }

            Estimate.ToLend = Estimate.CostSum - Estimate.AdvanceSum - Estimate.OwnedSum;

            Estimate.CostSumStr = Estimate.CostSum.ToString();
            Estimate.AdvanceSumStr = Estimate.AdvanceSum.ToString();
            Estimate.OwnedSumStr = Estimate.OwnedSum.ToString();
            Estimate.AllValueStr = (double.Parse(LendValue) + double.Parse(OwnedValue) + double.Parse(AreaValue)).ToString();
            Estimate.MaxLendStr = Math.Round(double.Parse(AreaValueEst) * 0.8 - double.Parse(LendValue), 2).ToString();
            Estimate.Ltv = double.Parse(LendValue) / double.Parse(AreaValueEst) * 100;

            if (Estimate.ToLend < double.Parse(AreaValueEst) * 0.8 - double.Parse(LendValue))
                Estimate.ToLendStr = Estimate.ToLend.ToString();
            else
                Estimate.ToLendStr = Estimate.ToLend.ToString() + " - Wyliczona wartość kredytu przekracza maksymalną możliwą wartość";

            Estimate.CostSum = 0;
            Estimate.AdvanceSum = 0;
            Estimate.OwnedSum = 0;
        }
        else
            await Application.Current.MainPage.DisplayAlert("Błąd", "Brak elementów w kosztorysie.", "OK");
    }

    [RelayCommand]
    private static async Task Undo() => await Shell.Current.GoToAsync("//MainPage");

    [RelayCommand]
    private async Task GeneratePDF()
    {
        EstiToPDF.Generate(Elements, Estimate, Finish, Applicants, Address, Area, AreaValue, AreaValueEst, LendValue, OwnedValue);
        await Undo();
    }

    private static bool AreAnyNullOrEmpty(params string[] values) => values.Any(string.IsNullOrEmpty);
}
