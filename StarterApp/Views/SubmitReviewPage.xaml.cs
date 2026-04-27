using StarterApp.Database.Models;
using StarterApp.ViewModels;

namespace StarterApp.Views;

[QueryProperty(nameof(Item), "Item")]
public partial class SubmitReviewPage : ContentPage
{
    private readonly SubmitReviewViewModel _viewModel;

    public Item Item
    {
        set
        {
            if (value != null)
            {
                _viewModel.LoadItem(value);
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