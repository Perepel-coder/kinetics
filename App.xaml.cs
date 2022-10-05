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
            var builder = new ContainerBuilder();
            builder.RegisterModule<ProjectModule>();
            builder.Register(c => new MainViewModel());
            var container = builder.Build();
            ProjectModule.container = container;
            var model = container.Resolve<MainViewModel>();
            var view = new MainWindow { DataContext = model };
            view.Show();
        }
    }
}
