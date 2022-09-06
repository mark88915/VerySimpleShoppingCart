using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace shoppingCart.Models.Client
{
    public class UserProductService
    {
        readonly private string connectionStr = new CommonTool().GetConnectionString();

        /// <summary>
        /// 將商品加入購物車
        /// </summary>
        /// <param name="userId">使用者Id</param>
        /// <param name="productId">商品Id</param>
        public void AddProductToShoppingCart(string userId, string productId)
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                connection.Open();

                string sql = @"INSERT INTO UserShoppingCart (UserId, ProductId)
                               VALUES (@UserId, @ProductId)";

                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.Add(new SqlParameter("@UserId", userId));
                cmd.Parameters.Add(new SqlParameter("@ProductId", productId));
                cmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        /// <summary>
        /// 確認商品是否存在該使用者的購物車
        /// </summary>
        /// <param name="userId">使用者Id</param>
        /// <param name="productId">商品Id</param>
        /// <returns></returns>
        public bool IsThisProductInThisUserShoppingCart(string userId, string productId)
        {
            DataTable shoppingCartDt = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                connection.Open();

                string sql = @"SELECT *
                               FROM UserShoppingCart
                               WHERE UserId = @UserId AND ProductId = @ProductId";

                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.Add(new SqlParameter("@UserId", userId));
                cmd.Parameters.Add(new SqlParameter("@ProductId", productId));

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(shoppingCartDt);

                connection.Close();
            }

            return shoppingCartDt.Rows.Count != 0;
        }

        /// <summary>
        /// 取得該使用者的購物車內容
        /// </summary>
        /// <param name="userId">使用者Id</param>
        /// <returns></returns>
        public List<Models.Product.Product> GetUserShoppingCart(string userId)
        {
            DataTable shoppingCartDt = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                connection.Open();

                string sql = @"SELECT *
                               FROM UserShoppingCart Cart
                               INNER JOIN Product Pd 
                                ON Cart.ProductId = Pd.ProductId 
                               WHERE UserId = @UserId";

                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.Add(new SqlParameter("@UserId", userId));

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(shoppingCartDt);

                connection.Close();
            }

            return this.MapUserShoppingCart(shoppingCartDt);
        }

        /// <summary>
        /// 把使用者購物車的資料從DataTable中Map進List裡
        /// </summary>
        /// <param name="shoppingCartDt"></param>
        /// <returns></returns>
        private List<Models.Product.Product> MapUserShoppingCart(DataTable shoppingCartDt)
        {
            List<Models.Product.Product> shoppingCartList = new List<Product.Product>();

            foreach (DataRow row in shoppingCartDt.Rows)
            {
                shoppingCartList.Add(new Product.Product
                {
                    ProductId = row["ProductId"].ToString(),
                    ProductName = row["ProductName"].ToString(),
                    ProductPrice = (int)row["ProductPrice"]
                });
            }

            return shoppingCartList;
        }

        /// <summary>
        /// 將商品移出購物車
        /// </summary>
        /// <param name="userId">使用者Id</param>
        /// <param name="productId">產品Id</param>
        public void RemoveFromShoppingCart(string userId, string productId)
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                connection.Open();

                string sql = @"DELETE FROM UserShoppingCart
                               WHERE UserId = @UserId AND ProductId = @ProductId";

                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.Add(new SqlParameter("@UserId", userId));
                cmd.Parameters.Add(new SqlParameter("@ProductId", productId));

                cmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        /// <summary>
        /// 取得要購買的商品資訊
        /// </summary>
        /// <param name="productId">商品Id</param>
        /// <returns></returns>
        public UserPurchase GetPurchasedProductData(string productId)
        {
            DataTable purchasedProductDt = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                connection.Open();

                string sql = @"SELECT *
                               FROM Product
                               WHERE ProductId = @ProductId";

                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.Add(new SqlParameter("@ProductId", productId));

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(purchasedProductDt);

                connection.Close();
            }

            return this.MapPurchasedProductData(purchasedProductDt);
        }

        /// <summary>
        /// 把被購買的商品資料的資料從DataTable中Map進List裡
        /// </summary>
        /// <param name="purchasedProductDt"></param>
        /// <returns></returns>
        private UserPurchase MapPurchasedProductData(DataTable purchasedProductDt)
        {
            UserPurchase purchasedProductData = new UserPurchase();
            foreach (DataRow row in purchasedProductDt.Rows)
            {
                purchasedProductData.ProductId = row["ProductId"].ToString();
                purchasedProductData.ProductName = row["ProductName"].ToString();
                purchasedProductData.ProductPrice = (int)row["ProductPrice"];
                purchasedProductData.ProductInStock = (int)row["ProductInStock"];
            }

            return purchasedProductData;
        }

        /// <summary>
        /// 購買
        /// </summary>
        /// <param name="purchasedData">被購買的商品資料</param>
        public void Purchase(UserPurchase purchasedData)
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                connection.Open();

                string sql = @"UPDATE Product
                               SET
                                    ProductInStock = @ProductInStock - @PurchasedAmount
                               WHERE ProductId = @ProductId";

                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.Add(new SqlParameter("@ProductInStock", purchasedData.ProductInStock));
                cmd.Parameters.Add(new SqlParameter("@PurchasedAmount", purchasedData.PurchasedAmount));
                cmd.Parameters.Add(new SqlParameter("@ProductId", purchasedData.ProductId));

                cmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        /// <summary>
        /// 新增購買歷史
        /// </summary>
        /// <param name="purchasedData">購買商品資訊</param>
        /// <param name="userId">使用者Id</param>
        public void AddPurchaseHistory(UserPurchase purchasedData, string userId)
        {
            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                connection.Open();

                string sql = @"INSERT INTO PurchaseHistory (UserId, ProductId, PurchasedAmount, PurchaseDate)
                               VALUES (@UserId, @ProductId, @PurchasedAmount, @PurchaseDate)";

                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.Add(new SqlParameter("@UserId", userId));
                cmd.Parameters.Add(new SqlParameter("@ProductId", purchasedData.ProductId));
                cmd.Parameters.Add(new SqlParameter("@PurchasedAmount", purchasedData.PurchasedAmount));
                cmd.Parameters.Add(new SqlParameter("@PurchaseDate", DateTime.Now));

                cmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        /// <summary>
        /// 取得使用者之購買歷史
        /// </summary>
        /// <param name="userId">使用者Id</param>
        /// <returns></returns>
        public List<PurchaseHistory> GetUserPurchaseHistory(string userId)
        {
            DataTable PurchaseHistoryDt = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                connection.Open();

                string sql = @"SELECT ph.UserId UserId, ph.PurchasedAmount PurchasedAmount, ph.PurchaseDate PurchaseDate, pd.ProductName ProductName
                               FROM PurchaseHistory ph
                               INNER JOIN Product pd
                                   ON ph.ProductId = pd.ProductId
                               WHERE ph.UserId = @UserId";

                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.Add(new SqlParameter("@UserId", userId));

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(PurchaseHistoryDt);

                connection.Close();
            }

            return this.MapUserPurchaseHistory(PurchaseHistoryDt);
        }

        /// <summary>
        /// 把使用者購買歷史的資料從DataTable中Map進List裡
        /// </summary>
        /// <param name="PurchaseHistoryDt"></param>
        /// <returns></returns>
        private List<PurchaseHistory> MapUserPurchaseHistory(DataTable PurchaseHistoryDt)
        {
            List<PurchaseHistory> purchaseHistoryList = new List<PurchaseHistory>();

            foreach(DataRow row in PurchaseHistoryDt.Rows)
            {
                purchaseHistoryList.Add(new PurchaseHistory
                {
                    ProductName = row["ProductName"].ToString(),
                    PurchasedAmount = (int)row["PurchasedAmount"],
                    PurchaseDate = (DateTime)row["PurchaseDate"]
                });
            }

            return purchaseHistoryList;
        }
    }
}