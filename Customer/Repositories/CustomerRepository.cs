using ADM_Scada.Cores.Model;
using ADM_Scada.Cores.Respo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADM_Scada.Modules.Customer.Repositories
{
    public class CustomerRepository : RepositoryBase, IDataRepository<CustomerModel>
    {
        public CustomerRepository() : base() { }

        public async Task<List<CustomerModel>> GetAll()
        {
            string query = "SELECT * FROM customer;";
            DataTable dataTable = await ExecuteQueryAsync(query);
            return ConvertDataTableToList(dataTable);
        }

        public async Task<CustomerModel> GetById(int customerId)
        {
            string query = "SELECT * FROM customer WHERE Id = @CustomerId;";
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@CustomerId", customerId } };
            DataTable dataTable = await ExecuteQueryAsync(query, parameters);
            return ConvertDataTableToSingleObject(dataTable);
        }

        public async Task<int> Create(CustomerModel customer)
        {
            string query = "INSERT INTO customer (Name, Email, Phone, Address, Company, Code, Avatar) " +
                           "VALUES (@Name, @Email, @Phone, @Address, @Company, @Code, @Avatar);";
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@Name", customer.Name },
                { "@Email", customer.Email },
                { "@Phone", customer.Phone },
                { "@Address", customer.Address },
                { "@Company", customer.Company },
                { "@Code", customer.Code },
                { "@Avatar", customer.Avatar }
            };
            return await ExecuteNonQueryAsync(query, parameters);
        }

        public async Task<bool> Update(CustomerModel customer)
        {
            string query = "UPDATE customer SET Name = @Name, Email = @Email, " +
                           "Phone = @Phone, Address = @Address, Company = @Company, Code = @Code, Avatar = @Avatar" +
                           " WHERE Id = @CustomerId;";
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@CustomerId", customer.Id },
                { "@Name", customer.Name },
                { "@Email", customer.Email },
                { "@Phone", customer.Phone },
                { "@Address", customer.Address },
                { "@Company", customer.Company },
                { "@Code", customer.Code },
                { "@Avatar", customer.Avatar }
            };
            return await ExecuteNonQueryAsync(query, parameters) > 0;
        }

        public async Task<bool> Delete(int customerId)
        {
            string query = "DELETE FROM customer WHERE Id = @CustomerId;";
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@CustomerId", customerId } };
            return await ExecuteNonQueryAsync(query, parameters) > 0;
        }

        public async Task<CustomerModel> Get(int customerId)
        {
            string query = "SELECT * FROM customer WHERE Id = @CustomerId;";
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@CustomerId", customerId } };
            DataTable dataTable = await ExecuteQueryAsync(query, parameters);
            return ConvertDataTableToSingleObject(dataTable);
        }

        // Helper methods to convert DataTable to a list of objects
        private List<CustomerModel> ConvertDataTableToList(DataTable dataTable)
        {
            List<CustomerModel> customerList = new List<CustomerModel>();

            foreach (DataRow row in dataTable.Rows)
            {
                CustomerModel customer = new CustomerModel
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Name = Convert.ToString(row["Name"]),
                    Email = Convert.ToString(row["Email"]),
                    Phone = Convert.ToString(row["Phone"]),
                    Address = Convert.ToString(row["Address"]),
                    Company = Convert.ToString(row["Company"]),
                    Code = Convert.ToString(row["Code"]),
                    Avatar = Convert.ToString(row["Avatar"])
                };

                customerList.Add(customer);
            }

            return customerList;
        }

        private CustomerModel ConvertDataTableToSingleObject(DataTable dataTable)
        {
            if (dataTable.Rows.Count == 0)
            {
                return null;
            }

            DataRow row = dataTable.Rows[0];

            CustomerModel customer = new CustomerModel
            {
                Id = Convert.ToInt32(row["Id"]),
                Name = Convert.ToString(row["Name"]),
                Company = Convert.ToString(row["Company"]),
                Email = Convert.ToString(row["Email"]),
                Phone = Convert.ToString(row["Phone"]),
                Address = Convert.ToString(row["Address"]),
                Code = Convert.ToString(row["Code"]),
                Avatar = Convert.ToString(row["Avatar"])
            };

            return customer;
        }

        public Task<CustomerModel> GetByName(string s)
        {
            throw new NotImplementedException();
        }
    }
}
