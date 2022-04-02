using System;

namespace Autofilter.Tests.FakeData;

class PropTypesTestClass
{
    public Guid Guid { get; init; }
    public Guid? NullableGuid { get; init; }
    public bool Bool { get; init; }
    public bool? NullableBool { get; init; }
    public long Long { get; init; }
    public long? NullableLong { get; init; }
    public int Int { get; init; }
    public int? NullableInt { get; init; }
    public short Short { get; init; }
    public short? NullableShort { get; init; }
    public decimal Decimal { get; init; }
    public decimal? NullableDecimal { get; init; }
    public double Double { get; init; }
    public double? NullableDouble { get; init; }
    public float Float { get; init; }
    public float? NullableFloat { get; init; }
    public byte Byte { get; init; }
    public byte? NullableByte { get; init; }
    public char Char { get; init; }
    public char? NullableChar { get; init; }
    public TestEnum Enum { get; init; }
    public TestEnum? NullableEnum { get; init; }
    public DateTime DateTime { get; init; }
    public DateTime? NullableDateTime { get; init; }
    public string? String { get; init; }
}

public enum TestEnum { One, Two }