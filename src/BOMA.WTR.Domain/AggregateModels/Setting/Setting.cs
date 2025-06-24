using System.Text.Json;
using BOMA.WTR.Domain.SeedWork;
using BOMA.WTR.Domain.SharedKernel;

namespace BOMA.WTR.Domain.AggregateModels.Setting;

public class Setting : Entity<int>, IAggregateRoot
{
    public int Id { get; set; } // EF Core ustawi to przy seede
    public string Key { get; private set; }
    public string Value { get; private set; }
    public string Type { get; private set; }
    public string Description { get; private set; }
    public DateTimeOffset LastModified { get; private set; }
    
    private Setting() { }

    public Setting(string key, string value, string type, string description = null)
    {
        Set(key, value, type, description);
    }

    public void Set(string key, string value, string type, string description = null)
    {
        Key = key;
        Value = value;
        Type = type;
        Description = description;
        LastModified = DateTime.UtcNow;
    }

    public object GetTypedValue()
    {
        return Type switch
        {
            "int" => int.TryParse(Value, out var i) ? i : null,
            "bool" => bool.TryParse(Value, out var b) ? b : null,
            "decimal" => decimal.TryParse(Value, out var d) ? d : null,
            "json" => JsonSerializer.Deserialize<object>(Value),
            _ => Value
        } ?? throw new InvalidOperationException();
    }
}