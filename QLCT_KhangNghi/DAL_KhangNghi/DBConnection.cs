using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_KhangNghi
{
    public class DBConnection
    {
        protected SqlConnection conn;

        public DBConnection()
        {
            string connectionString = @"Data Source=LAPTOP-7V4UTMER\SQLEXPRESS;Initial Catalog=KhangNghiDB1;Integrated Security=True";
            conn = new SqlConnection(connectionString);
        }
    }
}
