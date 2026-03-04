# sqlutil
A collection of useful SQL Server database utilities.

Written by Benjamin Day
Pluralsight Author | Microsoft MVP | Scrum.org Professional Scrum Trainer
https://www.benday.com
https://www.honestcheetah.com
info@benday.com
YouTube: https://www.youtube.com/@_benday

*Got ideas for SQL Server utilities you'd like to see? Found a bug? Let us know by submitting an issue https://github.com/benday-inc/sqlutil/issues*. *Want to contribute? Submit a pull request.*

## Command Categories

<dl>

<dt>Connection Management</dt>
<dd>Commands for saving, listing, testing, and removing named database connections</dd>

<dt>Schema Introspection</dt>
<dd>Commands for listing databases, tables, and describing table columns</dd>

<dt>Search</dt>
<dd>Commands for searching tables, columns, column data, stored procedures, proc parameters, and proc source code by name or value pattern</dd>

<dt>Query & Export</dt>
<dd>Run ad-hoc SQL queries and export data as INSERT, INSERT with IDENTITY, or MERGE scripts</dd>

<dt>Database Comparison</dt>
<dd>Compare full schemas or specific objects (tables, views, stored procedures, functions) between two databases</dd>

<dt>Sessions</dt>
<dd>List active sessions on a SQL Server instance</dd>

</dl>

## Installing
sqlutil is distributed as a .NET Tool via NuGet. To install it go to the command prompt and type
`dotnet tool install Benday.SqlUtil -g`

### Prerequisites
- You'll need to install .NET 10+ from https://dotnet.microsoft.com/

## Getting Started
Everything starts with a connection. After you've installed sqlutil, run `sqlutil addconnection` to save a connection. A connection stores the server, database, and credentials so you don't have to supply them on every command.

Connections are named and you can have as many as you'd like.

### Set a Default Connection
There's one default connection. If you only work with one database, run `sqlutil addconnection /server:{server} /database:{database} /default:true` and that will set your default connection. Once set, all commands will use it automatically without needing `/connectionname`.

### Additional Named Connections
To add additional named connections, run `sqlutil addconnection /name:{name} /server:{server} /database:{database}`.

### Running Commands
Once you've set a default connection, you can run any sqlutil command without having to specify any additional connection info.

If you want to run a command against a connection that is NOT your default, supply `/connectionname:{name}`.

You can also skip saved connections entirely and supply `/connectionstring:{connectionstring}` on any command.

### Managing Connections
To add or update a connection use `sqlutil addconnection`. To list connections use `sqlutil listconnections`. To delete a connection use `sqlutil removeconnection`.

## GUI Option
> **Prefer a graphical interface?** Run `sqlutil gui` to launch the sqlutil GUI — a point-and-click interface for all the same functionality without the command line.

## Commands
| Category | Command Name | Description |
| --- | --- | --- |
| Connection Management | [addconnection](#addconnection) | Add or update a saved database connection |
| Connection Management | [listconnections](#listconnections) | List all saved database connections |
| Connection Management | [removeconnection](#removeconnection) | Remove a saved database connection |
| Connection Management | [testconnection](#testconnection) | Test connectivity to a database |
| Schema Introspection | [listdatabases](#listdatabases) | List all databases on a SQL Server instance |
| Schema Introspection | [listtables](#listtables) | List all tables in a database |
| Schema Introspection | [describetable](#describetable) | Show columns and types for a specific table |
| Search | [searchtables](#searchtables) | Search for tables by name pattern |
| Search | [searchcolumns](#searchcolumns) | Search for columns by name or data type pattern |
| Search | [searchcolumndata](#searchcolumndata) | Search for text within table column data |
| Search | [searchprocs](#searchprocs) | Search for stored procedures by name pattern |
| Search | [searchproccode](#searchproccode) | Search stored procedure source code for text |
| Search | [searchprocparams](#searchprocparams) | Search for stored procedure parameters by name pattern |
| Query & Export | [runquery](#runquery) | Run a SQL query and display the results |
| Query & Export | [exportdata](#exportdata) | Export query results as INSERT or MERGE SQL statements |
| Database Comparison | [comparedb](#comparedb) | Full schema comparison between two databases |
| Database Comparison | [comparetables](#comparetables) | Compare table and view schemas between two databases |
| Database Comparison | [compareprocs](#compareprocs) | Compare stored procedure and function definitions between two databases |
| Sessions | [listsessions](#listsessions) | List active database sessions |

# Connection Management
## <a name="addconnection"></a> addconnection
**Add or update a saved database connection.**
### Arguments
| Argument | Is Optional | Data Type | Description |
| --- | --- | --- | --- |
| name | Optional | String | Name for the connection. Omit to update the default connection. |
| connectionstring | Optional | String | Full SQL Server connection string (alternative to specifying individual fields) |
| server | Optional | String | SQL Server hostname or instance (e.g. `localhost` or `server\instance`) |
| database | Optional | String | Database name |
| integratedsecurity | Optional | Boolean | Use Windows integrated security instead of username/password |
| username | Optional | String | SQL Server login username |
| password | Optional | String | SQL Server login password |
| trustservercertificate | Optional | Boolean | Trust the server certificate (useful for local/dev servers) |
| default | Optional | Boolean | Also save this connection as the default |

## <a name="listconnections"></a> listconnections
**List all saved database connections.**

*No arguments.*

## <a name="removeconnection"></a> removeconnection
**Remove a saved database connection.**
### Arguments
| Argument | Is Optional | Data Type | Description |
| --- | --- | --- | --- |
| name | Required | String | Name of the connection to remove |

## <a name="testconnection"></a> testconnection
**Test connectivity to a database.**
### Arguments
| Argument | Is Optional | Data Type | Description |
| --- | --- | --- | --- |
| connectionname | Optional | String | Name of a saved connection |
| connectionstring | Optional | String | SQL Server connection string |

# Schema Introspection
## <a name="listdatabases"></a> listdatabases
**List all databases on a SQL Server instance.**
### Arguments
| Argument | Is Optional | Data Type | Description |
| --- | --- | --- | --- |
| connectionname | Optional | String | Name of a saved connection |
| connectionstring | Optional | String | SQL Server connection string |

## <a name="listtables"></a> listtables
**List all tables in a database.**
### Arguments
| Argument | Is Optional | Data Type | Description |
| --- | --- | --- | --- |
| connectionname | Optional | String | Name of a saved connection |
| connectionstring | Optional | String | SQL Server connection string |
| schema | Optional | String | Filter by schema name (e.g. `dbo`) |

## <a name="describetable"></a> describetable
**Show columns and types for a specific table.**
### Arguments
| Argument | Is Optional | Data Type | Description |
| --- | --- | --- | --- |
| connectionname | Optional | String | Name of a saved connection |
| connectionstring | Optional | String | SQL Server connection string |
| table | Required | String | Name of the table to describe |

# Search
## <a name="searchtables"></a> searchtables
**Search for tables by name pattern.**
### Arguments
| Argument | Is Optional | Data Type | Description |
| --- | --- | --- | --- |
| connectionname | Optional | String | Name of a saved connection |
| connectionstring | Optional | String | SQL Server connection string |
| search | Required | String | Table name search pattern |
| match | Optional | String | Match method: `contains` (default), `exact`, `startswith`, `endswith` |

## <a name="searchcolumns"></a> searchcolumns
**Search for columns by name or data type pattern.**
### Arguments
| Argument | Is Optional | Data Type | Description |
| --- | --- | --- | --- |
| connectionname | Optional | String | Name of a saved connection |
| connectionstring | Optional | String | SQL Server connection string |
| search | Optional | String | Column name search pattern |
| datatype | Optional | String | Data type filter pattern (e.g. `varchar`, `int`) |
| match | Optional | String | Match method: `contains` (default), `exact`, `startswith`, `endswith` |

## <a name="searchcolumndata"></a> searchcolumndata
**Search for text within table column data.**
### Arguments
| Argument | Is Optional | Data Type | Description |
| --- | --- | --- | --- |
| connectionname | Optional | String | Name of a saved connection |
| connectionstring | Optional | String | SQL Server connection string |
| table | Optional | String | Table name filter (supports `%` wildcards) |
| column | Optional | String | Column name filter (supports `%` wildcards) |
| search | Required | String | Text to search for in column data |

## <a name="searchprocs"></a> searchprocs
**Search for stored procedures by name pattern.**
### Arguments
| Argument | Is Optional | Data Type | Description |
| --- | --- | --- | --- |
| connectionname | Optional | String | Name of a saved connection |
| connectionstring | Optional | String | SQL Server connection string |
| search | Required | String | Stored procedure name search pattern |
| match | Optional | String | Match method: `contains` (default), `exact`, `startswith`, `endswith` |

## <a name="searchproccode"></a> searchproccode
**Search stored procedure source code for text.**
### Arguments
| Argument | Is Optional | Data Type | Description |
| --- | --- | --- | --- |
| connectionname | Optional | String | Name of a saved connection |
| connectionstring | Optional | String | SQL Server connection string |
| search | Required | String | Text to search for in stored procedure source code |
| match | Optional | String | Match method: `contains` (default), `exact`, `startswith`, `endswith` |

## <a name="searchprocparams"></a> searchprocparams
**Search for stored procedure parameters by name pattern.**
### Arguments
| Argument | Is Optional | Data Type | Description |
| --- | --- | --- | --- |
| connectionname | Optional | String | Name of a saved connection |
| connectionstring | Optional | String | SQL Server connection string |
| search | Required | String | Parameter name search pattern |
| match | Optional | String | Match method: `contains` (default), `exact`, `startswith`, `endswith` |

# Query & Export
## <a name="runquery"></a> runquery
**Run a SQL query and display the results.**
### Arguments
| Argument | Is Optional | Data Type | Description |
| --- | --- | --- | --- |
| connectionname | Optional | String | Name of a saved connection |
| connectionstring | Optional | String | SQL Server connection string |
| query | Required | String | SQL query to execute |
| filename | Optional | String | Write results to a CSV file instead of stdout |

## <a name="exportdata"></a> exportdata
**Export query results as INSERT or MERGE SQL statements.**
### Arguments
| Argument | Is Optional | Data Type | Description |
| --- | --- | --- | --- |
| connectionname | Optional | String | Name of a saved connection |
| connectionstring | Optional | String | SQL Server connection string |
| query | Required | String | SELECT query to export data from |
| scripttype | Required | String | Script type: `insert`, `identityinsert`, or `mergeinto` |
| filename | Optional | String | Output file path (default: stdout) |

# Database Comparison
## <a name="comparedb"></a> comparedb
**Full schema comparison between two databases (tables, views, stored procedures, functions, foreign keys).**
### Arguments
| Argument | Is Optional | Data Type | Description |
| --- | --- | --- | --- |
| connectionname1 | Optional | String | Name of the first saved connection |
| connectionstring1 | Optional | String | Connection string for the first database |
| connectionname2 | Optional | String | Name of the second saved connection |
| connectionstring2 | Optional | String | Connection string for the second database |

## <a name="comparetables"></a> comparetables
**Compare table and view schemas between two databases.**
### Arguments
| Argument | Is Optional | Data Type | Description |
| --- | --- | --- | --- |
| connectionname1 | Optional | String | Name of the first saved connection |
| connectionstring1 | Optional | String | Connection string for the first database |
| connectionname2 | Optional | String | Name of the second saved connection |
| connectionstring2 | Optional | String | Connection string for the second database |

## <a name="compareprocs"></a> compareprocs
**Compare stored procedure and function definitions between two databases.**
### Arguments
| Argument | Is Optional | Data Type | Description |
| --- | --- | --- | --- |
| connectionname1 | Optional | String | Name of the first saved connection |
| connectionstring1 | Optional | String | Connection string for the first database |
| connectionname2 | Optional | String | Name of the second saved connection |
| connectionstring2 | Optional | String | Connection string for the second database |

# Sessions
## <a name="listsessions"></a> listsessions
**List active database sessions.**
### Arguments
| Argument | Is Optional | Data Type | Description |
| --- | --- | --- | --- |
| connectionname | Optional | String | Name of a saved connection |
| connectionstring | Optional | String | SQL Server connection string |
