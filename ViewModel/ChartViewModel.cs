using kinetics.Model;
using ReactiveUI;

namespace kinetics.ViewModel
{
    public class ChartViewModel : ReactiveObject
    {
        private KineticsClass kineticsClass;
        public KineticsClass GetKineticsClass
        {
            get { return kineticsClass; }
            set { this.RaiseAndSetIfChanged(ref kineticsClass, value); }
        }
        public ChartViewModel(KineticsClass kineticsClass)
        {
            GetKineticsClass = kineticsClass;
        }
    }
}
