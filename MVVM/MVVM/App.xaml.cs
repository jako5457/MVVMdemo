using MVVM.Services;
using Xamarin.Forms;
using TinyIoC;

namespace MVVM
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var container = TinyIoCContainer.Current;
            container.Register<IPersonService, PersonService>();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
