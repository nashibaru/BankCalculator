using BankCalculator.xViewModels;

namespace BankCalculator.xViews;

public partial class Landing : ContentPage
{
    public Landing()
    {
        InitializeComponent();
        BindingContext = new LandingVM();
    }
}
