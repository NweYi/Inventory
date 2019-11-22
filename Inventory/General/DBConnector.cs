using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace Inventory.General
{
    public class DBConnector
    {
        public static SqlConnection Connect()
        {
            try
            {
                SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
                if (con.State == ConnectionState.Closed)
                    con.Open();
                return con;
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return null;
        }
    }
}