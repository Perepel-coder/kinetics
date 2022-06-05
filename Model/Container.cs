using Autofac;
using kinetics.Servise;
using kinetics.ViewModel;

namespace kinetics.Model
{
    public static class Container
    {
        public static ContainerBuilder GetBuilder()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<DialogService>().AsSelf().As<IDialogService>();
            builder.RegisterType<FileService>().AsSelf().As<IFileService>();
            builder.RegisterType<KineticsClass>().AsSelf().As<IKinetics>();
            builder.Register((c, p) => new ChartViewModel(p.Named<KineticsClass>("p1"))).AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf();
            return builder;
        }
    }
}
