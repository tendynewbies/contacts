using System.Threading.Tasks;
using Contacts.Helpers;
using Contacts.Views.Home;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Reactive.Bindings;
using Xamarin.Forms;

namespace Contacts.ViewModels
{
    public class ViewModel : ILoadable
    {
        public HamburgerMenuPage RootPage { get => Application.Current.MainPage as HamburgerMenuPage; }
        public ReactiveProperty<string> Title { get; set; } = new ReactiveProperty<string>(UIConstants.ContactList);

        public virtual async Task OnAppeared() { }

        public virtual async Task OnDisappearing() { }


        public async Task<bool> CheckCameraAndStoragePermissions()
        {
            bool granted = true;

            var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);

            if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera, Permission.Storage });
                cameraStatus = results[Permission.Camera];
                storageStatus = results[Permission.Storage];
            }

            granted = (cameraStatus == PermissionStatus.Granted) && (storageStatus == PermissionStatus.Granted);

            return granted;
        }
    }
}
