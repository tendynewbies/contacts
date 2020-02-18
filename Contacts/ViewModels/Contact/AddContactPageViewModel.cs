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
    public class AddContactPageViewModel : ViewModel
    {
        private readonly IDataService dataService;

        public ReactiveProperty<string> ProfilePic { get; set; } = new ReactiveProperty<string>("ic_add.png");
        public ReactiveProperty<string> Name { get; set; } = new ReactiveProperty<string>(string.Empty);
        public ReactiveProperty<string> Mobile { get; set; } = new ReactiveProperty<string>(string.Empty);
        public ReactiveProperty<string> Landline { get; set; } = new ReactiveProperty<string>(string.Empty);
        public ReactiveProperty<string> IsFavorite { get; set; } = new ReactiveProperty<string>(UIConstants.Favorite);

        public ReactiveCommand SaveCommand { get; set; }
        public ReactiveCommand MarkFavoriteCommand { get; set; } = new ReactiveCommand();
        public ReactiveCommand UpdatePhotoCommand { get; set; } = new ReactiveCommand();

        private HomeMenuItem contactListMenuItem;

        public AddContactPageViewModel()
        {
            dataService = DependencyService.Get<IDataService>();
            SaveCommand = Name.Select(a => !string.IsNullOrEmpty(a)).
                          CombineLatest(Mobile.Select(b => !string.IsNullOrEmpty(b)),
                          (a, b) => a && b).ToReactiveCommand();
            SaveCommand.Subscribe(async () => await ExecuteSaveCommand());
            MarkFavoriteCommand.Subscribe(ExecuteMarkFavoriteCommand);
            UpdatePhotoCommand.Subscribe(async () => await ExecuteUpdatePhotoCommand());
        }

        public override async Task OnAppeared()
        {
            Title.Value = UIConstants.AddContact;
        }

        private void ExecuteMarkFavoriteCommand()
        {
            IsFavorite.Value = IsFavorite.Value.Equals(UIConstants.Favorite) ? UIConstants.UnFavorite : UIConstants.Favorite;
        }

        private async Task ExecuteUpdatePhotoCommand()
        {
            string[] updateOptions = { UIConstants.TakePhoto, UIConstants.ChooseFromGallery};
            string chosenOption = await RootPage.DisplayActionSheet(UIConstants.UpdatePhoto, "Cancel", null, updateOptions);

            if(!string.IsNullOrEmpty(chosenOption))
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
                await RootPage.DisplayAlert("Take Photo", "Operation not supported", "Hmmmm");
                return;
            }

            try
            {
                await ClickPhoto();
            }
            catch
            {
                bool permitted = await RequestForRequiredPermissions();
                if(permitted)
                {
                    await TakePhoto();
                }
                else
                {
                    await RootPage.DisplayAlert("Take Photo", "Permission not granted", "Hmmmm");
                }
            }
        }

        private async Task<bool> RequestForRequiredPermissions()
        {
            bool choose = await RootPage.DisplayAlert("Update Photo", "Grant Permission to take photo.", "Hmmmm", "No");
            if(choose)
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
                    await RootPage.DisplayAlert("Choose Photo", "Permission not granted", "Hmmmm");
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

        private async Task ExecuteSaveCommand()
        {
            MyContact contactToBeAdded = new MyContact
            {
                Name = Name.Value,
                Mobile = Mobile.Value,
                Landline = Landline.Value,
                ProfilePic = ProfilePic.Value,
                IsFavourite = IsFavorite.Value.Equals(UIConstants.UnFavorite),

            };

            int addResult;
            try
            {
                addResult = await dataService.AddContact(contactToBeAdded);
            }
            catch(Exception ex)
            {
                await RootPage.DisplayAlert("Error adding contact", ex.Message, "Hmmmm");
                addResult = -1;
            }

            if(addResult == 1)
            {
                await GoToContactList();
            }
            else if(addResult != -1)
            {
                await RootPage.DisplayAlert("Contacts", "Error adding contact", "Hmmmm");
            }

        }

        private async Task GoToContactList()
        {
            await RootPage.DisplayAlert("Add Contact", $"Contact '{Name.Value}' saved", "Hmmmm");
            if (null == contactListMenuItem)
            {
                contactListMenuItem = new HomeMenuItem()
                {
                    Id = MenuItemType.ContactList,
                    Title = UIConstants.ContactList
                };
            }
            ResetForm();
            await RootPage.NavigateFromMenu(contactListMenuItem);
        }

        private void ResetForm()
        {
            ProfilePic.Value = "ic_add.png";
            Name.Value = string.Empty;
            Mobile.Value = string.Empty;
            Landline.Value = string.Empty;
            IsFavorite.Value = UIConstants.Favorite;
        }
    }
}
