using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public class NullableEnumConverter<TEnum> : JsonConverter<TEnum> where TEnum : struct, Enum
{
    public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return default;
        }

        if (reader.TokenType == JsonTokenType.Number)
        {
            if (Enum.TryParse<TEnum>(reader.GetInt32().ToString(), out TEnum value))
            {
                return value;
            }
        }
        else if (reader.TokenType == JsonTokenType.String)
        {
            if (Enum.TryParse<TEnum>(reader.GetString(), out TEnum value))
            {
                return value;
            }
        }

        throw new JsonException($"Unable to parse enum value: {reader.GetString()}");
    }

    public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}

public class NullableInt32Converter : JsonConverter<int>
{
    public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return 0;
        }

        if (reader.TokenType == JsonTokenType.String)
        {
            if (!string.IsNullOrEmpty(reader.GetString()))
            {
                return int.Parse(reader.GetString());
            }
            else
            {
                return 0;
            }
        }

        if (reader.TokenType == JsonTokenType.Number)
        {
            return reader.GetInt32();
        }

        return 0;
    }

    public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value);
    }
}

public class NullableFloatConverter : JsonConverter<float>
{
    public override float Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return 0.0f;
        }

        else if (reader.TokenType == JsonTokenType.String)
        {
            if (!string.IsNullOrEmpty(reader.GetString()))
            {
                return float.Parse(reader.GetString());
            }
            else
            {
                return 0.0f;
            }
        }

        else if (reader.TokenType == JsonTokenType.Number)
        {
            return reader.GetSingle();
        }

        return 0.0f;
    }

    public override void Write(Utf8JsonWriter writer, float value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value);
    }
}

public class NullableStringConverter : JsonConverter<string>
{
    public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return "";
        }

        else if (reader.TokenType == JsonTokenType.Number)
        {
            return reader.GetInt32().ToString();
        }

        else if (reader.TokenType == JsonTokenType.String)
        {
            return reader.GetString();
        }

        return "";
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}

public class NullableInt64Converter : JsonConverter<long>
{
    public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return 0;
        }

        else if (reader.TokenType == JsonTokenType.Number)
        {
            return reader.GetInt64();
        }

        else if (reader.TokenType == JsonTokenType.String)
        {
            if (!string.IsNullOrEmpty(reader.GetString()))
            {
                return long.Parse(reader.GetString());
            }
            return 0;
        }

        return 0;
    }

    public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value);
    }
}
