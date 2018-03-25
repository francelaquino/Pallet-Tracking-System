using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Db_Connections;
using System.Data;
using System.Data.SqlClient;
using RFIDWarehouse.Areas.en.Models;

namespace RFIDWarehouse.Areas.en.Controllers
{
    public class employeeController : Controller
    {
        //
        // GET: /en/employee/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult newEmployee()
        {
            return View();
        }

        [HttpPost]
        public string saveEmployee(mdlEmployee employeeinfo)
        {

            string result = "";

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT ID FROM EMPLOYEES WHERE EMPLOYEENO=@EMPLOYEENO";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("EMPLOYEENO", SqlDbType.VarChar).Value = employeeinfo.EMPLOYEENO;

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    result = "Employee no. already exist";
                }
                else
                {
                    dr.Dispose();
                    cmd.Dispose();

                    strSQL = @"INSERT INTO EMPLOYEES(EMPLOYEENO,NAME,EMAILADDRESS,MOBILENO,ADDRESS,ENCODEDBY,MODIFIEDBY,DATEMODIFIED,DATEENCODED,ACTIVE)
                    VALUES(@EMPLOYEENO,@NAME,@EMAILADDRESS,@MOBILENO,@ADDRESS,@ENCODEDBY,@MODIFIEDBY,GETDATE(),GETDATE(),@ACTIVE)";




                    cmd = new SqlCommand(strSQL, dbconn.DbConn);
                    cmd.Parameters.Add("EMPLOYEENO", SqlDbType.VarChar).Value = employeeinfo.EMPLOYEENO;
                    cmd.Parameters.Add("NAME", SqlDbType.VarChar).Value = employeeinfo.NAME;
                    cmd.Parameters.Add("EMAILADDRESS", SqlDbType.VarChar).Value = employeeinfo.EMAILADDRESS;
                    cmd.Parameters.Add("MOBILENO", SqlDbType.VarChar).Value = employeeinfo.MOBILENO;
                    cmd.Parameters.Add("ADDRESS", SqlDbType.VarChar).Value = employeeinfo.ADDRESS;
                    cmd.Parameters.Add("ENCODEDBY", SqlDbType.VarChar).Value = "currentuser";
                    cmd.Parameters.Add("MODIFIEDBY", SqlDbType.VarChar).Value = "currentuser";
                    cmd.Parameters.Add("ACTIVE", SqlDbType.VarChar).Value = employeeinfo.ACTIVE;
                    cmd.ExecuteNonQuery();

                    result = "";
                }
                dr.Dispose();
                cmd.Dispose();


                dbconn.closeConnection();
                return result;

            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            finally
            {
                dbconn.closeConnection();
            }




        }

        public ActionResult Employee()
        {
            return View();

        }

        [HttpGet]
        public ActionResult getEmployees()
        {

            List<mdlEmployee> model = new List<mdlEmployee>();

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT ID,EMPLOYEENO,NAME,MOBILENO, CASE WHEN ACTIVE='A' THEN 'Active' else 'Inactive' end as ACTIVE FROM EMPLOYEES ";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);

                SqlDataReader dr = cmd.ExecuteReader();
                mdlEmployee list = new mdlEmployee();
                while (dr.Read())
                {
                    list = new mdlEmployee();
                    list.ID = dr["ID"].ToString();
                    list.EMPLOYEENO = dr["EMPLOYEENO"].ToString();
                    list.NAME = dr["NAME"].ToString();
                    list.MOBILENO = dr["MOBILENO"].ToString();
                    list.ACTIVE = dr["ACTIVE"].ToString();
                    model.Add(list);
                }
                dr.Dispose();
                cmd.Dispose();


                dbconn.closeConnection();
                return Json(model, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                dbconn.closeConnection();
            }




        }


        [HttpGet]
        public ActionResult getActiveEmployees()
        {

            List<mdlEmployee> model = new List<mdlEmployee>();

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT ID,EMPLOYEENO,NAME FROM EMPLOYEES WHERE ACTIVE='A'";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);

                SqlDataReader dr = cmd.ExecuteReader();
                mdlEmployee list = new mdlEmployee();
                while (dr.Read())
                {
                    list = new mdlEmployee();
                    list.ID = dr["ID"].ToString();
                    list.NAME = dr["EMPLOYEENO"].ToString()+" - "+dr["NAME"].ToString();
                    model.Add(list);
                }
                dr.Dispose();
                cmd.Dispose();


                dbconn.closeConnection();
                return Json(model, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                dbconn.closeConnection();
            }




        }

        public ActionResult editEmployee()
        {
            return View();
        }

        [HttpGet]
        public ActionResult getEmployeeInfo(string id)
        {
            mdlEmployee model = new mdlEmployee();

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT * FROM EMPLOYEES WHERE ID=@ID";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = id;

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    dr.Read();
                    model.ID = dr["ID"].ToString();
                    model.EMPLOYEENO = dr["EMPLOYEENO"].ToString();
                    model.NAME = dr["NAME"].ToString();
                    model.EMAILADDRESS = dr["EMAILADDRESS"].ToString();
                    model.MOBILENO = dr["MOBILENO"].ToString();
                    model.ADDRESS = dr["ADDRESS"].ToString();
                    model.ACTIVE = dr["ACTIVE"].ToString();
                }
                dr.Dispose();
                cmd.Dispose();


                dbconn.closeConnection();
                return Json(model, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                dbconn.closeConnection();
            }




        }

        [HttpPost]
        public string updateEmployee(mdlEmployee employeeinfo)
        {

            string result = "";

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT ID FROM EMPLOYEES WHERE EMPLOYEENO=@EMPLOYEENO";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("EMPLOYEENO", SqlDbType.VarChar).Value = employeeinfo.EMPLOYEENO;

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    if (dr["ID"].ToString() != employeeinfo.ID)
                    {
                        result = "Employee no. already exist";
                    }

                }

                dr.Dispose();
                cmd.Dispose();

                if (result == "")
                {


                    strSQL = @"UPDATE EMPLOYEES SET EMPLOYEENO=@EMPLOYEENO,NAME=@NAME,EMAILADDRESS=@EMAILADDRESS,MOBILENO=@MOBILENO,
                    ADDRESS=@ADDRESS,ACTIVE=@ACTIVE,
                    MODIFIEDBY=@MODIFIEDBY,DATEMODIFIED=GETDATE() WHERE ID=@ID";

                    cmd = new SqlCommand(strSQL, dbconn.DbConn);
                    cmd.Parameters.Add("EMPLOYEENO", SqlDbType.VarChar).Value = employeeinfo.EMPLOYEENO;
                    cmd.Parameters.Add("NAME", SqlDbType.VarChar).Value = employeeinfo.NAME;
                    cmd.Parameters.Add("EMAILADDRESS", SqlDbType.VarChar).Value = employeeinfo.EMAILADDRESS;
                    cmd.Parameters.Add("MOBILENO", SqlDbType.VarChar).Value = employeeinfo.MOBILENO;
                    cmd.Parameters.Add("ADDRESS", SqlDbType.VarChar).Value = employeeinfo.ADDRESS;
                    cmd.Parameters.Add("ENCODEDBY", SqlDbType.VarChar).Value = "currentuser";
                    cmd.Parameters.Add("MODIFIEDBY", SqlDbType.VarChar).Value = "currentuser";
                    cmd.Parameters.Add("ACTIVE", SqlDbType.VarChar).Value = employeeinfo.ACTIVE;
                    cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = employeeinfo.ID;
                    cmd.ExecuteNonQuery();

                    dr.Dispose();
                    cmd.Dispose();

                    result = "Employee successfully updated";


                }


                dr.Dispose();
                cmd.Dispose();


                dbconn.closeConnection();
                return result;

            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            finally
            {
                dbconn.closeConnection();
            }




        }

	}
}