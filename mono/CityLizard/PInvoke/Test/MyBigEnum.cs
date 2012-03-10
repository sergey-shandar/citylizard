namespace CityLizard.PInvoke.Test
{
    public enum MyBigEnum: long
    {
        MaxValue = long.MaxValue,
        MinValue = long.MinValue,
    }

    public enum MyPBigEnum : ulong
    {
        MaxValue = ulong.MaxValue,
    }

    public enum IntEnum : int
    {
        MaxValue = int.MaxValue,
        MinValue = int.MinValue,
    }

    public enum UIntEnum : uint
    {
        MaxValue = uint.MaxValue,
    }
}
