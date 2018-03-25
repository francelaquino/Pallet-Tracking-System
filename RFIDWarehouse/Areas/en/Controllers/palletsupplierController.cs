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
    public class palletsupplierController : Controller
    {
        //
        // GET: /en/palletsupplier/
        public ActionResult newPalletSupplier()
        {
            return View();
        }

        [HttpPost]
        public string savePalletSupplier(mdlPalletSupplier supplierinfo)
        {

            string result = "";

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT ID FROM SUPPLIERS WHERE SUPPLIER=@SUPPLIER";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("SUPPLIER", SqlDbType.VarChar).Value = supplierinfo.SUPPLIER;

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    result = "Pallet supplier already exist";
                }
                else
                {
                    dr.Dispose();
                    cmd.Dispose();

                    strSQL = @"INSERT INTO SUPPLIERS(SUPPLIER,ACTIVE,ENCODEDBY,MODIFIEDBY,DATEMODIFIED,DATEENCODED)
                    VALUES(@SUPPLIER,@ACTIVE,@ENCODEDBY,@MODIFIEDBY,GETDATE(),GETDATE())";

                    cmd = new SqlCommand(strSQL, dbconn.DbConn);
                    cmd.Parameters.Add("SUPPLIER", SqlDbType.VarChar).Value = supplierinfo.SUPPLIER;
                    cmd.Parameters.Add("ACTIVE", SqlDbType.VarChar).Value = supplierinfo.ACTIVE;
                    cmd.Parameters.Add("ENCODEDBY", SqlDbType.VarChar).Value = "currentuser";
                    cmd.Parameters.Add("MODIFIEDBY", SqlDbType.VarChar).Value = "currentuser";
                    cmd.ExecuteNonQuery();

                    result = "Pallet supplier successfully saved.";
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


        [HttpPost]
        public string updatePalletSupplier(mdlPalletSupplier supplierinfo)
        {

            string result = "";

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT ID FROM SUPPLIERS WHERE SUPPLIER=@SUPPLIER";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("SUPPLIER", SqlDbType.VarChar).Value = supplierinfo.SUPPLIER;

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    if (dr["ID"].ToString() != supplierinfo.ID)
                    {
                        result = "Pallet supplier already exist";
                    }

                }

                dr.Dispose();
                cmd.Dispose();

                if (result == "")
                {


                    strSQL = @"UPDATE SUPPLIERS SET SUPPLIER=@SUPPLIER,ACTIVE=@ACTIVE,MODIFIEDBY=@MODIFIEDBY,DATEMODIFIED=GETDATE()
                        WHERE ID=@ID";

                    cmd = new SqlCommand(strSQL, dbconn.DbConn);
                    cmd.Parameters.Add("SUPPLIER", SqlDbType.VarChar).Value = supplierinfo.SUPPLIER;
                    cmd.Parameters.Add("ACTIVE", SqlDbType.VarChar).Value = supplierinfo.ACTIVE;
                    cmd.Parameters.Add("MODIFIEDBY", SqlDbType.VarChar).Value = "currentuser";
                    cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = supplierinfo.ID;
                    cmd.ExecuteNonQuery();

                    dr.Dispose();
                    cmd.Dispose();

                    result = "Pallet supplier successfully updated";


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

        public ActionResult palletSuppliers()
        {
            return View();
        }


        public ActionResult EditPalletSupplier()
        {
            return View();
        }


        
        

        [HttpGet]
        public ActionResult getPalletSupplierInfo(string id)
        {
            mdlPalletSupplier model = new mdlPalletSupplier();

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT ID,SUPPLIER,ACTIVE FROM SUPPLIERS WHERE ID=@ID";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = id;

                SqlDataReader dr = cmd.ExecuteReader();
               
                if(dr.HasRows)
                {
                    dr.Read();
                    model.ID = dr["ID"].ToString();
                    model.SUPPLIER = dr["SUPPLIER"].ToString();
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

        [HttpGet]
        public ActionResult getPalletSupplier()
        {

            List<mdlPalletSupplier> model = new List<mdlPalletSupplier>();

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT ID,SUPPLIER,Case When ACTIVE = 'A' Then 'Active' Else 'Inactive' End AS ACTIVE FROM SUPPLIERS ORDER BY SUPPLIER ASC ";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);

                SqlDataReader dr = cmd.ExecuteReader();
                mdlPalletSupplier list = new mdlPalletSupplier();
                while (dr.Read())
                {
                     list = new mdlPalletSupplier();
                    list.ID = dr["ID"].ToString();
                    list.SUPPLIER = dr["SUPPLIER"].ToString();
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
        public ActionResult getActivePalletSupplier()
        {

            List<mdlPalletSupplier> model = new List<mdlPalletSupplier>();

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT ID,SUPPLIER FROM SUPPLIERS WHERE ACTIVE='A' ORDER BY SUPPLIER ASC ";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);

                SqlDataReader dr = cmd.ExecuteReader();
                mdlPalletSupplier list = new mdlPalletSupplier();
                while (dr.Read())
                {
                    list = new mdlPalletSupplier();
                    list.ID = dr["ID"].ToString();
                    list.SUPPLIER = dr["SUPPLIER"].ToString();
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

	}
}