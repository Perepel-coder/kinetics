using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace kinetics.Model
{
    public enum Result { True, False, Error }
    public interface IKinetics { Result Process(); }
    public struct Coord
    {
        public int Number { get; set; }
        public double Time_T { get; set; }
        public double Concentration_C { get; set; }
    }
    public class KineticsClass : ReactiveObject, IKinetics
    {
        private double k;
        private double kt;
        private List<double> speed;
        private double concentrationA;
        private double concentrationB;
        private List<double> concentrationBt;
        private double errorRateB;
        private double errorRateK;
        private List<double> time;
        private int NumberOfExperiments;
        private ObservableCollection<Coord> points;
        private ObservableCollection<Coord> pointsSp;
        #region field properties
        public int GetNumberOfExperiments
        {
            get { return NumberOfExperiments; }
            set { this.RaiseAndSetIfChanged(ref NumberOfExperiments, value); }
        }
        public ObservableCollection<Coord> GetPoints
        {
            get { return points; }
            set { this.RaiseAndSetIfChanged(ref points, value); }
        }
        public ObservableCollection<Coord> GetPointsSp
        {
            get { return pointsSp; }
            set { this.RaiseAndSetIfChanged(ref pointsSp, value); }
        }
        public double GetK
        {
            get { return k; }
            set { this.RaiseAndSetIfChanged(ref k, value); }
        }
        public double GetKt
        {
            get { return kt; }
            set { this.RaiseAndSetIfChanged(ref kt, value); }
        }
        public List<double> GetSpeed
        {
            get { return speed; }
            set { this.RaiseAndSetIfChanged(ref speed, value); }
        }
        public double GetConcentrationA
        {
            get { return concentrationA; }
            set { this.RaiseAndSetIfChanged(ref concentrationA, value); }
        }
        public double GetConcentrationB
        {
            get { return concentrationB; }
            set { this.RaiseAndSetIfChanged(ref concentrationB, value); }
        }
        public List<double> GetConcentrationBt
        {
            get { return concentrationBt; }
            set { this.RaiseAndSetIfChanged(ref concentrationBt, value); }
        }
        public double GetErrorRateB
        {
            get { return errorRateB; }
            set { this.RaiseAndSetIfChanged(ref errorRateB, value); }
        }
        public double GetErrorRateK
        {
            get { return errorRateK; }
            set { this.RaiseAndSetIfChanged(ref errorRateK, value); }
        }
        public List<double> GetTime
        {
            get { return time; }
            set { this.RaiseAndSetIfChanged(ref time, value); }
        }
        #endregion
        public KineticsClass()
        {
            GetK = 1; GetKt = GetK;
            GetConcentrationA = 100;
            GetConcentrationB = 0;
            GetErrorRateB = 20;
            GetErrorRateK = 20;
            GetTime = new List<double> { 0 };
            GetSpeed = new List<double>();
            GetConcentrationBt = new List<double>();
            GetPoints = new ObservableCollection<Coord>();
            GetPointsSp = new ObservableCollection<Coord>();
            GetNumberOfExperiments = 0;
        }
        public Result Process()
        {
            try
            {
                if (GetConcentrationB > GetConcentrationA || GetK == 0)
                {
                    throw new Exception("Невозможно достичь требуемой концентрации. Измените начальные условия");
                }
                else
                {
                    while ((GetConcentrationBt.Count == 0 || GetConcentrationBt.Last() > GetConcentrationB))
                    {
                        var pow = GetKt * GetTime.Last() * (-1);
                        var Bt = Math.Round(GetConcentrationA * Math.Pow(Math.E, pow), 2);
                        GetSpeed.Add(Math.Round(GetKt * Bt, 2));
                        GetTime.Add(GetTime.Last() + 0.1);
                        GetConcentrationBt.Add(Bt);
                    }
                    return Result.False;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message}", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information);
                return Result.Error;
            }
        }
        public Result CalculationErrors()
        {
            var ErrorB = 100 - GetConcentrationBt.Last() * 100 / GetConcentrationB;
            if (ErrorB < GetErrorRateB || GetConcentrationB == 0)
            {
                GetPoints.Clear(); GetPointsSp.Clear();
                for (int i = 0; i < GetConcentrationBt.Count(); i++)
                {
                    GetPoints.Add(new Coord { Number = i + 1, Time_T = GetTime[i], Concentration_C = GetConcentrationBt[i] });
                    GetPointsSp.Add(new Coord { Number = i + 1, Time_T = GetSpeed[i], Concentration_C = GetConcentrationBt[i] });
                }
                GetNumberOfExperiments = GetConcentrationBt.Count();
                return Result.True;
            }
            else
            {
                var MaxK = GetK + GetK * (GetErrorRateK / 100);
                var ki = GetKt + GetK * 0.01;
                try
                {
                    if (ki <= MaxK)
                    {
                        GetKt = ki;
                        GetTime.Clear(); GetTime.Add(0);
                        GetSpeed.Clear(); GetConcentrationBt.Clear();
                    }
                    else { throw new Exception("Невозможно достичь требуемой концентрации. Измените начальные условия"); }
                }
                catch (Exception e)
                {
                    MessageBox.Show($"{e.Message}", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Information);
                    return Result.Error;
                }
                return Result.False;
            }
        }
    }
}
