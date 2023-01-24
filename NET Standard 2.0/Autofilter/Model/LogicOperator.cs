using System.Text.Json.Serialization;

namespace Autofilter.Model;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum LogicOperator { And, Or }