using DogFetchApp.Helper;
using DogFetchApp.ViewModels;
using System.Threading.Tasks;
using System.Windows;

namespace DogFetchApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MainViewModel currentViewmodel;
        
        public MainWindow()
        {
            InitializeComponent();
            ApiHelper.InitializeClient();

            currentViewmodel = new MainViewModel();

            DataContext = currentViewmodel;
            Task.Run(() => currentViewmodel.LoadBreedList());
        }

    }
}
