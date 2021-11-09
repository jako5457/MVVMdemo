// Nu kan det lade sig gøre at ViewModel kan sende en Message 
// til View'et og bede om at åbne en DisplayAlert. Dette sker både
// i ShowAgeCommand og AnswerToLifeCommand.
// I View'ets constructor laves et "abonnement" på Messages, både
// uden og med en parameter.

using MVVM.Models;
using MVVM.Services;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using TinyIoC;

namespace MVVM.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public ObservableCollection<Person> Persons { get; private set; }

        private readonly IPersonService _personService;

        #region CONSTRUCTOR
        public MainPageViewModel()
        {
            Persons = new ObservableCollection<Person>();

            _personService = TinyIoCContainer.Current.Resolve<IPersonService>();

            MakeOlderCommand = new Command(
                execute: () =>
                {
                    Age++;
                    _personSelectedItem.Age = Age;
                    RefreshCanExecutes();
                },
                canExecute: () =>
                {
                    return _personSelectedItem != null;
                });

            ClearEntriesCommand = new Command(
               execute: () =>
               {
                   Name = string.Empty;
                   Age = 0;
               });

            AddCommand = new Command(
               execute: () => {
                   _personService.CreatePerson(new Person { Name = Name, Age = Age });
                   UpdateEntries();
               },
               canExecute: () =>
               {
                   return Name?.Length > 0 && Age > 0;
               });

            ShowAgeCommand = new Command(
                execute: () => MessagingCenter.Send(this, "AgeButtonClicked", PersonSelectedItem),
                canExecute: () => _personSelectedItem != null
                );

            AnswerToLife = new Command<string>(
                execute: (string param) => MessagingCenter.Send(this, "AnswerToLifeClicked", param)
                );

            UpdateEntries();
        }
        #endregion

        #region PROPERTY CHANGE NOTIFICATION
        Person _personSelectedItem = new Person();
        public Person PersonSelectedItem
        {
            get { return _personSelectedItem; }
            set
            {
                if (value != null)
                {
                    if (SetProperty(ref _personSelectedItem, value))
                    {
                        Name = value.Name;
                        Age = value.Age;
                        RefreshCanExecutes();
                    }
                }
            }
        }

        string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); RefreshCanExecutes(); }
        }

        int _age;
        public int Age
        {
            get { return _age; }
            set { SetProperty(ref _age, value); RefreshCanExecutes(); }
        }
        #endregion

        #region COMMANDING
        // Properties for implementing commands in constructor.
        public Command MakeOlderCommand { get; private set; }

        public Command AddCommand { get; private set; }

        public Command ClearEntriesCommand { get; private set; }

        public Command ShowAgeCommand { get; private set; }

        public Command AnswerToLife { get; private set; }


        // Property for local implementation (an alternative syntax).
        public Command _onDeleteCommand;
        public Command DeleteCommand
        {
            get
            {
                return _onDeleteCommand ?? (_onDeleteCommand = new Command(
                    execute: () =>
                    {
                        _personService.DeletePerson(_personSelectedItem ?? null);
                        UpdateEntries();
                    },
                    canExecute: () =>
                    {
                        return _personSelectedItem != null && _personService.GetPeople().Count > 1;
                    }
                    ));
            }
        }

        void RefreshCanExecutes()
        {
            DeleteCommand.ChangeCanExecute();
            MakeOlderCommand.ChangeCanExecute();
            AddCommand.ChangeCanExecute();
            ShowAgeCommand.ChangeCanExecute();
        }
        #endregion

        public void UpdateEntries()
        {
            _personSelectedItem = new Person();
            Persons = new ObservableCollection<Person>(_personService.GetPeople());

            OnPropertyChanged(nameof(Persons));
        }
    }
}
