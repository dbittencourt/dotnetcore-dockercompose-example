using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.Shared.Interfaces
{
    public interface IHttpClient
    {
        Task<string> RequestAsync(string address, IDictionary<string, string> optional);
        Task<T> RequestAsync<T>(string address, IDictionary<string, string> optional, bool handleType = false) where T : class, new();
    }
}