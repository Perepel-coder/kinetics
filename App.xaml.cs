using Autofac;
using kinetics.Model;
using kinetics.ViewModel;
using System.Windows;

namespace kinetics
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void _Startup(object sender, StartupEventArgs e)
        {
            var container = Container.GetBuilder().Build();
            var view = new MainWindow { DataContext = container.Resolve<MainViewModel>() };
            view.Show();
        }
    }
}
