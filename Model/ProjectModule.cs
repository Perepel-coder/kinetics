using Autofac;
using kinetics.Servise;
using kinetics.ViewModel;

namespace kinetics.Model
{
    internal class ProjectModule : Module
    {
        internal static IContainer container { get; set; }
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DialogService>().AsSelf().As<IDialogService>();
            builder.RegisterType<FileService>().AsSelf().As<IFileService>();
            builder.RegisterType<KineticsClass>().AsSelf().As<IKinetics>();
            builder.Register((c, p) => new  ChartViewModel(p.Named<KineticsClass>("p1"))).AsSelf();
            builder.Register((c, p) => new Chart() { DataContext = p.Named<ChartViewModel>("p1")}).AsSelf();
        }
    }
}
