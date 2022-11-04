namespace SimplyMeetApi.Migrations;

[Migration(2)]
public class D2 : Migration
{
	//===========================================================================================
	// Global Variables
	//===========================================================================================
	#region Const Fields
	private const String D2_ACCOUNTS_TABLE = "ACCOUNTS";
	#endregion

	//===========================================================================================
	// Migration Models
	//===========================================================================================
	private class D2_AccountModel
	{
		public Int32 Id { get; set; }

		public String PublicId { get; set; }
		public String PublicKey_Base64 { get; set; }
		public DateTime Creation { get; set; }
		public DateTime? LastActive { get; set; }
		public EAccountStatus Status { get; set; }

		public Int32 ProfileId { get; set; }
		public Int32 FilterId { get; set; }
	}

	//===========================================================================================
	// Public Methods
	//===========================================================================================
	public override void Up()
	{
		Rename.Column(nameof(D1.D1_AccountModel.LastLogin)).OnTable(D2_ACCOUNTS_TABLE).To(nameof(D2_AccountModel.LastActive));
	}
	public override void Down()
	{
		Rename.Column(nameof(D2_AccountModel.LastActive)).OnTable(D2_ACCOUNTS_TABLE).To(nameof(D1.D1_AccountModel.LastLogin));
	}
}