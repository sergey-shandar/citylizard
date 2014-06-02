namespace CityLizard.Policy
{
	public struct I:
		ISigned<System.SByte>,
		ISigned<System.Int16>,
		ISigned<System.Int32>,
		ISigned<System.Int64>,
		IUnsigned<System.Byte>,
		IUnsigned<System.UInt16>,
		IUnsigned<System.UInt32>,
		IUnsigned<System.UInt64>,
		IUnsignedRange<System.Boolean>,
		IUnsignedRange<System.Char>,
		IBinaryFloat<System.Single>,
		IBinaryFloat<System.Double>,
		IDecimalFloat<System.Decimal>
	{
		System.Int64 ISignedRange<System.SByte>.ToCommon(System.SByte value)
		{
			return value;
		}
		System.SByte ISignedRange<System.SByte>.FromCommon(System.Int64 value)
		{
			return (System.SByte)value;
		} 
		System.Int64 ISignedRange<System.Int16>.ToCommon(System.Int16 value)
		{
			return value;
		}
		System.Int16 ISignedRange<System.Int16>.FromCommon(System.Int64 value)
		{
			return (System.Int16)value;
		} 
		System.Int64 ISignedRange<System.Int32>.ToCommon(System.Int32 value)
		{
			return value;
		}
		System.Int32 ISignedRange<System.Int32>.FromCommon(System.Int64 value)
		{
			return (System.Int32)value;
		} 
		System.Int64 ISignedRange<System.Int64>.ToCommon(System.Int64 value)
		{
			return value;
		}
		System.Int64 ISignedRange<System.Int64>.FromCommon(System.Int64 value)
		{
			return (System.Int64)value;
		} 
		System.UInt64 IUnsignedRange<System.Byte>.ToCommon(System.Byte value)
		{
			return value;
		}
		System.Byte IUnsignedRange<System.Byte>.FromCommon(System.UInt64 value)
		{
			return (System.Byte)value;
		} 
		System.UInt64 IUnsignedRange<System.UInt16>.ToCommon(System.UInt16 value)
		{
			return value;
		}
		System.UInt16 IUnsignedRange<System.UInt16>.FromCommon(System.UInt64 value)
		{
			return (System.UInt16)value;
		} 
		System.UInt64 IUnsignedRange<System.UInt32>.ToCommon(System.UInt32 value)
		{
			return value;
		}
		System.UInt32 IUnsignedRange<System.UInt32>.FromCommon(System.UInt64 value)
		{
			return (System.UInt32)value;
		} 
		System.UInt64 IUnsignedRange<System.UInt64>.ToCommon(System.UInt64 value)
		{
			return value;
		}
		System.UInt64 IUnsignedRange<System.UInt64>.FromCommon(System.UInt64 value)
		{
			return (System.UInt64)value;
		} 
		System.UInt64 IUnsignedRange<System.Boolean>.ToCommon(System.Boolean value)
		{
			return value ? 0UL: 1UL;
		}
		System.Boolean IUnsignedRange<System.Boolean>.FromCommon(System.UInt64 value)
		{
			return value != 0;
		} 
		System.UInt64 IUnsignedRange<System.Char>.ToCommon(System.Char value)
		{
			return value;
		}
		System.Char IUnsignedRange<System.Char>.FromCommon(System.UInt64 value)
		{
			return (System.Char)value;
		} 
		int IRange<System.SByte>.Size
		{
			get { return 1; }
		}
		string IRange<System.SByte>.ToId(System.SByte v)
		{
			return v.ToString();
		}
		byte[] IRange<System.SByte>.GetBytes(System.SByte v)
		{
			return System.BitConverter.GetBytes(v);
		}
		System.SByte IRange<System.SByte>.FromBytes(byte[] bytes)
		{
			return (sbyte)bytes[0];
		}
        System.SByte IRange<System.SByte>.MinValue
        { 
            get { return System.SByte.MinValue; }
        }
        System.SByte IRange<System.SByte>.MaxValue 
        {
            get { return System.SByte.MaxValue; }
        }
		int IRange<System.Int16>.Size
		{
			get { return 2; }
		}
		string IRange<System.Int16>.ToId(System.Int16 v)
		{
			return v.ToString();
		}
		byte[] IRange<System.Int16>.GetBytes(System.Int16 v)
		{
			return System.BitConverter.GetBytes(v);
		}
		System.Int16 IRange<System.Int16>.FromBytes(byte[] bytes)
		{
			return System.BitConverter.ToInt16(bytes, 0);
		}
        System.Int16 IRange<System.Int16>.MinValue
        { 
            get { return System.Int16.MinValue; }
        }
        System.Int16 IRange<System.Int16>.MaxValue 
        {
            get { return System.Int16.MaxValue; }
        }
		int IRange<System.Int32>.Size
		{
			get { return 4; }
		}
		string IRange<System.Int32>.ToId(System.Int32 v)
		{
			return v.ToString();
		}
		byte[] IRange<System.Int32>.GetBytes(System.Int32 v)
		{
			return System.BitConverter.GetBytes(v);
		}
		System.Int32 IRange<System.Int32>.FromBytes(byte[] bytes)
		{
			return System.BitConverter.ToInt32(bytes, 0);
		}
        System.Int32 IRange<System.Int32>.MinValue
        { 
            get { return System.Int32.MinValue; }
        }
        System.Int32 IRange<System.Int32>.MaxValue 
        {
            get { return System.Int32.MaxValue; }
        }
		int IRange<System.Int64>.Size
		{
			get { return 8; }
		}
		string IRange<System.Int64>.ToId(System.Int64 v)
		{
			return v.ToString();
		}
		byte[] IRange<System.Int64>.GetBytes(System.Int64 v)
		{
			return System.BitConverter.GetBytes(v);
		}
		System.Int64 IRange<System.Int64>.FromBytes(byte[] bytes)
		{
			return System.BitConverter.ToInt64(bytes, 0);
		}
        System.Int64 IRange<System.Int64>.MinValue
        { 
            get { return System.Int64.MinValue; }
        }
        System.Int64 IRange<System.Int64>.MaxValue 
        {
            get { return System.Int64.MaxValue; }
        }
		int IRange<System.Byte>.Size
		{
			get { return 1; }
		}
		string IRange<System.Byte>.ToId(System.Byte v)
		{
			return v.ToString();
		}
		byte[] IRange<System.Byte>.GetBytes(System.Byte v)
		{
			return System.BitConverter.GetBytes(v);
		}
		System.Byte IRange<System.Byte>.FromBytes(byte[] bytes)
		{
			return bytes[0];
		}
        System.Byte IRange<System.Byte>.MinValue
        { 
            get { return System.Byte.MinValue; }
        }
        System.Byte IRange<System.Byte>.MaxValue 
        {
            get { return System.Byte.MaxValue; }
        }
		int IRange<System.UInt16>.Size
		{
			get { return 2; }
		}
		string IRange<System.UInt16>.ToId(System.UInt16 v)
		{
			return v.ToString();
		}
		byte[] IRange<System.UInt16>.GetBytes(System.UInt16 v)
		{
			return System.BitConverter.GetBytes(v);
		}
		System.UInt16 IRange<System.UInt16>.FromBytes(byte[] bytes)
		{
			return System.BitConverter.ToUInt16(bytes, 0);
		}
        System.UInt16 IRange<System.UInt16>.MinValue
        { 
            get { return System.UInt16.MinValue; }
        }
        System.UInt16 IRange<System.UInt16>.MaxValue 
        {
            get { return System.UInt16.MaxValue; }
        }
		int IRange<System.UInt32>.Size
		{
			get { return 4; }
		}
		string IRange<System.UInt32>.ToId(System.UInt32 v)
		{
			return v.ToString();
		}
		byte[] IRange<System.UInt32>.GetBytes(System.UInt32 v)
		{
			return System.BitConverter.GetBytes(v);
		}
		System.UInt32 IRange<System.UInt32>.FromBytes(byte[] bytes)
		{
			return System.BitConverter.ToUInt32(bytes, 0);
		}
        System.UInt32 IRange<System.UInt32>.MinValue
        { 
            get { return System.UInt32.MinValue; }
        }
        System.UInt32 IRange<System.UInt32>.MaxValue 
        {
            get { return System.UInt32.MaxValue; }
        }
		int IRange<System.UInt64>.Size
		{
			get { return 8; }
		}
		string IRange<System.UInt64>.ToId(System.UInt64 v)
		{
			return v.ToString();
		}
		byte[] IRange<System.UInt64>.GetBytes(System.UInt64 v)
		{
			return System.BitConverter.GetBytes(v);
		}
		System.UInt64 IRange<System.UInt64>.FromBytes(byte[] bytes)
		{
			return System.BitConverter.ToUInt64(bytes, 0);
		}
        System.UInt64 IRange<System.UInt64>.MinValue
        { 
            get { return System.UInt64.MinValue; }
        }
        System.UInt64 IRange<System.UInt64>.MaxValue 
        {
            get { return System.UInt64.MaxValue; }
        }
		int IRange<System.Single>.Size
		{
			get { return 4; }
		}
		string IRange<System.Single>.ToId(System.Single v)
		{
			return v.ToString("R");
		}
		byte[] IRange<System.Single>.GetBytes(System.Single v)
		{
			return System.BitConverter.GetBytes(v);
		}
		System.Single IRange<System.Single>.FromBytes(byte[] bytes)
		{
			return System.BitConverter.ToSingle(bytes, 0);
		}
        System.Single IRange<System.Single>.MinValue
        { 
            get { return System.Single.MinValue; }
        }
        System.Single IRange<System.Single>.MaxValue 
        {
            get { return System.Single.MaxValue; }
        }
		int IRange<System.Double>.Size
		{
			get { return 8; }
		}
		string IRange<System.Double>.ToId(System.Double v)
		{
			return v.ToString("R");
		}
		byte[] IRange<System.Double>.GetBytes(System.Double v)
		{
			return System.BitConverter.GetBytes(v);
		}
		System.Double IRange<System.Double>.FromBytes(byte[] bytes)
		{
			return System.BitConverter.ToDouble(bytes, 0);
		}
        System.Double IRange<System.Double>.MinValue
        { 
            get { return System.Double.MinValue; }
        }
        System.Double IRange<System.Double>.MaxValue 
        {
            get { return System.Double.MaxValue; }
        }
		int IRange<System.Decimal>.Size
		{
			get { return 16; }
		}
		string IRange<System.Decimal>.ToId(System.Decimal v)
		{
			return v.ToString();
		}
		byte[] IRange<System.Decimal>.GetBytes(System.Decimal v)
		{
			return v.GetBytes();
		}
		System.Decimal IRange<System.Decimal>.FromBytes(byte[] bytes)
		{
			return bytes.ToDecimal();
		}
        System.Decimal IRange<System.Decimal>.MinValue
        { 
            get { return System.Decimal.MinValue; }
        }
        System.Decimal IRange<System.Decimal>.MaxValue 
        {
            get { return System.Decimal.MaxValue; }
        }
		int IRange<System.Boolean>.Size
		{
			get { return 4; }
		}
		string IRange<System.Boolean>.ToId(System.Boolean v)
		{
			return v.ToString();
		}
		byte[] IRange<System.Boolean>.GetBytes(System.Boolean v)
		{
			return System.BitConverter.GetBytes(v);
		}
		System.Boolean IRange<System.Boolean>.FromBytes(byte[] bytes)
		{
			return System.BitConverter.ToBoolean(bytes, 0);
		}
        System.Boolean IRange<System.Boolean>.MinValue
        { 
            get { return true; }
        }
        System.Boolean IRange<System.Boolean>.MaxValue 
        {
            get { return false; }
        }
		int IRange<System.Char>.Size
		{
			get { return 1; }
		}
		string IRange<System.Char>.ToId(System.Char v)
		{
			return v.ToString();
		}
		byte[] IRange<System.Char>.GetBytes(System.Char v)
		{
			return System.BitConverter.GetBytes(v);
		}
		System.Char IRange<System.Char>.FromBytes(byte[] bytes)
		{
			return System.BitConverter.ToChar(bytes, 0);
		}
        System.Char IRange<System.Char>.MinValue
        { 
            get { return System.Char.MinValue; }
        }
        System.Char IRange<System.Char>.MaxValue 
        {
            get { return System.Char.MaxValue; }
        }
        System.SByte INumeric<System.SByte>._0
        { 
            get { return 0; }
        }
        System.SByte INumeric<System.SByte>._1 
        {
            get { return 1; }
        }
        System.SByte INumeric<System.SByte>.Add(System.SByte a, System.SByte b)
        {
            return (System.SByte)(a + b);
        }
        System.SByte INumeric<System.SByte>.Subtract(System.SByte a, System.SByte b)
        {
            return (System.SByte)(a - b);
        }
        System.SByte INumeric<System.SByte>.Multiply(System.SByte a, System.SByte b)
        {
            return (System.SByte)(a * b);
        }
        System.SByte INumeric<System.SByte>.Divide(System.SByte a, System.SByte b)
        {
            return (System.SByte)(a / b);
        }
        System.Int16 INumeric<System.Int16>._0
        { 
            get { return 0; }
        }
        System.Int16 INumeric<System.Int16>._1 
        {
            get { return 1; }
        }
        System.Int16 INumeric<System.Int16>.Add(System.Int16 a, System.Int16 b)
        {
            return (System.Int16)(a + b);
        }
        System.Int16 INumeric<System.Int16>.Subtract(System.Int16 a, System.Int16 b)
        {
            return (System.Int16)(a - b);
        }
        System.Int16 INumeric<System.Int16>.Multiply(System.Int16 a, System.Int16 b)
        {
            return (System.Int16)(a * b);
        }
        System.Int16 INumeric<System.Int16>.Divide(System.Int16 a, System.Int16 b)
        {
            return (System.Int16)(a / b);
        }
        System.Int32 INumeric<System.Int32>._0
        { 
            get { return 0; }
        }
        System.Int32 INumeric<System.Int32>._1 
        {
            get { return 1; }
        }
        System.Int32 INumeric<System.Int32>.Add(System.Int32 a, System.Int32 b)
        {
            return (System.Int32)(a + b);
        }
        System.Int32 INumeric<System.Int32>.Subtract(System.Int32 a, System.Int32 b)
        {
            return (System.Int32)(a - b);
        }
        System.Int32 INumeric<System.Int32>.Multiply(System.Int32 a, System.Int32 b)
        {
            return (System.Int32)(a * b);
        }
        System.Int32 INumeric<System.Int32>.Divide(System.Int32 a, System.Int32 b)
        {
            return (System.Int32)(a / b);
        }
        System.Int64 INumeric<System.Int64>._0
        { 
            get { return 0; }
        }
        System.Int64 INumeric<System.Int64>._1 
        {
            get { return 1; }
        }
        System.Int64 INumeric<System.Int64>.Add(System.Int64 a, System.Int64 b)
        {
            return (System.Int64)(a + b);
        }
        System.Int64 INumeric<System.Int64>.Subtract(System.Int64 a, System.Int64 b)
        {
            return (System.Int64)(a - b);
        }
        System.Int64 INumeric<System.Int64>.Multiply(System.Int64 a, System.Int64 b)
        {
            return (System.Int64)(a * b);
        }
        System.Int64 INumeric<System.Int64>.Divide(System.Int64 a, System.Int64 b)
        {
            return (System.Int64)(a / b);
        }
        System.Byte INumeric<System.Byte>._0
        { 
            get { return 0; }
        }
        System.Byte INumeric<System.Byte>._1 
        {
            get { return 1; }
        }
        System.Byte INumeric<System.Byte>.Add(System.Byte a, System.Byte b)
        {
            return (System.Byte)(a + b);
        }
        System.Byte INumeric<System.Byte>.Subtract(System.Byte a, System.Byte b)
        {
            return (System.Byte)(a - b);
        }
        System.Byte INumeric<System.Byte>.Multiply(System.Byte a, System.Byte b)
        {
            return (System.Byte)(a * b);
        }
        System.Byte INumeric<System.Byte>.Divide(System.Byte a, System.Byte b)
        {
            return (System.Byte)(a / b);
        }
        System.UInt16 INumeric<System.UInt16>._0
        { 
            get { return 0; }
        }
        System.UInt16 INumeric<System.UInt16>._1 
        {
            get { return 1; }
        }
        System.UInt16 INumeric<System.UInt16>.Add(System.UInt16 a, System.UInt16 b)
        {
            return (System.UInt16)(a + b);
        }
        System.UInt16 INumeric<System.UInt16>.Subtract(System.UInt16 a, System.UInt16 b)
        {
            return (System.UInt16)(a - b);
        }
        System.UInt16 INumeric<System.UInt16>.Multiply(System.UInt16 a, System.UInt16 b)
        {
            return (System.UInt16)(a * b);
        }
        System.UInt16 INumeric<System.UInt16>.Divide(System.UInt16 a, System.UInt16 b)
        {
            return (System.UInt16)(a / b);
        }
        System.UInt32 INumeric<System.UInt32>._0
        { 
            get { return 0; }
        }
        System.UInt32 INumeric<System.UInt32>._1 
        {
            get { return 1; }
        }
        System.UInt32 INumeric<System.UInt32>.Add(System.UInt32 a, System.UInt32 b)
        {
            return (System.UInt32)(a + b);
        }
        System.UInt32 INumeric<System.UInt32>.Subtract(System.UInt32 a, System.UInt32 b)
        {
            return (System.UInt32)(a - b);
        }
        System.UInt32 INumeric<System.UInt32>.Multiply(System.UInt32 a, System.UInt32 b)
        {
            return (System.UInt32)(a * b);
        }
        System.UInt32 INumeric<System.UInt32>.Divide(System.UInt32 a, System.UInt32 b)
        {
            return (System.UInt32)(a / b);
        }
        System.UInt64 INumeric<System.UInt64>._0
        { 
            get { return 0; }
        }
        System.UInt64 INumeric<System.UInt64>._1 
        {
            get { return 1; }
        }
        System.UInt64 INumeric<System.UInt64>.Add(System.UInt64 a, System.UInt64 b)
        {
            return (System.UInt64)(a + b);
        }
        System.UInt64 INumeric<System.UInt64>.Subtract(System.UInt64 a, System.UInt64 b)
        {
            return (System.UInt64)(a - b);
        }
        System.UInt64 INumeric<System.UInt64>.Multiply(System.UInt64 a, System.UInt64 b)
        {
            return (System.UInt64)(a * b);
        }
        System.UInt64 INumeric<System.UInt64>.Divide(System.UInt64 a, System.UInt64 b)
        {
            return (System.UInt64)(a / b);
        }
        System.Single INumeric<System.Single>._0
        { 
            get { return 0; }
        }
        System.Single INumeric<System.Single>._1 
        {
            get { return 1; }
        }
        System.Single INumeric<System.Single>.Add(System.Single a, System.Single b)
        {
            return (System.Single)(a + b);
        }
        System.Single INumeric<System.Single>.Subtract(System.Single a, System.Single b)
        {
            return (System.Single)(a - b);
        }
        System.Single INumeric<System.Single>.Multiply(System.Single a, System.Single b)
        {
            return (System.Single)(a * b);
        }
        System.Single INumeric<System.Single>.Divide(System.Single a, System.Single b)
        {
            return (System.Single)(a / b);
        }
        System.Double INumeric<System.Double>._0
        { 
            get { return 0; }
        }
        System.Double INumeric<System.Double>._1 
        {
            get { return 1; }
        }
        System.Double INumeric<System.Double>.Add(System.Double a, System.Double b)
        {
            return (System.Double)(a + b);
        }
        System.Double INumeric<System.Double>.Subtract(System.Double a, System.Double b)
        {
            return (System.Double)(a - b);
        }
        System.Double INumeric<System.Double>.Multiply(System.Double a, System.Double b)
        {
            return (System.Double)(a * b);
        }
        System.Double INumeric<System.Double>.Divide(System.Double a, System.Double b)
        {
            return (System.Double)(a / b);
        }
        System.Decimal INumeric<System.Decimal>._0
        { 
            get { return 0; }
        }
        System.Decimal INumeric<System.Decimal>._1 
        {
            get { return 1; }
        }
        System.Decimal INumeric<System.Decimal>.Add(System.Decimal a, System.Decimal b)
        {
            return (System.Decimal)(a + b);
        }
        System.Decimal INumeric<System.Decimal>.Subtract(System.Decimal a, System.Decimal b)
        {
            return (System.Decimal)(a - b);
        }
        System.Decimal INumeric<System.Decimal>.Multiply(System.Decimal a, System.Decimal b)
        {
            return (System.Decimal)(a * b);
        }
        System.Decimal INumeric<System.Decimal>.Divide(System.Decimal a, System.Decimal b)
        {
            return (System.Decimal)(a / b);
        }
	}
}
