using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace shoppingCart.Models.Product
{
    public class ProductService
    {
        /// <summary>
        /// 連線字串
        /// </summary>
        readonly private string connectionStr = new CommonTool().GetConnectionString();

        /// <summary>
        /// 取得所有商品
        /// </summary>
        /// <returns></returns>
        public List<Product> GetAllProduct()
        {
            DataTable productDt = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                connection.Open();

                string sql = @"SELECT *
                               FROM Product";

                SqlCommand cmd = new SqlCommand(sql, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(productDt);

                connection.Close();
            }

            return this.MapProduct(productDt);
        }

        /// <summary>
        /// 將各筆資料塞進物件再塞進List裡
        /// </summary>
        /// <param name="productDt">存放查詢結果的DataTable</param>
        /// <returns></returns>
        private List<Product> MapProduct(DataTable productDt)
        {
            List<Product> model = new List<Product>();

            foreach (DataRow row in productDt.Rows)
            {
                model.Add(new Product
                {
                    ProductId = row["ProductId"].ToString(),
                    ProductName = row["ProductName"].ToString(),
                    ProductPrice = (int)row["ProductPrice"],
                    ProductInStock = (int)row["ProductInStock"]
                });
            }

            return model;
        }

        /// <summary>
        /// 新增資料進DB
        /// </summary>
        /// <param name="newProductData">新增的資料</param>
        public void AddProduct(Product newProductData)
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                connection.Open();

                string sql = @"INSERT INTO Product (ProductName, ProductPrice, ProductInStock)
                               VALUES (@ProductName, @ProductCost, @ProductInStock)";

                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.Add(new SqlParameter("@ProductName", newProductData.ProductName));
                cmd.Parameters.Add(new SqlParameter("@ProductCost", newProductData.ProductPrice));
                cmd.Parameters.Add(new SqlParameter("@ProductInStock", newProductData.ProductInStock));

                cmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        /// <summary>
        /// 從DB裡刪除資料
        /// </summary>
        /// <param name="deletedProductId">作為刪除依據的商品名稱</param>
        public void DeleteProduct(string deletedProductId)
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                connection.Open();

                string sql = @"DELETE FROM Product
                               WHERE ProductId = @DeletedProductId";

                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.Add(new SqlParameter("@DeletedProductId", deletedProductId));
                cmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        /// <summary>
        /// 查詢更新商品資料
        /// </summary>
        /// <param name="updatedProductId">作為查詢更新資料依據的商品名稱</param>
        /// <returns></returns>
        public Product GetUpdateProductData(string updatedProductId)
        {
            DataTable productDt = new DataTable();
            Product updatedProductData = new Product();

            using(SqlConnection connection = new SqlConnection(connectionStr))
            {
                connection.Open();

                string sql = @"SELECT *
                               FROM Product
                               WHERE ProductId = @UpdatedProductId";

                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.Add(new SqlParameter("@UpdatedProductId", updatedProductId));

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(productDt);

                connection.Close();
            }

            foreach(DataRow row in productDt.Rows)
            {
                updatedProductData.ProductId = updatedProductId;
                updatedProductData.ProductName = row["ProductName"].ToString();
                updatedProductData.ProductPrice = (int)row["ProductPrice"];
                updatedProductData.ProductInStock = (int)row["ProductInStock"];
            }

            return updatedProductData;
        }

        /// <summary>
        /// 更新商品資訊
        /// </summary>
        /// <param name="updatedProductData">要更新的商品資訊</param>
        public void UpdateProductData(Product updatedProductData)
        {
            using(SqlConnection connection = new SqlConnection(connectionStr))
            {
                connection.Open();

                string sql = @"UPDATE Product
                               SET
                                   ProductPrice = @ProductCost,
                                   ProductInStock = @ProductInStock
                               WHERE ProductId = @ProductId";

                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.Add(new SqlParameter("@ProductId", updatedProductData.ProductId));
                cmd.Parameters.Add(new SqlParameter("@ProductCost", updatedProductData.ProductPrice));
                cmd.Parameters.Add(new SqlParameter("@ProductInStock", updatedProductData.ProductInStock));
                cmd.ExecuteNonQuery();

                connection.Close();
            }
        }
    }
}