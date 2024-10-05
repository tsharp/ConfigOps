using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using Json.More;
using Json.Patch;

namespace ConfigOps.Sdk.Serialization
{
    internal static class JsonExtensions
    {
        public static string MergeJson(this string deltaJson, string targetJson, bool writeIndented = false)
        {
            JsonNode empty = new JsonObject();
            JsonNode target = JsonNode.Parse(targetJson);

            var patch = empty.CreatePatch(deltaJson);
            var result = patch.Apply(target);

            if (result.IsSuccess && result.Result != null)
            {
                return target.ToJsonString(new JsonSerializerOptions()
                {
                    WriteIndented = writeIndented
                });
            }

            throw new InvalidOperationException("Failed to merge json");
        }

        public static string SetJsonProperty(this string json, string propertyName, string propertyValue, bool writeIndented = false)
        {
            JsonNode root = JsonNode.Parse(json);
            root[propertyName] = propertyValue;

            return root.ToJsonString(new System.Text.Json.JsonSerializerOptions()
            {
                WriteIndented = writeIndented
            });
        }

        public static T MergeObjects<T>(this T target, T delta)
            where T : class, new()
        {
            var targetJson = target.ObjectToJson();
            var deltaJson = delta.ObjectToJson();

            return deltaJson
                .MergeJson(targetJson)
                .JsonToObject<T>();
        }

        public static JsonPatch CreateJsonPatch(this string deltaJson)
        {
            JsonNode empty = new JsonObject();
            JsonNode deltaObject = JsonNode.Parse(deltaJson);

            return empty.CreatePatch(deltaObject);
        }

        public static JsonPatch CreateJsonPatch<T>(this T deltaObject, T sourceObject)
        {
            var deltaJson = deltaObject.ObjectToJson();
            var patch = CreateJsonPatch(deltaJson);

            return patch;
        }
    }
}
