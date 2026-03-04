using System.Data;
using Benday.CommandsFramework;
using Benday.SqlUtils.Api;

namespace Benday.SqlUtils.ShovelCli;

public abstract class CompareDatabaseCommandBase : DatabaseCommandBase
{
    protected const string ArgConnectionName1 = "connectionname1";
    protected const string ArgConnectionString1 = "connectionstring1";
    protected const string ArgConnectionName2 = "connectionname2";
    protected const string ArgConnectionString2 = "connectionstring2";

    public CompareDatabaseCommandBase(CommandExecutionInfo info, ITextOutputProvider outputProvider)
        : base(info, outputProvider) { }

    protected void AddTwoConnectionArguments(ArgumentCollection args)
    {
        args.AddString(ArgConnectionName1)
            .AsNotRequired()
            .WithDescription("Name of first saved connection");
        args.AddString(ArgConnectionString1)
            .AsNotRequired()
            .WithDescription("Connection string for first database");
        args.AddString(ArgConnectionName2)
            .AsNotRequired()
            .WithDescription("Name of second saved connection");
        args.AddString(ArgConnectionString2)
            .AsNotRequired()
            .WithDescription("Connection string for second database");
    }

    protected void ValidateTwoConnectionArguments()
    {
        if (!Arguments.HasValue(ArgConnectionName1) && !Arguments.HasValue(ArgConnectionString1))
            throw new KnownException($"You must provide /{ArgConnectionName1} or /{ArgConnectionString1}.");
        if (!Arguments.HasValue(ArgConnectionName2) && !Arguments.HasValue(ArgConnectionString2))
            throw new KnownException($"You must provide /{ArgConnectionName2} or /{ArgConnectionString2}.");
    }

    private string ResolveConnectionString(string nameArg, string stringArg)
    {
        if (Arguments.HasValue(stringArg))
            return Arguments.GetStringValue(stringArg);

        var connectionName = Arguments.GetStringValue(nameArg);
        var configKey = $"{ConnectionConfigPrefix}{connectionName}";

        if (!ExecutionInfo.Configuration.HasValue(configKey))
            throw new KnownException(
                $"No saved connection named '{connectionName}'. Use 'shovel addconnection' to create one.");

        return ExecutionInfo.Configuration.GetValue(configKey);
    }

    protected string GetConnectionString1() =>
        ResolveConnectionString(ArgConnectionName1, ArgConnectionString1);

    protected string GetConnectionString2() =>
        ResolveConnectionString(ArgConnectionName2, ArgConnectionString2);

    protected string GetDb1Name() =>
        Arguments.HasValue(ArgConnectionName1) ? Arguments.GetStringValue(ArgConnectionName1) : "db1";

    protected string GetDb2Name() =>
        Arguments.HasValue(ArgConnectionName2) ? Arguments.GetStringValue(ArgConnectionName2) : "db2";

    // ── SQL queries ────────────────────────────────────────────────────

    private const string TableColumnsQuery = @"
SELECT col.TABLE_NAME, col.COLUMN_NAME, col.DATA_TYPE,
       ISNULL(CAST(col.CHARACTER_MAXIMUM_LENGTH AS nvarchar(20)), '') AS CHARACTER_MAXIMUM_LENGTH,
       col.IS_NULLABLE, col.ORDINAL_POSITION
FROM information_schema.tables t
JOIN information_schema.columns col
    ON t.table_catalog = col.table_catalog
    AND t.table_schema = col.table_schema
    AND t.table_name = col.table_name
WHERE t.table_type = 'BASE TABLE'
ORDER BY col.table_name, col.column_name";

    private const string ViewColumnsQuery = @"
SELECT col.TABLE_NAME, col.COLUMN_NAME, col.DATA_TYPE,
       ISNULL(CAST(col.CHARACTER_MAXIMUM_LENGTH AS nvarchar(20)), '') AS CHARACTER_MAXIMUM_LENGTH,
       col.IS_NULLABLE, col.ORDINAL_POSITION
FROM information_schema.tables t
JOIN information_schema.columns col
    ON t.table_catalog = col.table_catalog
    AND t.table_schema = col.table_schema
    AND t.table_name = col.table_name
WHERE t.table_type = 'VIEW'
  AND t.table_name NOT IN ('sysconstraints', 'syssegments')
ORDER BY col.table_name, col.column_name";

    private const string PrimaryKeyQuery = @"
SELECT kcu.COLUMN_NAME, kcu.ORDINAL_POSITION
FROM INFORMATION_SCHEMA.key_column_usage kcu
JOIN INFORMATION_SCHEMA.table_constraints tc ON tc.constraint_name = kcu.constraint_name
WHERE kcu.table_name = @TABLE_NAME AND tc.constraint_type = 'primary key'
ORDER BY kcu.ORDINAL_POSITION";

    private const string StoredProcsQuery = @"
SELECT specific_catalog, specific_schema, routine_name
FROM information_schema.routines
WHERE routine_type = 'PROCEDURE' AND routine_name NOT LIKE 'dt_%'
ORDER BY routine_name";

    private const string FunctionsQuery = @"
SELECT specific_catalog, specific_schema, routine_name
FROM information_schema.routines
WHERE routine_type = 'FUNCTION' AND routine_name NOT LIKE 'dt_%'
ORDER BY routine_name";

    private const string ProcParamsQuery = @"
SELECT specific_catalog, specific_schema, specific_name, ordinal_position,
       parameter_mode, parameter_name, data_type,
       ISNULL(CAST(character_maximum_length AS nvarchar(20)), '') AS character_maximum_length
FROM INFORMATION_SCHEMA.parameters
ORDER BY specific_catalog, specific_schema, specific_name, ordinal_position";

    private const string RoutineSourceQuery = @"
SELECT m.definition
FROM sys.sql_modules m
JOIN sys.objects o ON o.object_id = m.object_id
WHERE o.name = @PROC_NAME AND RTRIM(o.type) = @PROC_TYPE";

    private const string ForeignKeysQuery = @"
SELECT refcon.constraint_name,
       ccu_from.table_name AS from_table, ccu_from.column_name AS from_column,
       refcon.unique_constraint_name,
       ccu_to.table_name AS to_table, ccu_to.column_name AS to_column
FROM information_schema.REFERENTIAL_CONSTRAINTS refcon
JOIN information_schema.constraint_column_usage ccu_from
    ON refcon.constraint_name = ccu_from.constraint_name
JOIN information_schema.constraint_column_usage ccu_to
    ON refcon.unique_constraint_name = ccu_to.constraint_name
ORDER BY refcon.constraint_name";

    // ── Helpers ────────────────────────────────────────────────────────

    private static DataTable QueryDb(string connStr, string query, Dictionary<string, string>? args = null)
    {
        var util = new SqlServerDatabaseUtility();
        util.Initialize(connStr);
        return util.RunQuery(query, args) ?? new DataTable();
    }

    private static Dictionary<string, List<DataRow>> GroupBy(DataTable table, string keyColumn)
    {
        var result = new Dictionary<string, List<DataRow>>(StringComparer.OrdinalIgnoreCase);
        foreach (DataRow row in table.Rows)
        {
            var key = row[keyColumn]?.ToString() ?? "";
            if (!result.TryGetValue(key, out var list))
                result[key] = list = new List<DataRow>();
            list.Add(row);
        }
        return result;
    }

    // ── Table / view comparison ────────────────────────────────────────

    protected List<DbDiff> CompareTables(string conn1, string conn2, string db1Name, string db2Name) =>
        CompareTableOrView(conn1, conn2, db1Name, db2Name, TableColumnsQuery, "table", comparePrimaryKeys: true);

    protected List<DbDiff> CompareViews(string conn1, string conn2, string db1Name, string db2Name) =>
        CompareTableOrView(conn1, conn2, db1Name, db2Name, ViewColumnsQuery, "view", comparePrimaryKeys: false);

    private List<DbDiff> CompareTableOrView(
        string conn1, string conn2, string db1Name, string db2Name,
        string query, string objectType, bool comparePrimaryKeys)
    {
        var diffs = new List<DbDiff>();

        var cols1 = QueryDb(conn1, query);
        var cols2 = QueryDb(conn2, query);

        var tables1 = GroupBy(cols1, "TABLE_NAME");
        var tables2 = GroupBy(cols2, "TABLE_NAME");

        foreach (var key in tables1.Keys)
            if (!tables2.ContainsKey(key))
                diffs.Add(new DbDiff(db1Name, $"{objectType} missing in {db2Name}", key, "", "", ""));

        foreach (var key in tables2.Keys)
            if (!tables1.ContainsKey(key))
                diffs.Add(new DbDiff(db2Name, $"{objectType} missing in {db1Name}", key, "", "", ""));

        foreach (var key in tables1.Keys)
        {
            if (!tables2.ContainsKey(key)) continue;

            if (comparePrimaryKeys)
                ComparePrimaryKeys(diffs, conn1, conn2, db1Name, db2Name, key);

            var rows1 = tables1[key];
            var rows2 = tables2[key];

            if (rows1.Count != rows2.Count)
                diffs.Add(new DbDiff(db1Name, "Column count is different", key, "",
                    rows1.Count.ToString(), rows2.Count.ToString()));

            var colIdx1 = rows1.ToDictionary(r => r["COLUMN_NAME"].ToString()!, StringComparer.OrdinalIgnoreCase);
            var colIdx2 = rows2.ToDictionary(r => r["COLUMN_NAME"].ToString()!, StringComparer.OrdinalIgnoreCase);

            foreach (var col in colIdx1.Keys)
                if (!colIdx2.ContainsKey(col))
                    diffs.Add(new DbDiff(db1Name, $"Column missing in {db2Name}", key, col, "", ""));

            foreach (var col in colIdx2.Keys)
                if (!colIdx1.ContainsKey(col))
                    diffs.Add(new DbDiff(db2Name, $"Column missing in {db1Name}", key, col, "", ""));

            foreach (var col in colIdx1.Keys)
            {
                if (!colIdx2.ContainsKey(col)) continue;

                var r1 = colIdx1[col];
                var r2 = colIdx2[col];

                var dt1 = r1["DATA_TYPE"].ToString()!;
                var dt2 = r2["DATA_TYPE"].ToString()!;

                if (!string.Equals(dt1, dt2, StringComparison.OrdinalIgnoreCase))
                {
                    diffs.Add(new DbDiff(db1Name, "Column datatype", key, col, dt1, dt2));
                }
                else
                {
                    var len1 = r1["CHARACTER_MAXIMUM_LENGTH"].ToString()!;
                    var len2 = r2["CHARACTER_MAXIMUM_LENGTH"].ToString()!;
                    if (!string.IsNullOrEmpty(len1) && len1 != len2)
                        diffs.Add(new DbDiff(db1Name, "Column length", key, col, len1, len2));
                }

                var null1 = r1["IS_NULLABLE"].ToString()!;
                var null2 = r2["IS_NULLABLE"].ToString()!;
                if (!string.Equals(null1, null2, StringComparison.OrdinalIgnoreCase))
                    diffs.Add(new DbDiff(db1Name, "Nullability", key, col, null1, null2));
            }
        }

        return diffs;
    }

    private void ComparePrimaryKeys(
        List<DbDiff> diffs, string conn1, string conn2,
        string db1Name, string db2Name, string tableName)
    {
        var args = new Dictionary<string, string> { { "TABLE_NAME", tableName } };
        var pk1 = QueryDb(conn1, PrimaryKeyQuery, args);
        var pk2 = QueryDb(conn2, PrimaryKeyQuery, args);

        if (pk1.Rows.Count == 0 && pk2.Rows.Count == 0) return;

        if ((pk1.Rows.Count == 0) != (pk2.Rows.Count == 0))
        {
            diffs.Add(new DbDiff(db1Name, "Primary key is missing", tableName, "",
                pk1.Rows.Count.ToString(), pk2.Rows.Count.ToString()));
            return;
        }

        if (pk1.Rows.Count != pk2.Rows.Count)
        {
            diffs.Add(new DbDiff(db1Name, "Primary key column count is different", tableName, "",
                pk1.Rows.Count.ToString(), pk2.Rows.Count.ToString()));
            return;
        }

        for (int i = 0; i < pk1.Rows.Count; i++)
        {
            var col1 = pk1.Rows[i]["COLUMN_NAME"].ToString()!;
            var col2 = pk2.Rows[i]["COLUMN_NAME"].ToString()!;
            if (!string.Equals(col1, col2, StringComparison.OrdinalIgnoreCase))
                diffs.Add(new DbDiff(db1Name, "Primary key definition does not match", tableName, "", col1, col2));
        }
    }

    // ── Stored procedure / function comparison ─────────────────────────

    protected List<DbDiff> CompareStoredProcs(string conn1, string conn2, string db1Name, string db2Name) =>
        CompareRoutines(conn1, conn2, db1Name, db2Name, StoredProcsQuery, "P", "procedure");

    protected List<DbDiff> CompareFunctions(string conn1, string conn2, string db1Name, string db2Name) =>
        CompareRoutines(conn1, conn2, db1Name, db2Name, FunctionsQuery, "FN", "function");

    private List<DbDiff> CompareRoutines(
        string conn1, string conn2, string db1Name, string db2Name,
        string routinesQuery, string sqlType, string objectType)
    {
        var diffs = new List<DbDiff>();

        var procs1 = QueryDb(conn1, routinesQuery);
        var procs2 = QueryDb(conn2, routinesQuery);

        var procIdx1 = procs1.Rows.Cast<DataRow>()
            .ToDictionary(r => r["routine_name"].ToString()!, StringComparer.OrdinalIgnoreCase);
        var procIdx2 = procs2.Rows.Cast<DataRow>()
            .ToDictionary(r => r["routine_name"].ToString()!, StringComparer.OrdinalIgnoreCase);

        foreach (var key in procIdx1.Keys)
            if (!procIdx2.ContainsKey(key))
                diffs.Add(new DbDiff(db1Name, $"{objectType} missing in {db2Name}", key, "", "", ""));

        foreach (var key in procIdx2.Keys)
            if (!procIdx1.ContainsKey(key))
                diffs.Add(new DbDiff(db2Name, $"{objectType} missing in {db1Name}", key, "", "", ""));

        var allParams1 = QueryDb(conn1, ProcParamsQuery);
        var allParams2 = QueryDb(conn2, ProcParamsQuery);

        var paramsByProc1 = GroupBy(allParams1, "specific_name");
        var paramsByProc2 = GroupBy(allParams2, "specific_name");

        foreach (var key in procIdx1.Keys)
        {
            if (!procIdx2.ContainsKey(key)) continue;

            var src1 = GetRoutineSource(conn1, key, sqlType);
            var src2 = GetRoutineSource(conn2, key, sqlType);

            if (!string.Equals(src1.Trim(), src2.Trim(), StringComparison.Ordinal))
            {
                diffs.Add(new DbDiff("BOTH", "Source code mismatch", key, "", "", ""));
                AddFirstLineDiff(diffs, key, src1, src2);
            }

            var params1 = paramsByProc1.ContainsKey(key)
                ? paramsByProc1[key].ToDictionary(r => r["parameter_name"].ToString()!, StringComparer.OrdinalIgnoreCase)
                : new Dictionary<string, DataRow>(StringComparer.OrdinalIgnoreCase);
            var params2 = paramsByProc2.ContainsKey(key)
                ? paramsByProc2[key].ToDictionary(r => r["parameter_name"].ToString()!, StringComparer.OrdinalIgnoreCase)
                : new Dictionary<string, DataRow>(StringComparer.OrdinalIgnoreCase);

            if (params1.Count != params2.Count)
                diffs.Add(new DbDiff(db1Name, "Parameter count is different", key, "",
                    params1.Count.ToString(), params2.Count.ToString()));

            foreach (var p in params1.Keys)
                if (!params2.ContainsKey(p))
                    diffs.Add(new DbDiff(db1Name, $"Parameter missing in {db2Name}", key, p, "", ""));

            foreach (var p in params2.Keys)
                if (!params1.ContainsKey(p))
                    diffs.Add(new DbDiff(db2Name, $"Parameter missing in {db1Name}", key, p, "", ""));

            foreach (var p in params1.Keys)
            {
                if (!params2.ContainsKey(p)) continue;

                var r1 = params1[p];
                var r2 = params2[p];

                var dt1 = r1["data_type"].ToString()!;
                var dt2 = r2["data_type"].ToString()!;

                if (!string.Equals(dt1, dt2, StringComparison.OrdinalIgnoreCase))
                {
                    diffs.Add(new DbDiff(db1Name, "Parameter datatype", key, p, dt1, dt2));
                }
                else
                {
                    var len1 = r1["character_maximum_length"].ToString()!;
                    var len2 = r2["character_maximum_length"].ToString()!;
                    if (!string.IsNullOrEmpty(len1) && len1 != len2)
                        diffs.Add(new DbDiff(db1Name, "Parameter length", key, p, len1, len2));
                }

                var mode1 = r1["parameter_mode"].ToString()!;
                var mode2 = r2["parameter_mode"].ToString()!;
                if (!string.Equals(mode1, mode2, StringComparison.OrdinalIgnoreCase))
                    diffs.Add(new DbDiff(db1Name, "Parameter direction", key, p, mode1, mode2));
            }
        }

        return diffs;
    }

    private static string GetRoutineSource(string connStr, string routineName, string sqlType)
    {
        var args = new Dictionary<string, string>
        {
            { "PROC_NAME", routineName },
            { "PROC_TYPE", sqlType }
        };

        var result = QueryDb(connStr, RoutineSourceQuery, args);
        if (result.Rows.Count == 0) return "";
        return result.Rows[0]["definition"]?.ToString() ?? "";
    }

    private static void AddFirstLineDiff(List<DbDiff> diffs, string routineName, string src1, string src2)
    {
        var lines1 = src1.Split('\n');
        var lines2 = src2.Split('\n');
        var maxLines = Math.Max(lines1.Length, lines2.Length);

        for (int i = 0; i < maxLines; i++)
        {
            var line1 = i < lines1.Length ? lines1[i] : null;
            var line2 = i < lines2.Length ? lines2[i] : null;

            if (line1 != line2)
            {
                diffs.Add(new DbDiff("BOTH", "Source code mismatch", routineName,
                    $"First difference at line #{i + 1}",
                    line1 ?? "(null)", line2 ?? "(null)"));
                break;
            }
        }
    }

    // ── Foreign key comparison ─────────────────────────────────────────

    protected List<DbDiff> CompareForeignKeys(string conn1, string conn2, string db1Name, string db2Name)
    {
        var diffs = new List<DbDiff>();

        var fkIdx1 = GroupBy(QueryDb(conn1, ForeignKeysQuery), "constraint_name");
        var fkIdx2 = GroupBy(QueryDb(conn2, ForeignKeysQuery), "constraint_name");

        var processed = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        CompareFkSet(diffs, fkIdx1, fkIdx2, db1Name, db2Name, processed);
        CompareFkSet(diffs, fkIdx2, fkIdx1, db2Name, db1Name, processed);

        return diffs;
    }

    private static void CompareFkSet(
        List<DbDiff> diffs,
        Dictionary<string, List<DataRow>> source, Dictionary<string, List<DataRow>> target,
        string sourceDb, string targetDb, HashSet<string> processed)
    {
        foreach (var key in source.Keys)
        {
            if (processed.Contains(key)) continue;
            processed.Add(key);

            if (!target.ContainsKey(key))
            {
                var row = source[key][0];
                diffs.Add(new DbDiff(sourceDb, $"Foreign key is missing in {targetDb}", key,
                    "From table --> to table",
                    row["from_table"].ToString()!, row["to_table"].ToString()!));
                continue;
            }

            var rows1 = source[key];
            var rows2 = target[key];
            var r1 = rows1[0];
            var r2 = rows2[0];

            if (!string.Equals(r1["unique_constraint_name"].ToString(),
                    r2["unique_constraint_name"].ToString(), StringComparison.OrdinalIgnoreCase))
                diffs.Add(new DbDiff(sourceDb, "Foreign keys do not point to the same primary key",
                    key, "FK --> PK",
                    r1["unique_constraint_name"].ToString()!,
                    r2["unique_constraint_name"].ToString()!));

            if (!string.Equals(r1["from_table"].ToString(), r2["from_table"].ToString(), StringComparison.OrdinalIgnoreCase) ||
                !string.Equals(r1["to_table"].ToString(), r2["to_table"].ToString(), StringComparison.OrdinalIgnoreCase))
                diffs.Add(new DbDiff(sourceDb, "Foreign keys do not point to the same table",
                    key, "From table --> to table",
                    $"{r1["from_table"]} --> {r1["to_table"]}",
                    $"{r2["from_table"]} --> {r2["to_table"]}"));

            if (rows1.Count != rows2.Count)
            {
                diffs.Add(new DbDiff(sourceDb, "Foreign keys do not have the same number of columns",
                    key, "", rows1.Count.ToString(), rows2.Count.ToString()));
            }
            else
            {
                for (int i = 0; i < rows1.Count; i++)
                {
                    if (!string.Equals(rows1[i]["from_column"].ToString(), rows2[i]["from_column"].ToString(), StringComparison.OrdinalIgnoreCase) ||
                        !string.Equals(rows1[i]["to_column"].ToString(), rows2[i]["to_column"].ToString(), StringComparison.OrdinalIgnoreCase))
                        diffs.Add(new DbDiff(sourceDb, "Foreign keys do not point to the same columns",
                            key, "From column --> to column",
                            $"{rows1[i]["from_column"]} --> {rows1[i]["to_column"]}",
                            $"{rows2[i]["from_column"]} --> {rows2[i]["to_column"]}"));
                }
            }
        }
    }

    // ── Output ─────────────────────────────────────────────────────────

    protected void WriteDiffs(List<DbDiff> diffs)
    {
        if (diffs.Count == 0)
        {
            WriteLine("No differences found.");
            return;
        }

        var table = new DataTable();
        table.Columns.Add("Source");
        table.Columns.Add("Diff Type");
        table.Columns.Add("Object");
        table.Columns.Add("Property");
        table.Columns.Add("Value 1");
        table.Columns.Add("Value 2");

        foreach (var d in diffs)
            table.Rows.Add(d.Source, d.DiffType, d.ObjectName, d.PropertyName, d.Value1, d.Value2);

        WriteDataTable(table);
    }
}

public record DbDiff(
    string Source,
    string DiffType,
    string ObjectName,
    string PropertyName,
    string Value1,
    string Value2);
