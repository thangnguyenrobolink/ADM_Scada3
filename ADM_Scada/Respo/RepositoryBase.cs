using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ADM_Scada.Respo
{
    public abstract class RepositoryBase
    {
        private readonly string connectionString = @"Data Source=.\sqlexpress;Initial Catalog=ScadarADM;Integrated Security=True";

        protected RepositoryBase(string connectionString)
        {
            this.connectionString = connectionString;
        }

        protected async Task<int> ExecuteNonQueryAsync(string query, Dictionary<string, object> parameters = null)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                await connection.OpenAsync();

                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.AddWithValue(parameter.Key, parameter.Value ?? DBNull.Value);
                    }
                }

                return await command.ExecuteNonQueryAsync();
            }
        }

        protected async Task<object> ExecuteScalarAsync(string query, Dictionary<string, object> parameters = null)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                await connection.OpenAsync();

                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.AddWithValue(parameter.Key, parameter.Value ?? DBNull.Value);
                    }
                }

                return await command.ExecuteScalarAsync();
            }
        }

        protected async Task<DataTable> ExecuteQueryAsync(string query, Dictionary<string, object> parameters = null)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                await connection.OpenAsync();

                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.AddWithValue(parameter.Key, parameter.Value ?? DBNull.Value);
                    }
                }

                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    DataTable dataTable = new DataTable();
                    await Task.Run(() => adapter.Fill(dataTable));
                    return dataTable;
                }
            }
        }
    }
}
