using Microsoft.Office.Interop.Excel;
using System.Collections.ObjectModel;
namespace kinetics.Model
{
    public static class SaveData
    {
        public static void SaveFile(Application xlApp, KineticsClass kineticsClass)
        {
            xlApp.SheetsInNewWorkbook = 1;
            var workBook = xlApp.Workbooks.Add();                   // создать книгу
            var xlWorkSheet = (Worksheet)xlApp.Worksheets.get_Item(1);  // создать страницу
            xlWorkSheet.Columns.ColumnWidth = 30;
            xlWorkSheet.Cells[1, 1] = "Концентрация начального раствора (A): ";
            xlWorkSheet.Cells[2, 1] = "Концентрация конечного раствора (Aкон): ";
            xlWorkSheet.Cells[3, 1] = "Константа скорости растворения (К): ";
            xlWorkSheet.Cells[4, 1] = "Погрешность определения скорости % (K1):";
            xlWorkSheet.Cells[5, 1] = "Погрешность значения концентрации (Акон) % :";
            xlWorkSheet.Cells[1, 2] = kineticsClass.GetConcentrationA;
            xlWorkSheet.Cells[2, 2] = kineticsClass.GetConcentrationB;
            xlWorkSheet.Cells[3, 2] = kineticsClass.GetK;
            xlWorkSheet.Cells[4, 2] = kineticsClass.GetErrorRateK;
            xlWorkSheet.Cells[5, 2] = kineticsClass.GetErrorRateB;
        }
        public static void SaveFile(Application xlApp, ObservableCollection<Coord> tc, ObservableCollection<Coord> tv)
        {
            xlApp.SheetsInNewWorkbook = 1;
            var workBook = xlApp.Workbooks.Add();                   // создать книгу
            var xlWorkSheet = (Worksheet)xlApp.Worksheets.get_Item(1);  // создать страницу
            xlWorkSheet.Columns.ColumnWidth = 30;
            for (int i = 3; i <= tc.Count + 2; i++)
            {
                xlWorkSheet.Cells[i, 1] = tc[i - 3].Time_T;
                xlWorkSheet.Cells[i, 2] = tc[i - 3].Concentration_C;
            }
            xlWorkSheet.Cells[2, 1] = "T";
            xlWorkSheet.Cells[2, 2] = "C";
            //----------------------------------------------------------------------------------------
            var chart = xlWorkSheet.ChartObjects().Add(150, 50, 350, 250).Chart;
            SeriesCollection seriesCollection = (SeriesCollection)chart.SeriesCollection();
            Series series = seriesCollection.NewSeries();
            series.XValues = xlWorkSheet.get_Range("A3", "A" + (tc.Count + 1).ToString());
            series.Values = xlWorkSheet.get_Range("B3", "B" + (tc.Count + 1).ToString());
            chart.ChartType = XlChartType.xlXYScatterLines;
            //-----------------------------------------------------------------------------------------
            for (int i = 3; i <= tv.Count + 2; i++)
            {
                xlWorkSheet.Cells[i, 4] = tv[i - 3].Time_T;
                xlWorkSheet.Cells[i, 5] = tv[i - 3].Concentration_C;
            }
            xlWorkSheet.Cells[2, 4] = "T";
            xlWorkSheet.Cells[2, 5] = "V";
            //----------------------------------------------------------------------------------------
            ChartObjects xlCharts2 = (ChartObjects)xlWorkSheet.ChartObjects();
            ChartObject myChart2 = (ChartObject)xlCharts2.Add(250, 150, 450, 350);
            var chart2 = myChart2.Chart;
            SeriesCollection seriesCollection2 = (SeriesCollection)chart2.SeriesCollection();
            Series series2 = seriesCollection2.NewSeries();
            series2.XValues = xlWorkSheet.get_Range("D3", "D" + (tv.Count + 1).ToString());
            series2.Values = xlWorkSheet.get_Range("E3", "E" + (tv.Count + 1).ToString());
            chart2.ChartType = XlChartType.xlXYScatterLines;
        }
    }
}
