using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Contacts.Helpers;
using Contacts.Models;
using Contacts.Services.Data;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Reactive.Bindings;
using Xamarin.Forms;

namespace Contacts.ViewModels.Contact
{
    public class UpdateContactPageViewModel : ViewModel
    {
        private readonly IDataService dataService;

        public ReactiveProperty<string> ProfilePic { get; set; } = new ReactiveProperty<string>("ic_add.png");
        public ReactiveProperty<string> Name { get; set; } = new ReactiveProperty<string>(string.Empty);
        public ReactiveProperty<string> Mobile { get; set; } = new ReactiveProperty<string>(string.Empty);
        public ReactiveProperty<string> Landline { get; set; } = new ReactiveProperty<string>(string.Empty);
        public ReactiveProperty<string> IsFavorite { get; set; } = new ReactiveProperty<string>(string.Empty);

        public ReactiveCommand UpdateCommand { get; set; }
        public ReactiveCommand MarkFavoriteCommand { get; set; } = new ReactiveCommand();
        public ReactiveCommand UpdatePhotoCommand { get; set; } = new ReactiveCommand();
        public ReactiveCommand DeleteCommand { get; set; } = new ReactiveCommand();
        private MyContact contactToUpdate;
        private INavigation navigator;

        public UpdateContactPageViewModel(MyContact contactToUpdate, INavigation navigator)
        {
            this.contactToUpdate = contactToUpdate;
            this.navigator = navigator;
            dataService = DependencyService.Get<IDataService>();
            UpdateCommand = Name.Select(a => !string.IsNullOrEmpty(a)).
                          CombineLatest(Mobile.Select(b => !string.IsNullOrEmpty(b)),
                          (a, b) => a && b).ToReactiveCommand();
            UpdateCommand.Subscribe(async () => await ExecuteUpdateCommand());
            DeleteCommand.Subscribe(async () => await ExecuteDeleteCommand());
            MarkFavoriteCommand.Subscribe(async () => ExecuteMarkFavoriteCommand());
            UpdatePhotoCommand.Subscribe(async () => await ExecuteUpdatePhotoCommand());
        }

        public override async Task OnAppeared()
        {
            Title.Value = UIConstants.UpdateContact;
            SetContactProperties();
        }

        private void SetContactProperties()
        {
            ProfilePic.Value = contactToUpdate.ProfilePic;
            Name.Value = contactToUpdate.Name;
            Mobile.Value = contactToUpdate.Mobile;
            Landline.Value = contactToUpdate.Landline;
            IsFavorite.Value = contactToUpdate.IsFavourite ? UIConstants.UnFavorite : UIConstants.Favorite;
        }


        private async Task ExecuteMarkFavoriteCommand()
        {
            IsFavorite.Value = IsFavorite.Value.Equals(UIConstants.Favorite) ? UIConstants.UnFavorite : UIConstants.Favorite;
            await ExecuteFavoriteCommand();
        }

        private async Task ExecuteFavoriteCommand()
        {
            contactToUpdate.IsFavourite = IsFavorite.Value.Equals(UIConstants.UnFavorite);

            int favoriteToggleResult;
            try
            {
                favoriteToggleResult = await dataService.UpdateContact(contactToUpdate);
            }
            catch (Exception ex)
            {
                await RootPage.DisplayAlert("Error updating favorite.", ex.Message, "Hmmmm");
                IsFavorite.Value = IsFavorite.Value.Equals(UIConstants.Favorite) ? UIConstants.UnFavorite : UIConstants.Favorite;
                favoriteToggleResult = -1;
            }

            if (Math.Abs(favoriteToggleResult) != 1)
            {
                await RootPage.DisplayAlert("Contacts", "Error updating favorite.", "Hmmmm");
                IsFavorite.Value = IsFavorite.Value.Equals(UIConstants.Favorite) ? UIConstants.UnFavorite : UIConstants.Favorite;
            }
        }

        private async Task ExecuteUpdatePhotoCommand()
        {
            string[] updateOptions = { UIConstants.TakePhoto, UIConstants.ChooseFromGallery };
            string chosenOption = await RootPage.DisplayActionSheet(UIConstants.UpdatePhoto, "Cancel", null, updateOptions);

            if (!string.IsNullOrEmpty(chosenOption))
            {
                if (chosenOption.Equals(UIConstants.TakePhoto))
                {
                    await TakePhoto();
                }
                else if (chosenOption.Equals(UIConstants.ChooseFromGallery))
                {
                    await ChoosePhoto();
                }
            }
        }

        private async Task TakePhoto()
        {
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await RootPage.DisplayAlert("Take Photo", "Operation not supported.", "Hmmmm");
                return;
            }

            try
            {
                await ClickPhoto();
            }
            catch
            {
                bool permitted = await RequestForRequiredPermissions();
                if (permitted)
                {
                    await TakePhoto();
                }
                else
                {
                    await RootPage.DisplayAlert("Take Photo", "Permission not granted!", "Hmmmm");
                }
            }
        }

        private async Task<bool> RequestForRequiredPermissions()
        {
            bool choose = await RootPage.DisplayAlert("Update Photo", "Grant Permission to take photo.", "Hmmmm", "No");
            if (choose)
            {
                choose = await CheckCameraAndStoragePermissions();
            }
            return choose;
        }

        private async Task ClickPhoto()
        {
            var image = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                DefaultCamera = CameraDevice.Rear,
                CompressionQuality = 50
            });
            if (null != image)
            {
                ProfilePic.Value = image.Path;
            }
        }

        private async Task ChoosePhoto()
        {
            await CrossMedia.Current.Initialize();
            try
            {
                await ChoosePhotoInner();
            }
            catch
            {
                bool permitted = await RequestForRequiredPermissions();
                if (permitted)
                {
                    await ChoosePhoto();
                }
                else
                {
                    await RootPage.DisplayAlert("Choose Photo", "Permission not granted!", "Hmmmm");
                }
            }
        }

        private async Task ChoosePhotoInner()
        {
            var image = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
            {
                CompressionQuality = 50
            });
            if (null != image)
            {
                ProfilePic.Value = image.Path;
            }
        }

        private async Task ExecuteDeleteCommand()
        {

            bool confirmDelete = await RootPage.DisplayAlert("Delete Contact", $"Do you want to delete {contactToUpdate.Name}?", "Hmmm", "No");
            if (!confirmDelete) return;

            int deleteResult;
            try
            {
                deleteResult = await dataService.DeleteContact(contactToUpdate);
            }
            catch (Exception ex)
            {
                await RootPage.DisplayAlert("Error deleting contact.", ex.Message, "Hmmmm");
                deleteResult = -1;
            }

            if (deleteResult == 1)
            {
                await RootPage.DisplayAlert("Update Contact", $"Contact '{contactToUpdate.Name}' deleted!", "Hmmmm");
                await NavigateSuccess();
            }
            else if (deleteResult != -1)
            {
                await RootPage.DisplayAlert("Contacts", "Error deleting contact.", "Hmmmm");
            }
        }

        private async Task ExecuteUpdateCommand()
        {
            contactToUpdate.Name = Name.Value;
            contactToUpdate.Mobile = Mobile.Value;
            contactToUpdate.Landline = Landline.Value;
            contactToUpdate.ProfilePic = ProfilePic.Value;
            contactToUpdate.IsFavourite = IsFavorite.Value.Equals(UIConstants.UnFavorite);

            int updateResult;
            try
            {
                updateResult = await dataService.UpdateContact(contactToUpdate);
            }
            catch (Exception ex)
            {
                await RootPage.DisplayAlert("Error updating contact.", ex.Message, "Hmmmm");
                updateResult = -1;
            }

            if (updateResult == 1)
            {
                await RootPage.DisplayAlert("Update Contact", $"Contact '{Name.Value}' updated!", "Hmmmm");
                await NavigateSuccess();
            }
            else if (updateResult != -1)
            {
                await RootPage.DisplayAlert("Contacts", "Error updating contact.", "Hmmmm");
            }
        }

        private async Task NavigateSuccess()
        {
            ResetForm();
            await navigator.PopAsync();
        }

        private void ResetForm()
        {
            ProfilePic.Value = "ic_add.png";
            Name.Value = string.Empty;
            Mobile.Value = string.Empty;
            Landline.Value = string.Empty;
            IsFavorite.Value = string.Empty;
        }
    }
}
