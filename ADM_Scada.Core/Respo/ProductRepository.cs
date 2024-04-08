using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ADM_Scada.Core.Models;
using Serilog;

namespace ADM_Scada.Core.Respo
{
    public class ProductRepository : RepositoryBase, IDataRepository<ProductModel>
    {
        public ProductRepository() { }

        public async Task<List<ProductModel>> GetAll()
        {
            try
            {
                string query = "SELECT * FROM [dbo].[product]";
                DataTable dataTable = await ExecuteQueryAsync(query);
                return ConvertDataTableToList(dataTable);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while retrieving all products.");
                throw new RepositoryException("An error occurred while retrieving all products. Please try again later.", ex);
            }
        }

        public async Task<ProductModel> GetByName(string prodName)
        {
            try
            {
                string query = "SELECT * FROM [dbo].[product] WHERE prod_fullname = @ProdName";
                Dictionary<string, object> parameters = new Dictionary<string, object> { { "@ProdName", prodName } };
                DataTable dataTable = await ExecuteQueryAsync(query, parameters);
                return ConvertDataTableToSingleObject(dataTable);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while retrieving product by name: {ProdName}", prodName);
                throw new RepositoryException($"An error occurred while retrieving product '{prodName}'. Please try again later.", ex);
            }
        }

        public async Task<int> Create(ProductModel product)
        {
            try
            {
                string query = @"INSERT INTO [dbo].[product] (prod_code, prod_fullname, hash_code, ingredient, exp, market, 
                                prod_name, label_path, barcode, delay_m4, delay_m5, pack_size, loose_uom, whole_uom, created_date, created_by, updated_date, updated_by) 
                                 VALUES (@ProdCode, @ProdFullName, @HashCode, @Ingredient, @Exp, @Market, @ProdName, 
                                @LabelPath, @Barcode, @DelayM4, @DelayM5, @PackSize, @LooseUom, @WholeUom, @CreatedDate, @CreatedBy, @UpdatedDate, @UpdatedBy)";

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@ProdCode", product.ProdCode },
                    { "@ProdFullName", product.ProdFullName },
                    { "@HashCode", product.HashCode },
                    { "@Ingredient", product.Ingredient },
                    { "@Exp", product.Exp },
                    { "@Market", product.Market },
                    { "@ProdName", product.ProdName },
                    { "@LabelPath", product.LabelPath },
                    { "@Barcode", product.Barcode },
                    { "@DelayM4", product.DelayM4 },
                    { "@DelayM5", product.DelayM5 },
                    { "@PackSize", product.PackSize },
                    { "@LooseUom", product.LooseUom },
                    { "@WholeUom", product.WholeUom },
                    { "@CreatedDate", product.CreatedDate },
                    { "@CreatedBy", product.CreatedBy },
                    { "@UpdatedDate", product.UpdatedDate },
                    { "@UpdatedBy", product.UpdatedBy }
                };
                return await ExecuteNonQueryAsync(query, parameters);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while creating product: {ProdName}", product.ProdName);
                throw new RepositoryException($"An error occurred while creating product '{product.ProdName}'. Please try again later.", ex);
            }
        }

        public async Task<bool> Update(ProductModel product)
        {
            try
            {
                string query = @"UPDATE [dbo].[product] 
                         SET prod_code = @ProdCode, prod_fullname = @ProdFullName, hash_code = @HashCode, 
                             ingredient = @Ingredient, exp = @Exp, market = @Market, prod_name = @ProdName, 
                             label_path = @LabelPath, barcode = @Barcode, delay_m4 = @DelayM4, delay_m5 = @DelayM5, 
                             pack_size = @PackSize, loose_uom = @LooseUom, whole_uom = @WholeUom, 
                             created_date = @CreatedDate, created_by = @CreatedBy, updated_date = @UpdatedDate, 
                             updated_by = @UpdatedBy 
                         WHERE id = @Id";

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@Id", product.Id },
                    { "@ProdCode", product.ProdCode },
                    { "@ProdFullName", product.ProdFullName },
                    { "@HashCode", product.HashCode },
                    { "@Ingredient", product.Ingredient },
                    { "@Exp", product.Exp },
                    { "@Market", product.Market },
                    { "@ProdName", product.ProdName },
                    { "@LabelPath", product.LabelPath },
                    { "@Barcode", product.Barcode },
                    { "@DelayM4", product.DelayM4 },
                    { "@DelayM5", product.DelayM5 },
                    { "@PackSize", product.PackSize },
                    { "@LooseUom", product.LooseUom },
                    { "@WholeUom", product.WholeUom },
                    { "@CreatedDate", product.CreatedDate },
                    { "@CreatedBy", product.CreatedBy },
                    { "@UpdatedDate", product.UpdatedDate },
                    { "@UpdatedBy", product.UpdatedBy }
                };
                return await ExecuteNonQueryAsync(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while updating product with ID: {ProductId}", product.Id);
                throw new RepositoryException($"An error occurred while updating product with ID '{product.Id}'. Please try again later.", ex);
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                string query = "DELETE FROM [dbo].[product] WHERE id = @Id";
                Dictionary<string, object> parameters = new Dictionary<string, object> { { "@Id", id } };
                return await ExecuteNonQueryAsync(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while deleting product with ID: {ProductId}", id);
                throw new RepositoryException($"An error occurred while deleting product with ID '{id}'. Please try again later.", ex);
            }
        }

        public async Task<ProductModel> GetById(int id)
        {
            try
            {
                string query = "SELECT * FROM [dbo].[product] WHERE id = @Id";
                Dictionary<string, object> parameters = new Dictionary<string, object> { { "@Id", id } };
                DataTable dataTable = await ExecuteQueryAsync(query, parameters);
                return ConvertDataTableToSingleObject(dataTable);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while retrieving product by ID: {ProductId}", id);
                throw new RepositoryException($"An error occurred while retrieving product with ID '{id}'. Please try again later.", ex);
            }
        }

        public async Task<List<ProductModel>> GetFiltered(params Func<ProductModel, bool>[] filters)
        {
            try
            {
                string whereClause = string.Join(" AND ", filters.Select(filter => $"({filter})"));
                string query = $"SELECT * FROM [dbo].[product] WHERE {whereClause}";
                DataTable dataTable = await ExecuteQueryAsync(query);
                return ConvertDataTableToList(dataTable);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while retrieving filtered products.");
                throw new RepositoryException("An error occurred while retrieving filtered products. Please try again later.", ex);
            }
        }

        public async Task<int> Count()
        {
            try
            {
                string query = "SELECT COUNT(*) FROM [dbo].[product]";
                object result = await ExecuteScalarAsync(query);
                return Convert.ToInt32(result);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while counting products.");
                throw new RepositoryException("An error occurred while counting products. Please try again later.", ex);
            }
        }

        public async Task<bool> Exists(int id)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM [dbo].[product] WHERE id = @Id";
                Dictionary<string, object> parameters = new Dictionary<string, object> { { "@Id", id } };
                int count = Convert.ToInt32(await ExecuteScalarAsync(query, parameters));
                return count > 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred while checking if product exists with ID: {ProductId}", id);
                throw new RepositoryException($"An error occurred while checking if product exists with ID '{id}'. Please try again later.", ex);
            }
        }

        private List<ProductModel> ConvertDataTableToList(DataTable dataTable)
        {
            List<ProductModel> productList = new List<ProductModel>();

            foreach (DataRow row in dataTable.Rows)
            {
                productList.Add(ConvertDataRowToProductModel(row));
            }

            return productList;
        }

        private ProductModel ConvertDataRowToProductModel(DataRow row)
        {
            return new ProductModel
            {
                Id = Convert.ToInt32(row["id"]),
                ProdCode = Convert.ToString(row["prod_code"]),
                ProdFullName = Convert.ToString(row["prod_fullname"]),
                HashCode = Convert.ToString(row["hash_code"]),
                Ingredient = Convert.ToString(row["ingredient"]),
                Exp = Convert.ToDecimal(row["exp"]),
                Market = Convert.ToString(row["market"]),
                ProdName = Convert.ToString(row["prod_name"]),
                LabelPath = Convert.ToString(row["label_path"]),
                Barcode = Convert.ToString(row["barcode"]),
                DelayM4 = Convert.ToString(row["delay_m4"]),
                DelayM5 = Convert.ToString(row["delay_m5"]),
                PackSize = Convert.ToDecimal(row["pack_size"]),
                LooseUom = Convert.ToString(row["loose_uom"]),
                WholeUom = Convert.ToString(row["whole_uom"]),
                CreatedDate = Convert.IsDBNull(row["created_date"]) ? (DateTime?)null : Convert.ToDateTime(row["created_date"]),
                CreatedBy = Convert.ToString(row["created_by"]),
                UpdatedDate = Convert.IsDBNull(row["updated_date"]) ? (DateTime?)null : Convert.ToDateTime(row["updated_date"]),
                UpdatedBy = Convert.ToString(row["updated_by"])
            };
        }

        private ProductModel ConvertDataTableToSingleObject(DataTable dataTable)
        {
            if (dataTable.Rows.Count == 0)
            {
                return null;
            }

            DataRow row = dataTable.Rows[0];
            return ConvertDataRowToProductModel(row);
        }
    }
}
