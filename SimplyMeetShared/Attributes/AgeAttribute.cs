namespace SimplyMeetShared.Attributes;

public class AgeAttribute : ValidationAttribute
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Properties
	public Int32 Min { get; set; }
	public Int32 Max { get; set; }
	#endregion

	//===========================================================================================
	// Public Methods
	//===========================================================================================
	public override Boolean IsValid(Object InValue)
	{
		var MyDate = InValue as DateTime?;
		if (MyDate == null) return true;

		var Age = MyDate.Value.GetAge();
		return Age >= Min && Age <= Max;
	}
}