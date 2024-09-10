using BankCalculator.xModels;
using BankCalculator.xViewModels;
using System.Text.RegularExpressions;

namespace BankCalculator.xViews;

public partial class DepositCalc : ContentPage
{
	public DepositCalc()
    {
        InitializeComponent();
        BindingContext = new DepoCalcVM();
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
}