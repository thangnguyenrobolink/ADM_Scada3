using ADM_Scada.Cores.Models;
using ADM_Scada.Cores.Respo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ADM_Scada.Modules.Product.Repositories
{
    public class ProductRepository : RepositoryBase, IDataRepository<ProductModel>
    {
        public ProductRepository() : base() { }

        public async Task<List<ProductModel>> GetAll()
        {
            string query = "SELECT * FROM product;";
            DataTable dataTable = await ExecuteQueryAsync(query);
            return ConvertDataTableToList(dataTable);
        }

        public async Task<ProductModel> GetById(int ProductId)
        {
            string query = "SELECT * FROM product WHERE Id = @ProductId;";
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@ProductId", ProductId } };
            DataTable dataTable = await ExecuteQueryAsync(query, parameters);
            return ConvertDataTableToSingleObject(dataTable);
        }

        public async Task<int> Create(ProductModel Product)
        {
            string query = "INSERT INTO product (Name, Code, Description, Market, Exp, Ingredient) " +
                           "VALUES (@Name, @Code, @Description, @Market, @Exp, @Ingredient);";
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@Name", Product.Name },
                { "@Code", Product.Code },
                { "@Description", Product.Description },
                { "@Market", Product.Market },
                { "@Exp", Product.Exp },
                { "@Ingredient", Product.Ingredient }
            };
            return await ExecuteNonQueryAsync(query, parameters);
        }

        public async Task<bool> Update(ProductModel Product)
        {
            string query = "UPDATE product SET Name = @Name, Description = @Description, " +
                           "Code = @Code, Market = @Market, Ingredient = @Ingredient, Exp = @Exp" +
                           " WHERE Id = @ProductId;";
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@ProductId", Product.Id },
                { "@Description", Product.Description },
                { "@Code", Product.Code },
                { "@Market", Product.Market },
                { "@Ingredient", Product.Ingredient },
                { "@Exp", Product.Exp },
                { "@Name", Product.Name }
            };
            return await ExecuteNonQueryAsync(query, parameters) > 0;
        }

        public async Task<bool> Delete(int ProductId)
        {
            string query = "DELETE FROM product WHERE Id = @ProductId;";
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@ProductId", ProductId } };
            return await ExecuteNonQueryAsync(query, parameters) > 0;
        }

        public async Task<ProductModel> Get(int ProductId)
        {
            string query = "SELECT * FROM product WHERE Id = @ProductId;";
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "@ProductId", ProductId } };
            DataTable dataTable = await ExecuteQueryAsync(query, parameters);
            return ConvertDataTableToSingleObject(dataTable);
        }

        // Helper methods to convert DataTable to a list of objects
        private List<ProductModel> ConvertDataTableToList(DataTable dataTable)
        {
            List<ProductModel> ProductList = new List<ProductModel>();

            foreach (DataRow row in dataTable.Rows)
            {
                ProductModel Product = new ProductModel
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Name = Convert.ToString(row["Name"]),
                    Description = Convert.ToString(row["Description"]),
                    Code = Convert.ToString(row["Code"]),
                    Market = Convert.ToString(row["Market"]),
                    Ingredient = Convert.ToString(row["Ingredient"]),
                    Exp = Convert.ToInt32(row["Exp"])
                };

                ProductList.Add(Product);
            }

            return ProductList;
        }

        private ProductModel ConvertDataTableToSingleObject(DataTable dataTable)
        {
            if (dataTable.Rows.Count == 0)
            {
                return null;
            }

            DataRow row = dataTable.Rows[0];

            ProductModel Product = new ProductModel
            {
                Id = Convert.ToInt32(row["Id"]),
                Name = Convert.ToString(row["Name"]),
                Description = Convert.ToString(row["Description"]),
                Code = Convert.ToString(row["Code"]),
                Market = Convert.ToString(row["Market"]),
                Ingredient = Convert.ToString(row["Ingredient"]),
                Exp = Convert.ToInt32(row["Exp"])
            };

            return Product;
        }

        public Task<ProductModel> GetByName(string s)
        {
            throw new NotImplementedException();
        }
    }
}
