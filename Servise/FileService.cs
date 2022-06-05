using Microsoft.Office.Interop.Excel;
using System;
using System.IO;

namespace kinetics.Servise
{
    public class FileService : IFileService
    {
        public string Open(string filename)
        {
            string MyArray;
            FileStream fs = new FileStream(filename, FileMode.OpenOrCreate); //создаем файловый поток
            StreamReader reader = new StreamReader(fs); // создаем «потоковый читатель» и связываем его с файловым потоком
            MyArray = reader.ReadToEnd(); //считываем все данные с потока и выводим на экран
            reader.Close(); //закрываем поток
            return MyArray;
        }
        public void Save(string filename, Application xlApp)
        {
            xlApp.DisplayAlerts = false;
            xlApp.Application.ActiveWorkbook.SaveAs(new FileInfo(filename), Type.Missing,
                 Type.Missing, Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlNoChange,
                 Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            xlApp.Quit();
        }
    }
}
