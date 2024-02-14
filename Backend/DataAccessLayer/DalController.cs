using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using log4net;
using System.Reflection;
using System.Data.SQLite;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    /// <summary>
    /// DalController is an abstract class, that allows interacting with each table of the database.
    /// </summary>
    public abstract class DalController
    {
        protected string _connectionString;
        protected string[] _primaryKeyNames;
        protected string _tableName;
        private const string SQLITE_SEQUENCE = "sqlite_sequence";
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Initializes a new instance of the DalController class,
        /// according to the given table name and primary-key columns.
        /// </summary>
        /// <param name="tableName">The table name of the DTO in DB.</param>
        /// <param name="primaryKeyNames">The columns name of the PK.</param>
        public DalController(string tableName, string[] primaryKeyNames)
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            this._connectionString = $"Data Source={path}; Version=3;";
            this._tableName = tableName;
            this._primaryKeyNames = primaryKeyNames;
            log.Debug($"Created new DalController.");
        }

        /// <summary>
        /// This method converts the database reader values into a corresponding DTO object.
        /// </summary>
        /// <param name="reader">The reader in the DB.</param>
        /// <returns>The converted DTO object.</returns>
        protected abstract DTO ConvertReaderToObject(SQLiteDataReader reader);

        /// <summary>
        /// The method gets all the records from the table.
        /// </summary>
        /// <returns>List of DTOs representing the records in the table.</returns>
        /// <exception cref="Exception">If SQL Exception occured.</exception>
        public List<DTO> Select()
        {
            List<DTO> results = new List<DTO>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {_tableName};";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add(ConvertReaderToObject(dataReader));
                    }
                }
                catch
                {
                    log.Error($"Error: failed in select from {_tableName}.");
                    throw new Exception($"Error: failed in select from {_tableName}.");
                }
                finally
                {
                    if (dataReader != null)
                    {
                        dataReader.Close();
                    }

                    command.Dispose();
                    connection.Close();
                }

            }
            log.Debug($"Got all records from table {_tableName}.");
            return results;
        }

        /// <summary>
        /// The method gets all the records from the table by some filter.
        /// </summary>
        /// <param name="attributeNames">The names of the columns we want to filter.</param>
        /// <param name="attributeValues">The values to the columns we want to filter.</param>
        /// <returns>All the filtered records from the table, represented as a List of DTO objects.</returns>
        /// <exception cref="Exception">If SQL Exception occured.</exception>
        public List<DTO> Select(string[] attributeNames, string[] attributeValues)
        {
            List<DTO> results = new List<DTO>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                string quary = $"SELECT * FROM {_tableName} WHERE ";
                for (int i = 0; i < attributeNames.Length; i++)
                {
                    quary += $"{attributeNames[i]} = @{attributeNames[i]} ";
                    if (i < attributeNames.Length - 1)
                        quary += "AND ";
                }

                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = quary + ";";
                SQLiteDataReader dataReader = null;
                try
                {
                    for (int i = 0; i < attributeValues.Length; i++)
                    {
                        command.Parameters.Add(new SQLiteParameter(attributeNames[i], attributeValues[i]));
                    }
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add(ConvertReaderToObject(dataReader));
                    }
                }
                catch
                {
                    log.Error($"Error: failed in select from {_tableName}.");
                    throw new Exception($"Error: failed in select from {_tableName}");
                }
                finally
                {
                    if (dataReader != null)
                    {
                        dataReader.Close();
                    }

                    command.Dispose();
                    connection.Close();
                }

            }
            log.Debug($"Got all records from table {_tableName}.");
            return results;
        }

        /// <summary>
        /// This method returns the max value of a specific column in the table.
        /// </summary>
        /// <param name="columnName">The name of the column in the table.</param>
        /// <returns>Max int value of the column in DB.</returns>
        /// <exception cref="Exception">If SQL Exception ocourd.</exception>
        public int GetMaxValue(string columnName)
        {
            int result = 0;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select max({columnName}) from {_tableName};";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        result = dataReader.GetInt32(0);
                    }
                }
                catch
                {
                    log.Error($"Error: Failed in select max {columnName} from {_tableName}.");
                    throw new Exception($"Error: Failed in select max {columnName} from {_tableName}.");
                }
                finally
                {
                    if (dataReader != null)
                    {
                        dataReader.Close();
                    }

                    command.Dispose();
                    connection.Close();
                }

            }
            log.Debug($"Got max {columnName} from {_tableName}.");
            return result;
        }

        /// <summary>
        /// This method inserts a new record to the table in the database.
        /// </summary>
        /// <param name="attributeNames">The column names in DB.</param>
        /// <param name="attributeValues">The new record values.</param>
        /// <exception cref="Exception">If SQL Exception occured.</exception>
        public void Insert(string[] attributeNames, string[] attributeValues)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                string query = $"INSERT INTO {_tableName} (";
                for (int i = 0; i < attributeNames.Length; i++)
                {
                    query += $"{attributeNames[i]} ";
                    if (i < attributeValues.Length - 1)
                        query += ",";
                }
                query += ") VALUES (";
                for (int i = 0; i < attributeNames.Length; i++)
                {
                    query += $"@{attributeNames[i]} ";
                    if (i < attributeValues.Length - 1)
                        query += ",";
                }
                query += ");";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                try
                {
                    connection.Open();
                    for (int i = 0; i < attributeNames.Length; i++)
                    {
                        SQLiteParameter param = new SQLiteParameter(@attributeNames[i], attributeValues[i]);
                        command.Parameters.Add(param);
                    }
                    
                    command.Prepare();

                    res = command.ExecuteNonQuery();

                    log.Debug($"Inserted to {_tableName}.");
                }
                catch
                {
                    log.Error($"Error: failed in insert to {_tableName}.");
                    throw new Exception($"Error: failed in insert to {_tableName}.");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// The method deletes a row from the table in the DB.
        /// </summary>
        /// <param name="keyValues">PRIMARY KEYS values of the DTO.</param>
        /// <returns>True if some row was deleted and false if no rows were deleted.</returns>
        /// <exception cref="Exception">If SQL Exception ocourd</exception>
        public bool Delete(string[] keyValues)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                string quary = $"DELETE FROM {_tableName} ";
                quary += "WHERE ";
                for (int i = 0; i < keyValues.Length; i++)
                {
                    quary += $"{_primaryKeyNames[i]} = @{_primaryKeyNames[i]}";
                    if (i < keyValues.Length - 1)
                        quary += " AND ";
                }
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = quary + ";"
                };
                try
                {
                    for (int i = 0; i < keyValues.Length; i++)//to where part
                    {
                        command.Parameters.Add(new SQLiteParameter(_primaryKeyNames[i], keyValues[i]));
                    }
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Debug($"Deleted row from {_tableName}.");
                }
                catch
                {
                    log.Error($"Error: failed to delete from {_tableName}.");
                    throw new Exception($"Error: failed to delete from {_tableName}.");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }

        /// <summary>
        /// This method deletes all the table data from the database.
        /// </summary>
        /// <returns>Returns true if deleted at least 1 row from the database.</returns>
        /// <exception cref="Exception">If an error has occured while deleting from database.</exception>
        public bool DeleteAll()
        {
            return DeleteAll(_tableName);
        }

        /// <summary>
        /// Helper private function for deleting all the data of the given table.
        /// </summary>
        /// <param name="tableName">The name of the table in the database.</param>
        /// <returns>Returns true if deleted at least 1 row from the database.</returns>
        /// <exception cref="Exception">If an error has occured while deleting from database.</exception>
        private bool DeleteAll(string tableName)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                string query = $"DELETE FROM {tableName}";
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = query + ";"
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Debug($"Deleted table {_tableName}.");
                }
                catch
                {
                    log.Error($"Error: failed to delete from {_tableName}.");
                    throw new Exception($"Error: failed to delete from {_tableName}.");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
            }
            return res > 0;
        }

        /// <summary>
        /// This method clears the data of the sqlite_sequence table in the database,
        /// which is responsible for providing the auto-increment identification numbers for the tables in the database.
        /// </summary>
        /// <returns>Returns true if deleted at least 1 row from the database.</returns>
        public bool DeleteSqliteSequence()
        {
            return DeleteAll(SQLITE_SEQUENCE);
        }

        /// <summary>
        /// This method updates a field in the table.
        /// </summary>
        /// <param name="keyValues">PRIMARY KEYS values of the DTO.</param>
        /// <param name="attributeName">The name of the column in the table we want to update.</param>
        /// <param name="attributeValue">The new value to the column.</param>
        /// <returns>Returns true if updated at least 1 row from the database.</returns>
        /// <exception cref="Exception">If SQL Exception ocourd.</exception>
        public bool Update(string[] keyValues, string attributeName, string attributeValue)
        {
            return Update(keyValues, new string[] { attributeName }, new string[] { attributeValue });
        }

        /// <summary>
        /// This method updates a few fields in the table.
        /// </summary>
        /// <param name="keyValues">PRIMARY KEYS values of the DTO</param>
        /// <param name="attributeNames">The name of columns in the table we want to update</param>
        /// <param name="attributeValues">The new values to the columns we want to update</param>
        /// <returns>Returns true if updated at least 1 row from the database.</returns>
        /// <exception cref="Exception">If SQL Exception ocourd.</exception>
        public bool Update(string[] keyValues, string[] attributeNames, string[] attributeValues)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                string quary = $"UPDATE {_tableName} SET ";
                for (int i = 0; i < attributeNames.Length; i++)
                {
                    quary += $"{attributeNames[i]}=@{attributeNames[i]} ";
                }
                quary += "WHERE ";
                for (int i = 0; i < keyValues.Length; i++)
                {
                    quary += $"{_primaryKeyNames[i]} = @{_primaryKeyNames[i]}";
                    if (i < keyValues.Length - 1)
                        quary += " and ";
                }
               
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = quary + ";"
                };
                try
                {

                    for (int i = 0; i < attributeNames.Length; i++)//Parameters to set part
                    {
                        command.Parameters.Add(new SQLiteParameter(attributeNames[i], attributeValues[i]));
                    }
                    for (int i = 0; i < keyValues.Length; i++)//Parameters to where part
                    {
                        command.Parameters.Add(new SQLiteParameter(_primaryKeyNames[i], keyValues[i]));
                    }
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Debug($"Updated table {_tableName}.");
                }
                catch
                {
                    log.Error($"Error: failed to update {_tableName}.");
                    throw new Exception($"Error: failed to update {_tableName}.");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }

    }
}
