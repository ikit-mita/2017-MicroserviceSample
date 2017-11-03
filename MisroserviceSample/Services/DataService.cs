using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MisroserviceSample.Options;

namespace MisroserviceSample.Services
{
    public class DataService : IDataService
    {
        public DataService(IOptions<DataOptions> options)
        {
            Name = options.Value?.Default ?? "!!You missed value!!";
        }

        private string Name { get; set; }

        public async Task<string> GetDataAsync()
        {
            await Task.Delay(200);
            return Name;
        }

        public async Task SaveDataAsync(string name)
        {
            await Task.Delay(150);
            Name = name;
        }
    }
}
