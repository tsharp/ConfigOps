using System.Threading.Tasks;

namespace ConfigOps.Core
{
    internal interface IConfigStore
    {
        //Task SetValue(string key, object config);
        //Task SetJson(string key, string config);
        //Task SetYaml(string key, string config);
        Task<T> Get<T>(string key) where T : class, new();
        Task<bool> Delete(string key);
        Task<string> Export(ConfigFormat configFormat);
        Task Apply(ConfigFormat configFormat, string config, string @namespace);
        Task Patch(ConfigFormat configFormat, string config, string @namespace);
    }
}
