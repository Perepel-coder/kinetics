using Microsoft.Office.Interop.Excel;
namespace kinetics.Servise
{
    public interface IFileService
    {
        string Open(string filename);
        void Save(string filename, Application FileArray);
    }
}
