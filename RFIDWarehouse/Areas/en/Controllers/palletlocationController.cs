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
    public class palletlocationController : Controller
    {
        //
        // GET: /en/palletlocation/
        public ActionResult newPalletLocation()
        {
            return View();
        }

        [HttpPost]
        public string savePalletLocation(mdlPalletLocation locationinfo)
        {

            string result = "";

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT ID FROM LOCATIONS WHERE LOCATION=@LOCATION";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("LOCATION", SqlDbType.VarChar).Value = locationinfo.LOCATION;

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    result = "Pallet location already exist";
                }
                else
                {
                    dr.Dispose();
                    cmd.Dispose();

                    strSQL = @"INSERT INTO LOCATIONS(LOCATION,ACTIVE,ENCODEDBY,MODIFIEDBY,DATEMODIFIED,DATEENCODED)
                    VALUES(@LOCATION,@ACTIVE,@ENCODEDBY,@MODIFIEDBY,GETDATE(),GETDATE())";

                    cmd = new SqlCommand(strSQL, dbconn.DbConn);
                    cmd.Parameters.Add("LOCATION", SqlDbType.VarChar).Value = locationinfo.LOCATION;
                    cmd.Parameters.Add("ACTIVE", SqlDbType.VarChar).Value = locationinfo.ACTIVE;
                    cmd.Parameters.Add("ENCODEDBY", SqlDbType.VarChar).Value = "currentuser";
                    cmd.Parameters.Add("MODIFIEDBY", SqlDbType.VarChar).Value = "currentuser";
                    cmd.ExecuteNonQuery();

                    result = "Pallet location successfully saved.";
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
        public string updatePalletLocation(mdlPalletLocation locationinfo)
        {

            string result = "";

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT ID FROM LOCATIONS WHERE LOCATION=@LOCATION";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("LOCATION", SqlDbType.VarChar).Value = locationinfo.LOCATION;

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    if (dr["ID"].ToString() != locationinfo.ID)
                    {
                        result = "Pallet location already exist";
                    }

                }

                dr.Dispose();
                cmd.Dispose();

                if (result == "")
                {


                    strSQL = @"UPDATE LOCATIONS SET LOCATION=@LOCATION,ACTIVE=@ACTIVE,MODIFIEDBY=@MODIFIEDBY,DATEMODIFIED=GETDATE()
                        WHERE ID=@ID";

                    cmd = new SqlCommand(strSQL, dbconn.DbConn);
                    cmd.Parameters.Add("LOCATION", SqlDbType.VarChar).Value = locationinfo.LOCATION;
                    cmd.Parameters.Add("ACTIVE", SqlDbType.VarChar).Value = locationinfo.ACTIVE;
                    cmd.Parameters.Add("MODIFIEDBY", SqlDbType.VarChar).Value = "currentuser";
                    cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = locationinfo.ID;
                    cmd.ExecuteNonQuery();

                    dr.Dispose();
                    cmd.Dispose();

                    result = "Pallet location successfully updated";


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

        public ActionResult palletLocations()
        {
            return View();
        }


        public ActionResult EditPalletLocation()
        {
            return View();
        }


        
        

        [HttpGet]
        public ActionResult getPalletLocationInfo(string id)
        {
            mdlPalletLocation model = new mdlPalletLocation();

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT ID,LOCATION,ACTIVE FROM LOCATIONS WHERE ID=@ID";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = id;

                SqlDataReader dr = cmd.ExecuteReader();
               
                if(dr.HasRows)
                {
                    dr.Read();
                    model.ID = dr["ID"].ToString();
                    model.LOCATION = dr["LOCATION"].ToString();
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
        public ActionResult getPalletLocation()
        {

            List<mdlPalletLocation> model = new List<mdlPalletLocation>();

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT ID,LOCATION,Case When ACTIVE = 'A' Then 'Active' Else 'Inactive' End AS ACTIVE FROM LOCATIONS ORDER BY LOCATION ASC";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);

                SqlDataReader dr = cmd.ExecuteReader();
                mdlPalletLocation list = new mdlPalletLocation();
                while (dr.Read())
                {
                     list = new mdlPalletLocation();
                    list.ID = dr["ID"].ToString();
                    list.LOCATION = dr["LOCATION"].ToString();
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
        public ActionResult getActivePalletLocation()
        {

            List<mdlPalletLocation> model = new List<mdlPalletLocation>();

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT ID,LOCATION FROM LOCATIONS WHERE ACTIVE='A' ORDER BY LOCATION ASC";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);

                SqlDataReader dr = cmd.ExecuteReader();
                mdlPalletLocation list = new mdlPalletLocation();
                while (dr.Read())
                {
                    list = new mdlPalletLocation();
                    list.ID = dr["ID"].ToString();
                    list.LOCATION = dr["LOCATION"].ToString();
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