using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace ShoppingCart.API.SQLDataProvider
{
    /// <summary>
    /// Enum that indicates a boolean search result from the database.
    /// </summary>
    internal enum DatabaseSearchResult //Greatly improve the readability of the code when the corresponding values are fetched from the database.
    {
        [Description("Indicates that the data search in the database was unsuccessful")]
        RecordDoesNotExists = 0,
        [Description("Indicates that the data search in the database was successful")]
        RecordExists = 1
    }

    internal class DatabaseFunctions
    {
        internal (int, SqlCommand) executeNonQuery(string sqlConnectionString, string storedProcedureName, List<SqlParameter> sqlParameter)
        {
            int commandResult = 0;
            SqlCommand commandReference = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                    {
                        connection.Open();

                        command.CommandType = CommandType.StoredProcedure;

                        foreach (SqlParameter parameter in sqlParameter)
                            command.Parameters.Add(parameter);

                        commandResult = command.ExecuteNonQuery();
                        commandReference = command;
                    }
                }
            }
            catch (SqlException sqlException)
            {
                //ToDo - Log this information
                System.Diagnostics.Debug.WriteLine(string.Format("SQL Exception at: {0} with exception details  - {1}", this.GetType(), sqlException));
                throw sqlException;
            }
            catch (Exception exception)
            {
                //ToDo - Log this information
                System.Diagnostics.Debug.WriteLine(exception);
                throw exception;
            }

            return (commandResult, commandReference);
        }

        public SqlDataReader executeReader(string sqlConnectionString, string storedProcedureName)
        {
            SqlConnection connection = new SqlConnection(sqlConnectionString);

            try
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    return command.ExecuteReader(CommandBehavior.CloseConnection);
                }
            }
            catch (SqlException sqlException)
            {
                connection.Close();
                //ToDo - Log this information
                System.Diagnostics.Debug.WriteLine(string.Format("SQL Exception at: {0} with exception details  - {1}", this.GetType(), sqlException));
                throw sqlException;
            }
            catch (Exception exception)
            {
                connection.Close();
                //ToDo - Log this information
                System.Diagnostics.Debug.WriteLine(exception);
                throw exception;
            }

            return null;
        }
    }
}


