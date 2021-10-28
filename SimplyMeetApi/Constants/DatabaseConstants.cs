using System;

namespace SimplyMeetApi.Constants
{
	public static class DatabaseConstants
	{
		//===========================================================================================
		// Global Variables
		//===========================================================================================
		#region Const Fields
		public const String ID_NAME = "Id";

		// Tables
		public const String CARDS_TABLE = "CARDS";

		public const String ACCOUNTS_TABLE = "ACCOUNTS";
		public const String ACCOUNT_ROLES_TABLE = "ACCOUNT_ROLES";
		public const String PROFILES_TABLE = "PROFILES";
		public const String PROFILE_TAGS_TABLE = "PROFILE_TAGS";
		public const String PROFILE_SEXUALITIES_TABLE = "PROFILE_SEXUALITIES";
		public const String FILTERS_TABLE = "FILTERS";

		public const String ROLES_TABLE = "ROLES";
		public const String TAGS_TABLE = "TAGS";
		public const String SEXUALITIES_TABLE = "SEXUALITIES";

		public const String PRONOUNS_TABLE = "PRONOUNS";
		public const String SEXES_TABLE = "SEXES";
		public const String GENDERS_TABLE = "GENDERS";
		public const String REGIONS_TABLE = "REGIONS";
		public const String COUNTRIES_TABLE = "COUNTRIES";

		public const String MATCH_CHOICES_TABLE = "MATCH_CHOICES";
		public const String MATCHES_TABLE = "MATCHES";

		public const String MESSAGES_TABLE = "MESSAGES";

		public const String REPORTS_TABLE = "REPORTS";
		public const String REPORT_REASONS_TABLE = "REPORT_REASONS";

		// Keywords
		public const String CREATE_TABLE = "CREATE TABLE";
		public const String SELECT = "SELECT";
		public const String SELECT_COUNT_FROM = "SELECT COUNT(*) FROM";
		public const String SELECT_ALL_FROM = "SELECT * FROM";
		public const String DROP_TABLE = "DROP TABLE";
		public const String DELETE = "DELETE";
		public const String INSERT_INTO = "INSERT INTO";
		public const String UPDATE = "UPDATE";
		public const String FROM = "FROM";
		public const String LEFT_JOIN = "LEFT JOIN";
		public const String ON = "ON";
		public const String GROUP_BY = "GROUP BY";
		public const String ORDER_BY = "ORDER BY";
		public const String ASC = "ASC";
		public const String DESC = "DESC";
		public const String WHERE = "WHERE";
		public const String VALUES = "VALUES";
		public const String SET = "SET";
		public const String AND = "AND";
		public const String OR = "OR";
		public const String NOT = "NOT";
		public const String IN = "IN";
		public const String AS = "AS";
		public const String COUNT = "COUNT";
		public const String LIMIT = "LIMIT";
		public const String OFFSET = "OFFSET";
		public const String IS = "IS";
		public const String NULL = "NULL";
		public const String CAST = "CAST";

		public const String INTEGER = "INTEGER";
		public const String TEXT = "TEXT";
		public const String UNIQUE = "UNIQUE";
		public const String NOT_NULL = "NOT NULL";

		public const String FOREIGN_KEY = "FOREIGN KEY";
		public const String REFERENCES = "REFERENCES";
		public const String ON_DELETE_CASCADE = "ON DELETE CASCADE";
		public const String ON_DELETE_SET_NULL = "ON DELETE SET NULL";

		public const String PRIMARY_KEY = "PRIMARY KEY";
		public const String AUTOINCREMENT = "AUTOINCREMENT";

		// SQLite Specific
		public const String RETURN_ID = ";SELECT last_insert_rowid();";
		public const String DATE = "DATE";
		public const String JULIANDAY = "JULIANDAY";
		public const String NOW = "'NOW'";
		public const String DAYS = "DAYS";
		public const String YEARS = "YEARS";
		#endregion
	}
}