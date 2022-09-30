using System.Text.Json;
using Xunit;

namespace Adapters.Json.Tests;

public class TimeOnlyJsonConverterTests
{
    public record TestNullableTimeOnly
    {
        [NullableTimeOnlyJsonConverter]
        public TimeOnly? Time { get; set; }
    }

    public record TestTimeOnly
    {
        [TimeOnlyJsonConverter]
        public TimeOnly Time { get; set; }
    }

    [Fact(DisplayName = "Serialize - Valid")]
    public void SerializeValid()
    {
        const string json = "{\"Time\":\"01:01\"}";
        var objNullableWithValue = JsonSerializer.Serialize(new TestNullableTimeOnly { Time = new TimeOnly(1, 1, 1) });
        var objNullableWithoutValue = JsonSerializer.Serialize(new TestNullableTimeOnly());
        var objWithValue = JsonSerializer.Serialize(new TestTimeOnly { Time = new TimeOnly(1, 1, 1) });
        var objWithoutValue = JsonSerializer.Serialize(new TestTimeOnly());

        Assert.Equal(json, objNullableWithValue);
        Assert.Equal("{\"Time\":null}", objNullableWithoutValue);
        Assert.Equal(json, objWithValue);
        Assert.Equal("{\"Time\":\"00:00\"}", objWithoutValue);
    }

    [Fact(DisplayName = "Deserialize - Valid")]
    public void DeserializeValid()
    {
        const string json = "{\"Time\":\"01:01\"}";
        const string jsonNull = "{\"Time\":null}";

        var objNullableWithValue = JsonSerializer.Deserialize<TestNullableTimeOnly>(json);
        var objNullableWithoutValue = JsonSerializer.Deserialize<TestNullableTimeOnly>(jsonNull);
        var objWithValue = JsonSerializer.Deserialize<TestTimeOnly>(json);
        var objWithoutValue = JsonSerializer.Deserialize<TestTimeOnly>(jsonNull);

        Assert.True(objNullableWithValue is { Time: { } });
        Assert.True(objNullableWithoutValue is { Time: null });
        Assert.True(objWithValue is { Time: { } });
        Assert.True(objWithoutValue is { } && objWithoutValue.Time == TimeOnly.MinValue);
    }

    [Fact(DisplayName = "Deserialize - Invalid")]
    public void DeserializeInvalid()
    {
        const string jsonInvalid = "{\"Time\":\"hello\"}";

        var objNullable = JsonSerializer.Deserialize<TestNullableTimeOnly>(jsonInvalid);
        var obj = JsonSerializer.Deserialize<TestTimeOnly>(jsonInvalid);

        Assert.True(objNullable is { Time: null });
        Assert.True(obj is { } && obj.Time == TimeOnly.MinValue);
    }
}
