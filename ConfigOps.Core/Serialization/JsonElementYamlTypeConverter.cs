﻿using System;
using System.Text.Json;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace ConfigOps.Core.Serialization;

public class JsonElementYamlTypeConverter : IYamlTypeConverter
{
    public bool Accepts(Type type)
    {
        return type == typeof(JsonElement);
    }

    public object ReadYaml(IParser parser, Type type)
    {
        // Implementing the ReadYaml method is complex because it requires converting YAML back into JsonElement.
        // This implementation will be left out for simplicity.
        throw new NotImplementedException("Deserialization from YAML to JsonElement is not implemented.");
    }

    public object? ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
    {
        throw new NotImplementedException();
    }

    public void WriteYaml(IEmitter emitter, object value, Type type, ObjectSerializer serializer)
    {
        if (value is JsonElement jsonElement)
        {
            WriteJsonElement(emitter, jsonElement);
        }
    }

    private void WriteJsonElement(IEmitter emitter, JsonElement element)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
                emitter.Emit(new MappingStart());
                foreach (var property in element.EnumerateObject())
                {
                    emitter.Emit(new Scalar(property.Name));
                    WriteJsonElement(emitter, property.Value);
                }
                emitter.Emit(new MappingEnd());
                break;
            case JsonValueKind.Array:
                emitter.Emit(new SequenceStart(null, null, false, SequenceStyle.Block));
                foreach (var item in element.EnumerateArray())
                {
                    WriteJsonElement(emitter, item);
                }
                emitter.Emit(new SequenceEnd());
                break;
            case JsonValueKind.String:
                emitter.Emit(new Scalar(element.GetString()));
                break;
            case JsonValueKind.Number:
                if (element.TryGetInt32(out int intValue))
                {
                    emitter.Emit(new Scalar(intValue.ToString()));
                }
                else if (element.TryGetDouble(out double doubleValue))
                {
                    emitter.Emit(new Scalar(doubleValue.ToString(System.Globalization.CultureInfo.InvariantCulture)));
                }
                break;
            case JsonValueKind.True:
                emitter.Emit(new Scalar("true"));
                break;
            case JsonValueKind.False:
                emitter.Emit(new Scalar("false"));
                break;
            case JsonValueKind.Null:
                emitter.Emit(new Scalar("~"));
                break;
            default:
                throw new InvalidOperationException($"Unsupported JsonValueKind: {element.ValueKind}");
        }
    }
}
