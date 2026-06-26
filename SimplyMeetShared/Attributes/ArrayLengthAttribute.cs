namespace SimplyMeetShared.Attributes;

public class ArrayLengthAttribute : ValidationAttribute
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Properties
	public Int32 MinimumLength { get; set; }
	public Int32 MaximumLength { get; set; }
	#endregion

	//===========================================================================================
	// Public Methods
	//===========================================================================================
	public ArrayLengthAttribute(Int32 InMaximumLength)
	{
		MaximumLength = InMaximumLength;
	}

	//===========================================================================================
	// Protected Methods
	//===========================================================================================
	protected override ValidationResult IsValid(Object InValue, ValidationContext InContext)
	{
		if (InValue is not Array Arr) return new ValidationResult($"Type is not an array.");
		if (Arr.Length < MinimumLength) return new ValidationResult($"Minimum allowed items is {MinimumLength}");
		if (Arr.Length > MaximumLength) return new ValidationResult($"Maximum allowed items is {MaximumLength}");

		return ValidationResult.Success;
	}
}
