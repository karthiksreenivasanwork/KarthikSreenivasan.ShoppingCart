using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace ShoppingCart.API.SQLDataProvider
{
    internal class DatabaseFunctions<T> where T : class
    {
        internal (int, SqlCommand) executeNonQuery(string sqlConnectionString, string storedProcedureName, List<SqlParameter> sqlParameter)
        {
            int commandResult = 0;
            SqlCommand commandReference = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(sqlConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(storedProcedureName))
                    {
                        connection.Open();

                        command.Connection = connection;
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
                System.Diagnostics.Debug.WriteLine(string.Format("SQL Exception at: {0} with exception details  - {1}", typeof(T), sqlException));
            }
            catch (Exception exception)
            {
                //ToDo - Log this information
                System.Diagnostics.Debug.WriteLine(exception);
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
            }
            catch (Exception exception)
            {
                connection.Close();
                //ToDo - Log this information
                System.Diagnostics.Debug.WriteLine(exception);
            }

            return null;
        }
    }
}


