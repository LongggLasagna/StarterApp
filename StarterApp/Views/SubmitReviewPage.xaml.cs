using StarterApp.Database.Models;
using StarterApp.ViewModels;

namespace StarterApp.Views;

[QueryProperty(nameof(Rental), "Rental")]
public partial class SubmitReviewPage : ContentPage
{
    private readonly SubmitReviewViewModel _viewModel;

    public Rental Rental
    {
        set
        {
            if (value != null)
            {
                _viewModel.LoadRental(value.Id);
            }
        }
    }

    public SubmitReviewPage(SubmitReviewViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }
}