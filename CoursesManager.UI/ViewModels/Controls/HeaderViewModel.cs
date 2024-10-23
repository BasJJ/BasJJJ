using System.Windows.Input;
using CoursesManager.MVVM.Commands;
using CoursesManager.MVVM.Data;
using CoursesManager.MVVM.Messages;
using CoursesManager.MVVM.Navigation;
using CoursesManager.UI.Messages;

namespace CoursesManager.UI.ViewModels.Controls;

public class HeaderViewModel : NavigatableViewModel
{
    public INavigationService NavigationService => _navigationService;
    private readonly IMessageBroker _messageBroker;

    public ICommand CloseCommand { get; private set; }

    public HeaderViewModel(INavigationService navigationService, IMessageBroker messageBroker) : base(navigationService)
    {
        _messageBroker = messageBroker;

        CloseCommand = new RelayCommand(() =>
        {
            messageBroker.Publish<ApplicationCloseRequestedMessage>(new ApplicationCloseRequestedMessage());
        });
    }
}