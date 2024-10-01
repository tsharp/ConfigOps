using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ConfigOps.Core.Persistence;
using ConfigOps.Core.Serialization;

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

        public Task SetValue(string key, object value)
        {
            var @bytes = value.SerializeToJsonBytes();
            return store.SetAsync(key, @bytes);
        }

        public Task SetJson(string key, string config)
        {
            var @object = config.DeserializeFromJson<object>();
            var @bytes = @object.SerializeToJsonBytes();

            return store.SetAsync(key, @bytes);
        }

        public Task SetYaml(string key, string config)
        {
            var @object = config.DeserializeFromYaml<IDictionary<string, object>>();
            var @bytes = @object.SerializeToJsonBytes();

            return store.SetAsync(key, @bytes);
        }

        public Task<bool> Delete(string key)
            => store.RemoveAsync(key);

        public async Task<T> Get<T>(string key)
        {
            var @bytes = await store.GetAsync(key);
            return @bytes.DeserializeFromJsonBytes<T>();
        }

        public async Task<string> Export(ConfigFormat exportType)
        {
            var result = new Dictionary<string, object>();
            var keys = await store.ScanAsync(string.Empty);

            foreach (var key in keys)
            {
                await ExportValue(result, key, key);
            }

            if (exportType == ConfigFormat.Yaml)
            {
                return result.SerializeToYaml();
            }

            if (exportType == ConfigFormat.Json)
            {
                return result.SerializeToJson();
            }

            throw new NotSupportedException();
        }

        private async Task ExportValue(IDictionary<string, object> store, string fullKey, string key)
        {
            var keyParts = key.Split('/');

            if (keyParts.Length == 1)
            {
                store.Add(key, await Get<object>(fullKey));
                return;
            }

            var currentKey = keyParts[0];
            var subKey = string.Join('/', keyParts.Skip(1));
            var subStore = store.ContainsKey(currentKey) ? store[currentKey] as IDictionary<string, object> : null;

            if (store.ContainsKey(currentKey) && subStore == null)
            {
                throw new NotSupportedException();
            }

            if (subStore == null)
            {
                subStore = new Dictionary<string, object>();
                store.Add(currentKey, subStore);
            }

            await ExportValue(subStore, fullKey, subKey);
        }

        public Task Apply(ConfigFormat configFormat, string content)
        {
            switch (configFormat)
            {
                case ConfigFormat.Json:
                    return Apply(content);
                case ConfigFormat.Yaml:
                    return Apply(content.DeserializeFromYaml<IDictionary<string, object>>().SerializeToJson());
                default:
                    throw new NotSupportedException();
            }
        }

        private async Task Apply(string json, string keyPrefix = "")
        {
            var store = json.DeserializeFromJson<IDictionary<string, object>>();

            await Apply(store, keyPrefix);
        }

        private async Task Apply(IDictionary<string, object> store, string keyPrefix)
        {
            var jsonObjects = store
                .Where(v => v.Value.GetType() == typeof(JsonElement) && ((JsonElement)v.Value).ValueKind == JsonValueKind.Object)
                .ToDictionary(v => v.Key, v => (JsonElement)v.Value);

            var nonObjects = store
                .Where(v => v.Value.GetType() != typeof(JsonElement) || ((JsonElement)v.Value).ValueKind != JsonValueKind.Object)
                .ToDictionary(v => v.Key, v => v.Value);

            if (jsonObjects.Any())
            {
                await ApplyJsonObjects(jsonObjects, keyPrefix);
            }

            if (nonObjects.Any())
            {
                await ApplyDictionary(nonObjects, keyPrefix);
            }
        }

        private async Task ApplyDictionary(IDictionary<string, object> dictionary, string keyPrefix = "")
        {
            foreach (var item in dictionary)
            {
                if (KeyValidator.IsValid(item.Key))
                {
                    await SetJson($"{keyPrefix}/{item.Key}".TrimStart('/'), item.Value.SerializeToJson());
                }
                else
                {
                    Console.WriteLine($"Skipping Invalid Key: {item.Key}");
                }
            }
        }

        private async Task ApplyJsonObjects(IDictionary<string, JsonElement> items, string keyPrefix = "")
        {
            foreach (var item in items)
            {
                var itemValue = item.Value.Deserialize<IDictionary<string, object>>();

                await Apply(itemValue, $"{keyPrefix}/{item.Key}".TrimStart('/'));
            }
        }
    }
}
