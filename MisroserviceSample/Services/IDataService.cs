using System.Threading.Tasks;

namespace MisroserviceSample.Services
{
    public interface IDataService
    {
        Task<string> GetDataAsync();
        Task SaveDataAsync(string name);
    }
}
