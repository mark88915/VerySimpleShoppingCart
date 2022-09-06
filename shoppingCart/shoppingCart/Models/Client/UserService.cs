using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace shoppingCart.Models.Client
{
    public class UserService
    {
        readonly private string connectionStr = new CommonTool().GetConnectionString();

        /// <summary>
        /// 新增使用者
        /// </summary>
        /// <param name="userData">要新增的資料</param>
        public void AddUser(User userData)
        {
            using(SqlConnection connection = new SqlConnection(connectionStr))
            {
                connection.Open();
                string sql = @"INSERT INTO UserInfo (UserId, Password)
                               VALUES (@UserId, @Password)";

                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.Parameters.Add(new SqlParameter("@UserId", userData.UserId));
                cmd.Parameters.Add(new SqlParameter("@Password", userData.Password));
                cmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        /// <summary>
        /// 取得所有使用者資料
        /// </summary>
        /// <returns></returns>
        public List<User> GetAllUser()
        {
            DataTable accountDt = new DataTable();

            using(SqlConnection connection = new SqlConnection(connectionStr))
            {
                connection.Open();

                string sql = @"SELECT *
                               FROM UserInfo";

                SqlCommand cmd = new SqlCommand(sql, connection);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(accountDt);

                connection.Close();
            }

            return this.MapUser(accountDt);
        }

        /// <summary>
        /// 把使用者的資料從DataTable中Map進List裡
        /// </summary>
        /// <param name="accountDt"></param>
        /// <returns></returns>
        private List<User> MapUser(DataTable accountDt)
        {
            List<User> userList = new List<User>();

            foreach (DataRow row in accountDt.Rows)
            {
                userList.Add(new User
                {
                    UserId = row["UserId"].ToString(),
                    Password = row["Password"].ToString()
                });
            }

            return userList;
        }

        /// <summary>
        /// 檢查帳戶是否存在
        /// </summary>
        /// <param name="userList">帳戶清單</param>
        /// <param name="userData">使用者資訊</param>
        /// <returns></returns>
        public bool IsAccountExist(List<User> userList, User userData)
        {
            return userList.Exists(user => user.UserId.Equals(userData.UserId) && user.Password.Equals(userData.Password));
        }

    }
}