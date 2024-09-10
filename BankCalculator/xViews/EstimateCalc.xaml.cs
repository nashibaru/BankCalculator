using BankCalculator.xModels;
using BankCalculator.xViewModels;
using System.Text.RegularExpressions;

namespace BankCalculator.xViews;

public partial class EstimateCalc : ContentPage
{
    public EstimateCalc()
    {
        InitializeComponent();
        BindingContext = new EstiCalcVM();
    }

    private void DoubleEntry(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(e.NewTextValue)) return;

        if (!Regex.Match(e.NewTextValue, @"^\d+([.]\d{0,2})?$").Success)
        {
            if (sender is Entry entry)
                entry.Text = string.IsNullOrEmpty(e.OldTextValue) ? string.Empty : e.OldTextValue;
        }
    }

    private void Clearing(object sender, EventArgs e)
    {
        ElementName.Text = "";
        Cost.Text = "";
        Advance.Text = "";

        var viewModel = (EstiCalcVM)BindingContext;

        viewModel.ElementName = string.Empty;
        viewModel.Cost = string.Empty;
        viewModel.Advance = string.Empty;
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        bool allLabelsFilled = true;

        foreach (var child in Summary.Children)
            if (child is HorizontalStackLayout horizontalLayout)
                foreach (var innerChild in horizontalLayout.Children)
                    if (innerChild is Label label)
                        if (string.IsNullOrEmpty(label.Text))
                        {
                            allLabelsFilled = false;
                            break;
                        }

        // Ustawiamy widocznoœæ na podstawie wyniku sprawdzania
        Summary.IsVisible = allLabelsFilled;
        PDF.IsEnabled = true;
    }

    private void AppearList(object sender, EventArgs e)
    {
        if (GlobalVar.Flag == true) elements.IsVisible = true;
        else elements.IsVisible = false;
    }
}