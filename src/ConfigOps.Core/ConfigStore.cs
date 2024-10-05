using System;
using System.Linq;
using System.Threading.Tasks;
using ConfigOps.Core.Persistence;
using ConfigOps.Sdk.Dto;
using ConfigOps.Sdk.Serialization;
using Json.More;
using Json.Patch;

namespace ConfigOps.Core
{
    internal class ConfigStore : IConfigStore
    {
        private readonly IKeyValueStore store;

        public static IConfigStore Create(IKeyValueStore store)
            => new ConfigStore(store);

        private ConfigStore(IKeyValueStore store)
        {
            this.store = store;
        }

        private Task SetValue(string key, object value)
        {
            Console.WriteLine($"Writing Key Value: {key.ToLowerInvariant()}");

            var @bytes = value
                .ObjectToYaml()
                .ToBytes();

            return store.SetAsync(key.ToLowerInvariant(), @bytes);
        }

        private Task SetJson(string key, string config)
        {
            Console.WriteLine($"Writing Key Value: {key.ToLowerInvariant()}");

            var @bytes = config.JsonToYaml()
                .ToBytes();

            return store.SetAsync(key.ToLowerInvariant(), @bytes);
        }

        private Task SetYaml(string key, string config)
        {
            Console.WriteLine($"Writing Key Value: {key.ToLowerInvariant()}");

            // We convert the yaml to json and back to yaml to ensure that the yaml is valid
            var @bytes = config
                .YamlToJson()
                .JsonToYaml()
                .ToBytes();

            return store.SetAsync(key.ToLowerInvariant(), @bytes);
        }

        public Task<bool> Delete(string key)
            => store.RemoveAsync(key.ToLowerInvariant());

        public async Task<T> Get<T>(string key)
            where T : class, new()
        {
            var @bytes = await store.GetAsync(key.ToLowerInvariant());
            return @bytes
                .FromBytes()
                .YamlToObject<T>();
        }

        public async Task<string> Export(ConfigFormat exportType)
        {
            //var result = new Dictionary<string, object>();
            //var keys = await store.ScanAsync(string.Empty);

            //foreach (var key in keys)
            //{
            //    await ExportValue(result, key, key);
            //}

            //if (exportType == ConfigFormat.Yaml)
            //{
            //    return result.SerializeToYaml();
            //}

            //if (exportType == ConfigFormat.Json)
            //{
            //    return result.SerializeToJson();
            //}

            throw new NotSupportedException();
        }

        //private async Task ExportValue(IDictionary<string, object> store, string fullKey, string key)
        //{
        //    var keyParts = key.Split('/');

        //    if (keyParts.Length == 1)
        //    {
        //        store.Add(key, await Get<object>(fullKey));
        //        return;
        //    }

        //    var currentKey = keyParts[0];
        //    var subKey = string.Join('/', keyParts.Skip(1));
        //    var subStore = store.ContainsKey(currentKey) ? store[currentKey] as IDictionary<string, object> : null;

        //    if (store.ContainsKey(currentKey) && subStore == null)
        //    {
        //        throw new NotSupportedException();
        //    }

        //    if (subStore == null)
        //    {
        //        subStore = new Dictionary<string, object>();
        //        store.Add(currentKey, subStore);
        //    }

        //    await ExportValue(subStore, fullKey, subKey);
        //}

        public Task Apply(ConfigFormat configFormat, string content, string @namespace)
        {
            switch (configFormat)
            {
                case ConfigFormat.Json:
                    return Apply(content.JsonToObject<SystemObject>(), @namespace);
                case ConfigFormat.Yaml:
                    return ApplyAll(@namespace, content.MultiYamlToObject<SystemObject>().ToArray());
                default:
                    throw new NotSupportedException();
            }
        }

        private async Task ApplyAll(string @namespace, params SystemObject[] systemObject)
        {
            // Validate All Before Applying
            foreach (var obj in systemObject)
            {
                ValidateSystemObject(obj);
            }

            // Apply All
            foreach (var obj in systemObject)
            {
                await Apply(obj, @namespace);
            }
        }

        private async Task Apply(SystemObject systemObject, string @namespace)
        {
            ValidateSystemObject(systemObject);

            await SetValue(GetSystemObjectKey(systemObject, @namespace), systemObject);
        }

        private void ValidateSystemObject(SystemObject systemObject)
        {
            if (string.IsNullOrEmpty(systemObject.ApiVersion))
            {
                throw new ArgumentException("ApiVersion is required", nameof(systemObject.ApiVersion));
            }

            if (string.IsNullOrEmpty(systemObject.Kind))
            {
                throw new ArgumentException("Kind is required", nameof(systemObject.Kind));
            }
        }

        private string GetSystemObjectKey(SystemObject systemObject, string @namespace)
        {
            var currentNamespaceDefault = @namespace ?? "default";

            if (string.IsNullOrWhiteSpace(currentNamespaceDefault))
            {
                throw new ArgumentException("Namespace is required", nameof(@namespace));
            }

            systemObject.Metadata = systemObject.Metadata ?? new SystemObjectMetadata();
            systemObject.Metadata.Name = systemObject.Metadata.Name ?? Guid.NewGuid().ToString();

            if (string.IsNullOrWhiteSpace(systemObject.Metadata.Namespace))
            {
                systemObject.Metadata.Namespace = currentNamespaceDefault;
            }

            systemObject.ApiVersion = systemObject.ApiVersion.Trim();
            systemObject.Kind = systemObject.Kind.Trim();
            systemObject.Metadata.Name = systemObject.Metadata.Name.Trim();
            systemObject.Metadata.Namespace = systemObject.Metadata.Namespace.Trim();

            // var key = $"{systemObject.Metadata.Namespace}/{systemObject.Kind}/{systemObject.ApiVersion}/{systemObject.Metadata.Name}";
            var key = $"{systemObject.Metadata.Namespace}/{systemObject.Kind}/{systemObject.Metadata.Name}";

            return key.ToLowerInvariant();
        }

        public async Task Patch(ConfigFormat configFormat, string config, string @namespace)
        {
            var delta = config.YamlToObject<SystemObject>();
            var key = GetSystemObjectKey(delta, @namespace);

            var target = await Get<SystemObject>(key);

            if (!target.ApiVersion.Equals(delta.ApiVersion))
            {
                throw new Exception("ApiVersion mismatch");
            }

            if (!target.Kind.Equals(delta.Kind))
            {
                throw new Exception("Kind mismatch");
            }

            if (!target.Metadata.Name.Equals(delta.Metadata.Name))
            {
                throw new Exception("Name mismatch");
            }

            if (!target.Metadata.Namespace.Equals(delta.Metadata.Namespace))
            {
                throw new Exception($"Namespace mismatch: {delta.Metadata.Namespace} -> {target.Metadata.Namespace}");
            }


            var patch = delta.CreateJsonPatch(target);

            Console.WriteLine(delta.ObjectToYaml());
            Console.WriteLine(target.ObjectToYaml());
            Console.WriteLine(patch.ToJsonDocument().RootElement.GetRawText());
        }
    }
}
