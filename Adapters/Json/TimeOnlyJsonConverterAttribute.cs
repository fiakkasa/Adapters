using System.Text.Json.Serialization;

namespace Adapters.Json;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class TimeOnlyJsonConverterAttribute : JsonConverterAttribute
{
    public TimeOnlyJsonConverterAttribute() : base(typeof(TimeOnlyJsonConverter))
    {
    }
}