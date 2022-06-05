using Autofac;
using kinetics.Model;
using kinetics.Servise;
using ReactiveUI;
using System.Windows;
using System.Windows.Input;

namespace kinetics.ViewModel
{
    public class MainViewModel : ReactiveObject
    {
        private IContainer container = Container.GetBuilder().Build();
        private KineticsClass kineticsClass;
        private int step;
        public int GetStep
        {
            get { return step; }
            set { this.RaiseAndSetIfChanged(ref step, value); }
        }
        public KineticsClass GetKineticsClass
        {
            get { return kineticsClass; }
            set { this.RaiseAndSetIfChanged(ref kineticsClass, value); }
        }
        public DialogService dialogService { get; set; }
        public FileService fileService { get; set; }
        public MainViewModel()
        {
            GetKineticsClass = container.Resolve<KineticsClass>();
            dialogService = container.Resolve<DialogService>();
            fileService = container.Resolve<FileService>();
        }
        private Command start;
        public ICommand Start
        {
            get
            {
                return start ??
                  (start = new Command(obj =>
                  {
                      Result result = Result.False;
                      GetKineticsClass.GetNumberOfExperiments = 0;
                      GetKineticsClass.GetTime.Clear(); GetKineticsClass.GetTime.Add(0);
                      GetKineticsClass.GetKt = GetKineticsClass.GetK;
                      GetKineticsClass.GetSpeed.Clear(); GetKineticsClass.GetConcentrationBt.Clear();
                      while (result == Result.False)
                      {
                          result = GetKineticsClass.Process();
                          if (result != Result.Error) { result = GetKineticsClass.CalculationErrors(); }
                      }
                      if(result == Result.True) { GetKineticsClass.GetK = GetKineticsClass.GetKt; }
                  }));
            }
        }
        private Command clear;
        public ICommand Clear
        {
            get
            {
                return clear ??
                  (clear = new Command(obj =>
                  {
                      GetKineticsClass = container.Resolve<KineticsClass>();
                      GetKineticsClass.GetPoints.Clear();
                  }));
            }
        }
        private Command chart;
        public ICommand Chart
        {
            get
            {
                return chart ??
                  (chart = new Command(obj =>
                  {
                      var Chart = new Chart { DataContext = container.Resolve<ChartViewModel>(new NamedParameter("p1", GetKineticsClass)) };
                      Chart.ShowDialog();
                  }));
            }
        }
        public Command saveInput;
        public ICommand SaveInput
        {
            get
            {
                return saveInput ??
                  (saveInput = new Command(obj =>
                  {
                      var xlApp = new Microsoft.Office.Interop.Excel.Application();
                      try
                      {
                          if (xlApp == null)
                          {
                              MessageBox.Show("Excel не установлен на вашем устройстве", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                              return;
                          }
                          if (!dialogService.SaveFileDialog()) { return; }
                          SaveData.SaveFile(xlApp, GetKineticsClass);
                          fileService.Save(dialogService.FilePath, xlApp);
                      }
                      catch
                      {
                          MessageBox.Show("Не удалось записать в файл!!!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                          xlApp.DisplayAlerts = false;
                          xlApp.Quit();
                      }
                  }));
            }
        }
        public Command saveOutput;
        public ICommand SaveOutput
        {
            get
            {
                return saveOutput ??
                  (saveOutput = new Command(obj =>
                  {
                      var xlApp = new Microsoft.Office.Interop.Excel.Application();
                      try
                      {
                          if (xlApp == null)
                          {
                              MessageBox.Show("Excel не установлен на вашем устройстве", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                              return;
                          }
                          if (!dialogService.SaveFileDialog()) { return; }
                          SaveData.SaveFile(xlApp, GetKineticsClass.GetPoints, GetKineticsClass.GetPointsSp);
                          fileService.Save(dialogService.FilePath, xlApp);
                      }
                      catch
                      {
                          MessageBox.Show("Не удалось записать в файл!!!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                          xlApp.DisplayAlerts = false;
                          xlApp.Quit();
                      }
                  }));
            }
        }
    }
}
