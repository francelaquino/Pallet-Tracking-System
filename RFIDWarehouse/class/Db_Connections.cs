//Programmer : Francel D. Aquino
//Email : francel_aquino@yahoo.com
//Comment : gAwAnG pInOy!!!
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;using System.Data.SqlClient;
namespace Db_Connections
{
    class Db_Connection
    {
        static string strConnection = @"Data Source=DESKTOP-FBLQOP0\TRACKLINE;Initial Catalog=rfidwarehouse;User Id=sa;Password=sqlserver";
        //static string strConnection = @"Data Source=HOS-ITD-APP-02\DBINSTANCE;Initial Catalog=rfidwarehouse;User Id=sa;Password=SqlServer17";
        public SqlConnection DbConn = new SqlConnection(strConnection);

        //Open Database Connection
        public void openConnection()
        {
            if (DbConn.State == ConnectionState.Closed)
            {
                DbConn.Open();
            }
        }

        //Close Database Connection
        public void closeConnection()
        {
            if (DbConn.State == ConnectionState.Open)
            {
                DbConn.Close();
                DbConn.Dispose();
            }
        }
        public string errors(string errmsg)
        {
            if (errmsg.Contains("12154") == true)
            {
                errmsg = "Database connection error...";

            }
            return errmsg;
        }
    }
}
