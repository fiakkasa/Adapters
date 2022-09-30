using System.Text.Json;
using Xunit;

namespace Adapters.Json.Tests;

public class DateOnlyJsonConverterTests
{
    public record TestNullableDateOnly
    {
        [NullableDateOnlyJsonConverter]
        public DateOnly? Date { get; set; }
    }

    public record TestDateOnly
    {
        [DateOnlyJsonConverter]
        public DateOnly Date { get; set; }
    }

    [Fact(DisplayName = "Serialize - Valid")]
    public void SerializeValid()
    {
        const string json = "{\"Date\":\"2022-01-01\"}";
        var objNullableWithValue = JsonSerializer.Serialize(new TestNullableDateOnly { Date = new DateOnly(2022, 1, 1) });
        var objNullableWithoutValue = JsonSerializer.Serialize(new TestNullableDateOnly());
        var objWithValue = JsonSerializer.Serialize(new TestDateOnly { Date = new DateOnly(2022, 1, 1) });
        var objWithoutValue = JsonSerializer.Serialize(new TestDateOnly());

        Assert.Equal(json, objNullableWithValue);
        Assert.Equal("{\"Date\":null}", objNullableWithoutValue);
        Assert.Equal(json, objWithValue);
        Assert.Equal("{\"Date\":\"0001-01-01\"}", objWithoutValue);
    }

    [Fact(DisplayName = "Deserialize - Valid")]
    public void DeserializeValid()
    {
        const string json = "{\"Date\":\"2022-01-01\"}";
        const string jsonNull = "{\"Date\":null}";

        var objNullableWithValue = JsonSerializer.Deserialize<TestNullableDateOnly>(json);
        var objNullableWithoutValue = JsonSerializer.Deserialize<TestNullableDateOnly>(jsonNull);
        var objWithValue = JsonSerializer.Deserialize<TestDateOnly>(json);
        var objWithoutValue = JsonSerializer.Deserialize<TestDateOnly>(jsonNull);

        Assert.True(objNullableWithValue is { Date: { } });
        Assert.True(objNullableWithoutValue is { Date: null });
        Assert.True(objWithValue is { Date: { } });
        Assert.True(objWithoutValue is { } && objWithoutValue.Date == DateOnly.MinValue);
    }

    [Fact(DisplayName = "Deserialize - Invalid")]
    public void DeserializeInvalid()
    {
        const string json = "{\"Date\":\"hello\"}";

        var objNullable = JsonSerializer.Deserialize<TestNullableDateOnly>(json);
        var obj = JsonSerializer.Deserialize<TestDateOnly>(json);

        Assert.True(objNullable is { Date: null });
        Assert.True(obj is { } && obj.Date == DateOnly.MinValue);
    }
}