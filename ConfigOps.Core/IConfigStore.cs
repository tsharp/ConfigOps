using System.Threading.Tasks;

namespace ConfigOps.Core
{
    internal interface IConfigStore
    {
        Task Apply(string key, object config);
        Task ApplyJson(string key, string config);
        Task ApplyYaml(string key, string config);
        Task<T> Get<T>(string key);
        Task<bool> Delete(string key);

        /// <summary>
        /// Export the entire configuration store to a raw json or yaml string.
        /// </summary>
        /// <returns></returns>
        Task<string> Export(ExportType exportType);
        Task Import(string json);
    }
}
