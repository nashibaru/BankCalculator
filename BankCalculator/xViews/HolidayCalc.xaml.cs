using System.Text.RegularExpressions;
using BankCalculator.xModels;
using BankCalculator.xViewModels;

namespace BankCalculator.xViews;

public partial class HolidayCalc : ContentPage
{
    public HolidayCalc()
    {
        InitializeComponent();
        BindingContext = new HoliCalcVM();
    }

    private void AddClient(object sender, EventArgs e)
    {
        if (GlobalVar.Counter > 0) clientlist.IsVisible = true;
        if (GlobalVar.Counter == 4) add.IsEnabled = false;
    }

    private void PeselEntry(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(e.NewTextValue)) return;

        if (!Regex.Match(e.NewTextValue, @"^\d+$").Success)
        {
            if (sender is Entry entry)
                entry.Text = string.IsNullOrEmpty(e.OldTextValue) ? string.Empty : e.OldTextValue;
        }
    }

    private void MoneyEntry(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(e.NewTextValue)) return;

        if (!Regex.Match(e.NewTextValue, @"^\d+([.]\d{0,2})?$").Success)
        {            
            if (sender is Entry entry)
                entry.Text = string.IsNullOrEmpty(e.OldTextValue) ? string.Empty : e.OldTextValue;
        }
    }

    private void ProcessToRDD(object sender, CheckedChangedEventArgs e)
    {
        RadioButton? rb = sender as RadioButton;

        if(rb!= null && rb.IsChecked)
        {
            if((string)rb.Content == "Mniej ni¿ 3")
                AdditionalFields.IsVisible = true;
            else
                AdditionalFields.IsVisible = false;
        }
    }

    private void ResetForm(object sender, EventArgs e)
    {
        peselek.Text = "";
        clientlist.IsVisible = false;
        lendAmount.Text = "";
        startDate.Date = DateTime.Now;

        foreach (var child in Children.Children)
            if (child is RadioButton radioButton)
                radioButton.IsChecked = false;

        foreach (var entry in AdditionalFields.Children.OfType<HorizontalStackLayout>().SelectMany(x => x.Children).OfType<Entry>())
            entry.Text = "";

        var viewModel = (HoliCalcVM)BindingContext;

        viewModel.Pesel = string.Empty;
        viewModel.LendAmount = string.Empty;
        viewModel.StartDate = DateTime.Now;
        viewModel.Option.OptionOne = false;
        viewModel.Option.OptionTwo = false;
        viewModel.MonthAgoData = new RddData();
        viewModel.TwoMonthsAgoData = new RddData();
        viewModel.ThreeMonthsAgoData = new RddData();
        viewModel.Pesels.Clear();
    }
}