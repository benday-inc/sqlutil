using System;

namespace Benday.SqlUtils.ConsoleUi
{
    public static class Constants
    {
        public static readonly string ExeName = "sqlutil";
        public static readonly string CommandArgumentNameExportData = "exportdata";
        
        public static readonly string ArgumentNameServerName = "server";
        public static readonly string ArgumentNameDatabaseName = "database";
        public static readonly string ArgumentNameUserName = "username";
        public static readonly string ArgumentNamePassword = "password";
        public static readonly string ArgumentNameQuery = "query";
        public static readonly string ArgumentNameScriptType = "scripttype";
        public static readonly string ArgumentNameFilename = "filename";

        public static readonly string ArgumentValueScriptType_Insert = "insert";
        public static readonly string ArgumentValueScriptType_IdentityInsert = "identityinsert";
        public static readonly string ArgumentValueScriptType_MergeInto = "mergeinto";

        public const string ArgumentNameWaitBeforeExit = "wait";
        public const string ArgumentNameQuiet = "quiet";
        public const string ArgumentNameVerbose = "verbose";
    }
}
