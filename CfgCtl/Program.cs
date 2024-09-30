using ConfigOps.Core;
using ConfigOps.Core.Persistence;

namespace CfgCtl
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            IKeyValueStore fileSystemKeyValueStore = FileSystemKeyValueStore.Create("C:\\testing\\sample-store");
            IConfigStore configStore = ConfigStore.Create(fileSystemKeyValueStore);

            var yamlConfig = await configStore.Export(ExportType.Yaml);

            File.WriteAllText("C:\\testing\\sample-store.yaml", yamlConfig);

            // cfgctl apply -f "config.yaml"
        }
    }
}
