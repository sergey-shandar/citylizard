namespace CityLizard.Policy
{
	public class All:
		INumeric<System.SByte>,
		INumeric<System.Byte>,
		INumeric<System.Int16>,
		INumeric<System.UInt16>,
		INumeric<System.Int32>,
		INumeric<System.UInt32>,
		INumeric<System.Int64>,
		INumeric<System.UInt64>,
		INumeric<System.Decimal>,
		INumeric<System.Single>,
		INumeric<System.Double>
	{
		public static readonly All P = new All();
        System.SByte INumeric<System.SByte>._0
        { 
            get { return 0; }
        }
        System.SByte INumeric<System.SByte>._1 
        {
            get { return 1; }
        }
        System.SByte INumeric<System.SByte>.MinValue
        { 
            get { return System.SByte.MinValue; }
        }
        System.SByte INumeric<System.SByte>.MaxValue 
        {
            get { return System.SByte.MaxValue; }
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
        System.Byte INumeric<System.Byte>._0
        { 
            get { return 0; }
        }
        System.Byte INumeric<System.Byte>._1 
        {
            get { return 1; }
        }
        System.Byte INumeric<System.Byte>.MinValue
        { 
            get { return System.Byte.MinValue; }
        }
        System.Byte INumeric<System.Byte>.MaxValue 
        {
            get { return System.Byte.MaxValue; }
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
        System.Int16 INumeric<System.Int16>._0
        { 
            get { return 0; }
        }
        System.Int16 INumeric<System.Int16>._1 
        {
            get { return 1; }
        }
        System.Int16 INumeric<System.Int16>.MinValue
        { 
            get { return System.Int16.MinValue; }
        }
        System.Int16 INumeric<System.Int16>.MaxValue 
        {
            get { return System.Int16.MaxValue; }
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
        System.UInt16 INumeric<System.UInt16>._0
        { 
            get { return 0; }
        }
        System.UInt16 INumeric<System.UInt16>._1 
        {
            get { return 1; }
        }
        System.UInt16 INumeric<System.UInt16>.MinValue
        { 
            get { return System.UInt16.MinValue; }
        }
        System.UInt16 INumeric<System.UInt16>.MaxValue 
        {
            get { return System.UInt16.MaxValue; }
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
        System.Int32 INumeric<System.Int32>._0
        { 
            get { return 0; }
        }
        System.Int32 INumeric<System.Int32>._1 
        {
            get { return 1; }
        }
        System.Int32 INumeric<System.Int32>.MinValue
        { 
            get { return System.Int32.MinValue; }
        }
        System.Int32 INumeric<System.Int32>.MaxValue 
        {
            get { return System.Int32.MaxValue; }
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
        System.UInt32 INumeric<System.UInt32>._0
        { 
            get { return 0; }
        }
        System.UInt32 INumeric<System.UInt32>._1 
        {
            get { return 1; }
        }
        System.UInt32 INumeric<System.UInt32>.MinValue
        { 
            get { return System.UInt32.MinValue; }
        }
        System.UInt32 INumeric<System.UInt32>.MaxValue 
        {
            get { return System.UInt32.MaxValue; }
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
        System.Int64 INumeric<System.Int64>._0
        { 
            get { return 0; }
        }
        System.Int64 INumeric<System.Int64>._1 
        {
            get { return 1; }
        }
        System.Int64 INumeric<System.Int64>.MinValue
        { 
            get { return System.Int64.MinValue; }
        }
        System.Int64 INumeric<System.Int64>.MaxValue 
        {
            get { return System.Int64.MaxValue; }
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
        System.UInt64 INumeric<System.UInt64>._0
        { 
            get { return 0; }
        }
        System.UInt64 INumeric<System.UInt64>._1 
        {
            get { return 1; }
        }
        System.UInt64 INumeric<System.UInt64>.MinValue
        { 
            get { return System.UInt64.MinValue; }
        }
        System.UInt64 INumeric<System.UInt64>.MaxValue 
        {
            get { return System.UInt64.MaxValue; }
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
        System.Decimal INumeric<System.Decimal>._0
        { 
            get { return 0; }
        }
        System.Decimal INumeric<System.Decimal>._1 
        {
            get { return 1; }
        }
        System.Decimal INumeric<System.Decimal>.MinValue
        { 
            get { return System.Decimal.MinValue; }
        }
        System.Decimal INumeric<System.Decimal>.MaxValue 
        {
            get { return System.Decimal.MaxValue; }
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
        System.Single INumeric<System.Single>._0
        { 
            get { return 0; }
        }
        System.Single INumeric<System.Single>._1 
        {
            get { return 1; }
        }
        System.Single INumeric<System.Single>.MinValue
        { 
            get { return System.Single.MinValue; }
        }
        System.Single INumeric<System.Single>.MaxValue 
        {
            get { return System.Single.MaxValue; }
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
        System.Double INumeric<System.Double>.MinValue
        { 
            get { return System.Double.MinValue; }
        }
        System.Double INumeric<System.Double>.MaxValue 
        {
            get { return System.Double.MaxValue; }
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
	}
}
