// NOTICE: This is unused code! FluentMigrator has some issues with not setting the correct constraints for SQLite databases.
// I wasn't sure what the lesser of the two evils is so I am keeping around both implementations for now.

using static SimplyMeetApi.Constants.DatabaseConstants;

namespace SimplyMeetApi.Migrations;

public abstract class CustomMigration
{
	//===========================================================================================
	// Public Methods
	//===========================================================================================
	public abstract void Up();
	public abstract void Down();

	//===========================================================================================
	// Protected Methods
	//===========================================================================================
	protected String GetPrimaryKeySql(String InColumnName) => $"{PRIMARY_KEY}([{InColumnName}] {AUTOINCREMENT})";
	protected String GetForeignKeySql(String InKeyColumn, String InReferenceTable, String InReferenceColumn, String InOnDelete) => $"{FOREIGN_KEY}({InKeyColumn}) {REFERENCES} {InReferenceTable}({InReferenceColumn}) {InOnDelete}";
}