namespace SimplyMeetApi.Extensions;

public static class MigrationExtensions
{
	//===========================================================================================
	// Public Static Methods
	//===========================================================================================
	public static ICreateTableColumnOptionOrWithColumnSyntax WithIdColumn(this ICreateTableWithColumnSyntax InTableWithColumnSyntax, String InName)
	{
		return InTableWithColumnSyntax
			.WithColumn(InName).AsInt32().NotNullable().PrimaryKey().Identity().Unique()
			;
	}
}