using BankCalculator.xModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BankCalculator.xViewModels;

public partial class HoliCalcVM : ObservableObject
{
    [ObservableProperty]
    private string? pesel;

    [ObservableProperty]
    private string lendAmount;
    
    [ObservableProperty]
    private DateTime startDate;

    [ObservableProperty]
    private RadioHelper option;

    [ObservableProperty]
    private RddData monthAgoData;

    [ObservableProperty]
    private RddData twoMonthsAgoData;

    [ObservableProperty]
    private RddData threeMonthsAgoData;

    public ObservableCollection<string> Pesels { get; } = [];

    private Applying apply;

    private bool hasError = false;
    private string errorMessage = "";

    public HoliCalcVM()
    {
        Option = new RadioHelper();
        MonthAgoData = new RddData();
        TwoMonthsAgoData = new RddData();
        ThreeMonthsAgoData = new RddData();
        StartDate = DateTime.Now;
    }

    [RelayCommand]
    private void AddClient()
    {
        if (!string.IsNullOrEmpty(Pesel) && Pesel.Length == 11)
        {
            Pesels.Add(Pesel);
            GlobalVar.Counter++;
            Pesel = string.Empty;
        }
    }

    [RelayCommand]
    private async Task HolidayCalculate()
    {
        apply = new();

        if (Pesels.Count > 0 && !string.IsNullOrEmpty(LendAmount))
        {
            if (double.Parse(LendAmount) > 1200000) apply.Reason.Add("Kwota kredytu przekroczyła 1.200.000,00 PLN.");

            if (DateTime.Compare(StartDate, new DateTime(2022, 6, 30)) > 0) apply.Reason.Add("Data uruchomienia kredytu przekroczyła 30.06.2022.");

            foreach (var item in Pesels) apply.Pesels.Add(item);
            apply.LendAmount = LendAmount;
            apply.StartDate = StartDate;

            if (Option.OptionOne == false && Option.OptionTwo == false)
            {
                errorMessage = "Zaznacz jedną z opcji!";
                hasError = true;
                apply.Reason.Clear();
                apply.Pesels.Clear();
            }

            if (Option.OptionTwo) apply.Children = "3 lub więcej";
            if (Option.OptionOne)
            {
                apply.Children = "Mniej niż 3";
                if (
                    !string.IsNullOrEmpty(MonthAgoData.Gain) &&
                    !string.IsNullOrEmpty(MonthAgoData.Invment) &&
                    !string.IsNullOrEmpty(TwoMonthsAgoData.Gain) &&
                    !string.IsNullOrEmpty(TwoMonthsAgoData.Invment) &&
                    !string.IsNullOrEmpty(ThreeMonthsAgoData.Gain) &&
                    !string.IsNullOrEmpty(ThreeMonthsAgoData.Invment)
                   )
                {
                    apply.RddData.Add(MonthAgoData);
                    apply.RddData.Add(TwoMonthsAgoData);
                    apply.RddData.Add(ThreeMonthsAgoData);
                    MonthAgoData.CheckRDD();
                    TwoMonthsAgoData.CheckRDD();
                    ThreeMonthsAgoData.CheckRDD();
                    apply.RddAvg = RDD();

                    if (RDD() < 0.3) apply.Reason.Add("Wskaźnik RDD jest mniejszy niż 30%.");
                }
                else
                {
                    hasError = true;
                    errorMessage = "Wypełnij raty i dochody!";
                    apply.Reason.Clear();
                    apply.Pesels.Clear();
                }

                if (apply.Reason.Count == 0) apply.Decision = "Decyzja pozytywna";
                else apply.Decision = "Decyzja negatywna";

                if (hasError == false)
                {
                    GlobalVar.Counter = 0;
                    HoliToPDF.Generate(apply);
                    apply = null;
                    await Undo();
                }
            }
        }
        else
        {
            errorMessage = "Nie wszystkie pola w formularzu zostały wypełnione!";
            hasError = true;
            apply.Reason.Clear();
            apply.Pesels.Clear();
        }

        await ShowErrorMessage();
    }

    private async Task ShowErrorMessage()
    {
        if (hasError)
        {
            await Application.Current.MainPage.DisplayAlert("Błąd", errorMessage, "OK");
            hasError = false;
        }
    }

    private double RDD() => Math.Round((MonthAgoData.Rdd + TwoMonthsAgoData.Rdd + ThreeMonthsAgoData.Rdd) / 3, 2);

    [RelayCommand]
    private static async Task Undo() => await Shell.Current.GoToAsync("//MainPage");
}
