﻿// Her er tilføjet Commanding Properties, som der bindes til
// i View'et. Bemærk at DeleteCommand er lavet med en anonymous 
// metode, hvorimod alle de andre er lavet med en normal metode.
// Der er dog et problem med ShowAgeCommand, som skal åbne en 
// DisplayAlert i View'et - men det kan ikke umiddelbart virke.
// Her er lavet en "hård" afhængighed ved at benytte Application.Current.MainPage.DisplayAlert()..
// Det er dog ikke nogen god løsning! Så er det godt at vi har Messages!

using MVVM.Models;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace MVVM.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        public ObservableCollection<Person> Persons { get; }

        #region CONSTRUCTOR
        public MainPageViewModel()
        {
            Persons = new ObservableCollection<Person>
                {
                    new Person { Name = "Anna", Age = 27 },
                    new Person { Name = "Christian", Age = 32 },
                    new Person { Name = "Helle", Age = 54 }
                };
        }
        #endregion

        #region PROPERTY CHANGE NOTIFICATION
        Person _personSelectedItem;
        public Person PersonSelectedItem
        {
            get { return _personSelectedItem; }
            set
            {
                if (SetProperty(ref _personSelectedItem, value))
                {
                    Name = value.Name;
                    Age = value.Age;
                    RefreshCanExecutes();
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
        // 1. Command with explicit Execute method
        private Command clearEntriesCommand;
        public Command ClearEntriesCommand => clearEntriesCommand ??= new Command(ExecuteClearEntries);

        private void ExecuteClearEntries()
        {
            Name = string.Empty;
            Age = 0;
        }

        // 2. Command with explicit Execute and CanExecute methods
        private Command addCommand;
        public Command AddCommand => addCommand ??= new Command(ExecuteAddCommand, CanExecuteAddCommand);

        private void ExecuteAddCommand(object obj)
        {
            Persons.Add(new Person { Name = Name, Age = Age });
        }

        private bool CanExecuteAddCommand(object arg)
        {
            return Name?.Length > 0 && Age > 0;
        }


        // 3. Commands with inline methods
        private Command showAgeCommand;
        public Command ShowAgeCommand => showAgeCommand ??= new Command(
            execute: () => Application.Current.MainPage.DisplayAlert("AgeButtonClicked", $"{PersonSelectedItem.Name} er {PersonSelectedItem.Age}", "OK"),
            canExecute: () => _personSelectedItem != null
            );


        private Command makeOlderCommand;
        public Command MakeOlderCommand => makeOlderCommand ??= new Command(
            execute: () =>
            {
                Age++;
                _personSelectedItem.Age = Age;
                RefreshCanExecutes();
            },
            canExecute: () => _personSelectedItem != null
            );


        // 4. Command med parameter
        private Command answerToLifeCommand;
        public Command AnswerToLifeCommand => answerToLifeCommand ?? new Command<string>
            (
                execute: (string param) => Application.Current.MainPage.DisplayAlert("AnswerToLifeClicked", $"{param}", "OK")
            );


        // 5. Update of CanExecute()
        void RefreshCanExecutes()                                                   
        {       
            AddCommand.ChangeCanExecute();
            ShowAgeCommand.ChangeCanExecute();
            MakeOlderCommand.ChangeCanExecute();
        }
        #endregion
    }
}
