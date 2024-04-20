using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ADM_Scada.Core.Models;
using Serilog;

namespace ADM_Scada.Core.Respo
{
    public class CustomerRepository : RepositoryBase, IDataRepository<CustomerModel>
    {
        public CustomerRepository() { }

        public async Task<List<CustomerModel>> GetAll()
        {
            try
            {
                string query = "SELECT * FROM [dbo].[customer]";
                
                DataTable dataTable = await ExecuteQueryAsync(query);
                return ConvertDataTableToList(dataTable);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while retrieving all customers.");
                throw new RepositoryException("An error occurred while retrieving all customers. Please try again later.", ex);
            }
        }

        public async Task<CustomerModel> GetByName(string custName)
        {
            try
            {
                string query = "SELECT * FROM [dbo].[customer] WHERE cust_name = @CustName";
                Dictionary<string, object> parameters = new Dictionary<string, object> { { "@CustName", custName } };
                DataTable dataTable = await ExecuteQueryAsync(query, parameters);
                return ConvertDataTableToSingleObject(dataTable);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while retrieving customer by name: {CustName}", custName);
                throw new RepositoryException($"An error occurred while retrieving customer '{custName}'. Please try again later.", ex);
            }
        }

        public async Task<int> Create(CustomerModel customer)
        {
            try
            {
                string query = @"INSERT INTO [dbo].[customer] (cust_code, cust_name, cust_avatar, cust_add, 
                                payment_term, email_address, fax_no, tel_no, mobile_no, created_date, created_by, updated_date, updated_by) 
                                 VALUES (@CustCode, @CustName, @CustAvatar, @CustAdd, @PaymentTerm, @EmailAddress, 
                                @FaxNo, @TelNo, @MobileNo, @CreatedDate, @CreatedBy, @UpdatedDate, @UpdatedBy)";

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@CustCode", customer.CustCode },
                    { "@CustName", customer.CustName },
                    { "@CustAvatar", customer.CustAvatar },
                    { "@CustAdd", customer.CustAdd },
                    { "@PaymentTerm", customer.PaymentTerm },
                    { "@EmailAddress", customer.EmailAddress },
                    { "@FaxNo", customer.FaxNo },
                    { "@TelNo", customer.TelNo },
                    { "@MobileNo", customer.MobileNo },
                    { "@CreatedDate", DateTime.Now },
                    { "@CreatedBy", customer.CreatedBy },
                    { "@UpdatedDate", DateTime.Now },
                    { "@UpdatedBy", customer.UpdatedBy }
                };
                return await ExecuteNonQueryAsync(query, parameters);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while creating customer: {CustName}", customer.CustName);
                throw new RepositoryException($"An error occurred while creating customer '{customer.CustName}'. Please try again later.", ex);
            }
        }

        public async Task<bool> Update(CustomerModel customer)
        {
            try
            {
                string query = @"UPDATE [dbo].[customer] 
                         SET cust_code = @CustCode, cust_name = @CustName, cust_avatar = @CustAvatar, 
                             cust_add = @CustAdd, payment_term = @PaymentTerm, email_address = @EmailAddress, 
                             fax_no = @FaxNo, tel_no = @TelNo, mobile_no = @MobileNo, 
                             created_date = @CreatedDate, created_by = @CreatedBy, updated_date = @UpdatedDate, 
                             updated_by = @UpdatedBy 
                         WHERE id = @Id";

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@Id", customer.Id },
                    { "@CustCode", customer.CustCode },
                    { "@CustName", customer.CustName },
                    { "@CustAvatar", customer.CustAvatar },
                    { "@CustAdd", customer.CustAdd },
                    { "@PaymentTerm", customer.PaymentTerm },
                    { "@EmailAddress", customer.EmailAddress },
                    { "@FaxNo", customer.FaxNo },
                    { "@TelNo", customer.TelNo },
                    { "@MobileNo", customer.MobileNo },
                    { "@CreatedDate", customer.CreatedDate },
                    { "@CreatedBy", customer.CreatedBy },
                    { "@UpdatedDate", DateTime.Now },
                    { "@UpdatedBy", customer.UpdatedBy }
                };
                return await ExecuteNonQueryAsync(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while updating customer with ID: {CustomerId}", customer.Id);
                throw new RepositoryException($"An error occurred while updating customer with ID '{customer.Id}'. Please try again later.", ex);
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                string query = "DELETE FROM [dbo].[customer] WHERE id = @Id";
                Dictionary<string, object> parameters = new Dictionary<string, object> { { "@Id", id } };
                return await ExecuteNonQueryAsync(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while deleting customer with ID: {CustomerId}", id);
                throw new RepositoryException($"An error occurred while deleting customer with ID '{id}'. Please try again later.", ex);
            }
        }

        public async Task<CustomerModel> GetById(int id)
        {
            try
            {
                string query = "SELECT * FROM [dbo].[customer] WHERE id = @Id";
                Dictionary<string, object> parameters = new Dictionary<string, object> { { "@Id", id } };
                DataTable dataTable = await ExecuteQueryAsync(query, parameters);
                return ConvertDataTableToSingleObject(dataTable);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while retrieving customer by ID: {CustomerId}", id);
                throw new RepositoryException($"An error occurred while retrieving customer with ID '{id}'. Please try again later.", ex);
            }
        }

        public async Task<List<CustomerModel>> GetFiltered(params Func<CustomerModel, bool>[] filters)
        {
            try
            {
                string whereClause = string.Join(" AND ", filters.Select(filter => $"({filter})"));
                string query = $"SELECT * FROM [dbo].[customer] WHERE {whereClause}";
                DataTable dataTable = await ExecuteQueryAsync(query);
                return ConvertDataTableToList(dataTable);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while retrieving filtered customers.");
                throw new RepositoryException("An error occurred while retrieving filtered customers. Please try again later.", ex);
            }
        }

        public async Task<int> Count()
        {
            try
            {
                string query = "SELECT COUNT(*) FROM [dbo].[customer]";
                object result = await ExecuteScalarAsync(query);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while counting customers.");
                throw new RepositoryException("An error occurred while counting customers. Please try again later.", ex);
            }
        }

        public async Task<bool> Exists(int id)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM [dbo].[customer] WHERE id = @Id";
                Dictionary<string, object> parameters = new Dictionary<string, object> { { "@Id", id } };
                int count = Convert.ToInt32(await ExecuteScalarAsync(query, parameters));
                return count > 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while checking if customer exists with ID: {CustomerId}", id);
                throw new RepositoryException($"An error occurred while checking if customer exists with ID '{id}'. Please try again later.", ex);
            }
        }

        private List<CustomerModel> ConvertDataTableToList(DataTable dataTable)
        {
            List<CustomerModel> customerList = new List<CustomerModel>();

            foreach (DataRow row in dataTable.Rows)
            {
                customerList.Add(ConvertDataRowToCustomerModel(row));
            }

            return customerList;
        }

        private CustomerModel ConvertDataRowToCustomerModel(DataRow row)
        {
            return new CustomerModel
            {
                Id = Convert.ToInt32(row["id"]),
                CustCode = Convert.ToString(row["cust_code"]),
                CustName = Convert.ToString(row["cust_name"]),
                CustAvatar = Convert.ToString(row["cust_avatar"]),
                CustAdd = Convert.ToString(row["cust_add"]),
                PaymentTerm = Convert.ToString(row["payment_term"]),
                EmailAddress = Convert.ToString(row["email_address"]),
                FaxNo = Convert.ToString(row["fax_no"]),
                TelNo = Convert.ToString(row["tel_no"]),
                MobileNo = Convert.ToString(row["mobile_no"]),
                CreatedDate = Convert.IsDBNull(row["created_date"]) ? (DateTime?)null : Convert.ToDateTime(row["created_date"]),
                CreatedBy = Convert.ToString(row["created_by"]),
                UpdatedDate = Convert.IsDBNull(row["updated_date"]) ? (DateTime?)null : Convert.ToDateTime(row["updated_date"]),
                UpdatedBy = Convert.ToString(row["updated_by"])
            };
        }

        private CustomerModel ConvertDataTableToSingleObject(DataTable dataTable)
        {
            if (dataTable.Rows.Count == 0)
            {
                return null;
            }

            DataRow row = dataTable.Rows[0];
            return ConvertDataRowToCustomerModel(row);
        }
    }
}