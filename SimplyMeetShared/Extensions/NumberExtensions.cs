namespace SimplyMeetShared.Extensions;

public static class NumberExtensions
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Static Fields
	private static readonly String[] _DecimalLevels = { "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
	private static readonly String[] _BinaryLevels = { "B", "KiB", "MiB", "GiB", "TiB", "PiB", "EiB", "ZiB", "YiB" };
	#endregion

	//===========================================================================================
	// Public Static Methods
	//===========================================================================================
	public static String ToSizeText(this UInt64 InNum, Int32 InDecimalPlaces = 2, Boolean InBinary = true)
	{
		var Levels = _DecimalLevels;
		var Divisor = 1000.0;
		var Size = (Double)InNum;

		if (InBinary)
		{
			Levels = _BinaryLevels;
			Divisor = 1024.0;
		}

		var SizeLevel = (Byte)0;
		while (Size / Divisor >= 1.0 && SizeLevel < Levels.Length - 1)
		{
			Size /= Divisor;
			SizeLevel++;
		}

		if (SizeLevel <= 0) return $"{(Int32)Size} {Levels[SizeLevel]}";
		return $"{Size.ToString($"n{InDecimalPlaces}")} {Levels[SizeLevel]}";
	}
	public static String ToSizeText(this Int64 InNum, Int32 InDecimalPlaces = 2, Boolean InBinary = true) => ToSizeText((UInt64)InNum, InDecimalPlaces, InBinary);
	public static String ToSizeText(this UInt32 InNum, Int32 InDecimalPlaces = 2, Boolean InBinary = true) => ToSizeText((UInt64)InNum, InDecimalPlaces, InBinary);
	public static String ToSizeText(this Int32 InNum, Int32 InDecimalPlaces = 2, Boolean InBinary = true) => ToSizeText((UInt64)InNum, InDecimalPlaces, InBinary);
	public static String ToSizeText(this UInt16 InNum, Int32 InDecimalPlaces = 2, Boolean InBinary = true) => ToSizeText((UInt64)InNum, InDecimalPlaces, InBinary);
	public static String ToSizeText(this Int16 InNum, Int32 InDecimalPlaces = 2, Boolean InBinary = true) => ToSizeText((UInt64)InNum, InDecimalPlaces, InBinary);
	public static String ToSizeText(this Byte InNum, Int32 InDecimalPlaces = 2, Boolean InBinary = true) => ToSizeText((UInt64)InNum, InDecimalPlaces, InBinary);
	public static String ToSizeText(this SByte InNum, Int32 InDecimalPlaces = 2, Boolean InBinary = true) => ToSizeText((UInt64)InNum, InDecimalPlaces, InBinary);
}