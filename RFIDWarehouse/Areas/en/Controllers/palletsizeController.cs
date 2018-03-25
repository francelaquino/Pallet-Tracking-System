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
    public class palletsizeController : Controller
    {
        //
        // GET: /en/palletsize/
        public ActionResult newPalletSize()
        {
            return View();
        }

        [HttpPost]
        public string savePalletSize(mdlPalletSize sizeinfo)
        {

            string result = "";

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT ID FROM SIZES WHERE SIZE=@SIZE";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("SIZE", SqlDbType.VarChar).Value = sizeinfo.SIZE;

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    result = "Pallet size already exist";
                }
                else
                {
                    dr.Dispose();
                    cmd.Dispose();

                    strSQL = @"INSERT INTO SIZES(SIZE,ACTIVE,ENCODEDBY,MODIFIEDBY,DATEMODIFIED,DATEENCODED)
                    VALUES(@SIZE,@ACTIVE,@ENCODEDBY,@MODIFIEDBY,GETDATE(),GETDATE())";

                    cmd = new SqlCommand(strSQL, dbconn.DbConn);
                    cmd.Parameters.Add("SIZE", SqlDbType.VarChar).Value = sizeinfo.SIZE;
                    cmd.Parameters.Add("ACTIVE", SqlDbType.VarChar).Value = sizeinfo.ACTIVE;
                    cmd.Parameters.Add("ENCODEDBY", SqlDbType.VarChar).Value = "currentuser";
                    cmd.Parameters.Add("MODIFIEDBY", SqlDbType.VarChar).Value = "currentuser";
                    cmd.ExecuteNonQuery();

                    result = "Pallet size successfully saved.";
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
        public string updatePalletSize(mdlPalletSize sizeinfo)
        {

            string result = "";

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT ID FROM SIZES WHERE SIZE=@SIZE";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("SIZE", SqlDbType.VarChar).Value = sizeinfo.SIZE;

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    if (dr["ID"].ToString() != sizeinfo.ID)
                    {
                        result = "Pallet size already exist";
                    }

                }

                dr.Dispose();
                cmd.Dispose();

                if (result == "")
                {


                    strSQL = @"UPDATE SIZES SET SIZE=@SIZE,ACTIVE=@ACTIVE,MODIFIEDBY=@MODIFIEDBY,DATEMODIFIED=GETDATE()
                        WHERE ID=@ID";

                    cmd = new SqlCommand(strSQL, dbconn.DbConn);
                    cmd.Parameters.Add("SIZE", SqlDbType.VarChar).Value = sizeinfo.SIZE;
                    cmd.Parameters.Add("ACTIVE", SqlDbType.VarChar).Value = sizeinfo.ACTIVE;
                    cmd.Parameters.Add("MODIFIEDBY", SqlDbType.VarChar).Value = "currentuser";
                    cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = sizeinfo.ID;
                    cmd.ExecuteNonQuery();

                    dr.Dispose();
                    cmd.Dispose();

                    result = "Pallet size successfully updated";


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

        public ActionResult palletSizes()
        {
            return View();
        }


        public ActionResult EditPalletSize()
        {
            return View();
        }


        
        

        [HttpGet]
        public ActionResult getPalletSizeInfo(string id)
        {
            mdlPalletSize model = new mdlPalletSize();

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT ID,SIZE,ACTIVE FROM SIZES WHERE ID=@ID";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = id;

                SqlDataReader dr = cmd.ExecuteReader();
               
                if(dr.HasRows)
                {
                    dr.Read();
                    model.ID = dr["ID"].ToString();
                    model.SIZE = dr["SIZE"].ToString();
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
        public ActionResult getPalletSize()
        {

            List<mdlPalletSize> model = new List<mdlPalletSize>();

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT ID,SIZE,Case When ACTIVE = 'A' Then 'Active' Else 'Inactive' End AS ACTIVE FROM SIZES ORDER BY SIZE ASC";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);

                SqlDataReader dr = cmd.ExecuteReader();
                mdlPalletSize list = new mdlPalletSize();
                while (dr.Read())
                {
                     list = new mdlPalletSize();
                    list.ID = dr["ID"].ToString();
                    list.SIZE = dr["SIZE"].ToString();
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
        public ActionResult getActivePalletSize()
        {

            List<mdlPalletSize> model = new List<mdlPalletSize>();

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT ID,SIZE FROM SIZES WHERE ACTIVE='A' ORDER BY SIZE ASC";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);

                SqlDataReader dr = cmd.ExecuteReader();
                mdlPalletSize list = new mdlPalletSize();
                while (dr.Read())
                {
                    list = new mdlPalletSize();
                    list.ID = dr["ID"].ToString();
                    list.SIZE = dr["SIZE"].ToString();
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