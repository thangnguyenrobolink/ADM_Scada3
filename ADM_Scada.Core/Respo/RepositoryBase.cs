using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows;

namespace ADM_Scada.Cores.Respo
{
    public abstract class RepositoryBase
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;

        protected RepositoryBase(string connectionString)
        {
            this.connectionString = connectionString;
        }

        protected RepositoryBase()
        {
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
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        await connection.OpenAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return null;
                    }

                    if (parameters != null)
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.AddWithValue(parameter.Key, parameter.Value ?? DBNull.Value);
                        }
                    }

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        try
                        {
                            await Task.Run(() => adapter.Fill(dataTable));
                        }
                        catch (Exception ex)
                        {
                            // Log or handle the exception
                            _ = MessageBox.Show(ex.Message);
                            return null;
                        }
                    }
                }
            }
            return dataTable;
        }
    }
}
