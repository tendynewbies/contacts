using System.Threading.Tasks;
using Contacts.Helpers;
using Reactive.Bindings;

namespace Contacts.ViewModels
{
    public class ViewModel : ILoadable
    {
        public ReactiveProperty<string> Title { get; set; } = new ReactiveProperty<string>(UIConstants.ContactList);

        public virtual async Task OnAppeared() { }

        public virtual async Task OnDisappearing() { }
    }
}
