using Contacts.ViewModels;
using Xamarin.Forms;

namespace Contacts.Views
{
    public class ViewPage : ContentPage
    {
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is ILoadable viewmodel)
            {
                await viewmodel.OnAppeared();
            }
        }

        protected async override void OnDisappearing()
        {
            if (BindingContext is ILoadable viewmodel)
            {
                await viewmodel.OnDisappearing();
            }
            base.OnDisappearing();
        }
    }
}
