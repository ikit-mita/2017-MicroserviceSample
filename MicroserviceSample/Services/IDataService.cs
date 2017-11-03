using System.Threading.Tasks;

namespace MicroserviceSample.Services
{
    public interface IDataService
    {
        Task<string> GetDataAsync();
        Task SaveDataAsync(string name);
    }
}
