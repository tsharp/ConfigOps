using System.Threading.Tasks;

namespace ConfigOps.Core
{
    internal interface IConfigStore
    {
        Task SetValue(string key, object config);
        Task SetJson(string key, string config);
        Task SetYaml(string key, string config);
        Task<T> Get<T>(string key);
        Task<bool> Delete(string key);
        Task<string> Export(ConfigFormat configFormat);
        Task Apply(ConfigFormat configFormat, string config);
    }
}
