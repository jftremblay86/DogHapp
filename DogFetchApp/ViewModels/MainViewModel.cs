using DogFetchApp.Commands;
using DogFetchApp.Helper;
using DogFetchApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
namespace DogFetchApp.ViewModels
{
    class MainViewModel : BaseViewModel
    {
        private string currentBreed;
        private string selectedBreed;

        private int selectedAmount;
        private int currentAmount;

        private ObservableCollection<string> images;
        private ObservableCollection<string> breeds;

        public List<int> Quantities { get; set; } = new List<int> { 1, 2, 3, 4, 5, 10 };

        public AsyncCommand<object> FetchImage { get; private set; }
        public AsyncCommand<object> NextImage { get; private set; }


        public DelegateCommand<object> restart { get; private set; }
        public DelegateCommand<string> ChangeLanguageCmd { get; private set; }


        public ObservableCollection<string> Images
        {
            get => images;
            set
            {
                images = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<string> Breeds
        {
            get => breeds;
            set
            {
                breeds = value;
                OnPropertyChanged();
            }
        }
        public string CurrentBreed
        {
            get => currentBreed;
            set
            {
                currentBreed = value;
                OnPropertyChanged();
            }
        }
        public string SelectedBreed
        {
            get => selectedBreed;
            set
            {
                selectedBreed = value;
                OnPropertyChanged();
            }
        }



        public int SelectedAmount
        {
            get => selectedAmount;
            set
            {
                selectedAmount = value;
                OnPropertyChanged();
            }
        }
        public int CurrentAmount
        {
            get => currentAmount;
            set
            {
                currentAmount = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            Breeds = new ObservableCollection<string>();
            Images = new ObservableCollection<string>();

            FetchImage = new AsyncCommand<object>(FetchImageAsync);
            NextImage = new AsyncCommand<object>(NextImageAsync);


            ChangeLanguageCmd = new DelegateCommand<string>(changeLanguage);

        }

        public async Task LoadBreedList()
        {
            Breeds = await DogApiProcessor.LoadBreedList();

        }
        private async Task FetchImageAsync(object arg)
        {
            CurrentBreed = SelectedBreed;
            currentAmount = SelectedAmount;
            int i = 0;
            Images.Clear();
            while (i < SelectedAmount)
            {
                DogModel dogImage = await DogApiProcessor.GetImageUrl(SelectedBreed);
                Images.Add(dogImage.DogPicture);
                i++;
            }

        }

        private async Task NextImageAsync(object arg)
        {
            int i = 0;
            Images.Clear();
            while (i < CurrentAmount)
            {
                DogModel dogImage = await DogApiProcessor.GetImageUrl(CurrentBreed);
                Images.Add(dogImage.DogPicture);
                i++;
            }

        }
        public void Restart()
        {

            var nom = Application.ResourceAssembly.Location;
            var newFichier = Path.ChangeExtension(nom, ".exe");
            Process.Start(newFichier);
            Application.Current.Shutdown();
        }

        private void changeLanguage(string arg)
        {
            Properties.Settings.Default.Language = arg;

            Properties.Settings.Default.Save();
            if (MessageBox.Show(
                    "Please restart app for the settings to take effect.\nWould you like to restart?",
                    "Warning!",
                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                Restart();
        }

    }
}
