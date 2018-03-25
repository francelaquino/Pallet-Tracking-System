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
    public class pallettypeController : Controller
    {
        //
        // GET: /en/pallettype/
        public ActionResult newPalletType()
        {
            return View();
        }

        [HttpPost]
        public string savePalletType(mdlPalletType typeinfo)
        {

            string result = "";

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT ID FROM TYPES WHERE TYPE=@TYPE";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("TYPE", SqlDbType.VarChar).Value = typeinfo.TYPE;

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    result = "Pallet type already exist";
                }
                else
                {
                    dr.Dispose();
                    cmd.Dispose();

                    strSQL = @"INSERT INTO TYPES(TYPE,ACTIVE,ENCODEDBY,MODIFIEDBY,DATEMODIFIED,DATEENCODED)
                    VALUES(@TYPE,@ACTIVE,@ENCODEDBY,@MODIFIEDBY,GETDATE(),GETDATE())";

                    cmd = new SqlCommand(strSQL, dbconn.DbConn);
                    cmd.Parameters.Add("TYPE", SqlDbType.VarChar).Value = typeinfo.TYPE;
                    cmd.Parameters.Add("ACTIVE", SqlDbType.VarChar).Value = typeinfo.ACTIVE;
                    cmd.Parameters.Add("ENCODEDBY", SqlDbType.VarChar).Value = "currentuser";
                    cmd.Parameters.Add("MODIFIEDBY", SqlDbType.VarChar).Value = "currentuser";
                    cmd.ExecuteNonQuery();

                    result = "Pallet type successfully saved.";
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
        public string updatePalletType(mdlPalletType typeinfo)
        {

            string result = "";

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT ID FROM TYPES WHERE TYPE=@TYPE";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("TYPE", SqlDbType.VarChar).Value = typeinfo.TYPE;

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    if (dr["ID"].ToString() != typeinfo.ID)
                    {
                        result = "Pallet type already exist";
                    }

                }

                dr.Dispose();
                cmd.Dispose();

                if (result == "")
                {


                    strSQL = @"UPDATE TYPES SET TYPE=@TYPE,ACTIVE=@ACTIVE,MODIFIEDBY=@MODIFIEDBY,DATEMODIFIED=GETDATE()
                        WHERE ID=@ID";

                    cmd = new SqlCommand(strSQL, dbconn.DbConn);
                    cmd.Parameters.Add("TYPE", SqlDbType.VarChar).Value = typeinfo.TYPE;
                    cmd.Parameters.Add("ACTIVE", SqlDbType.VarChar).Value = typeinfo.ACTIVE;
                    cmd.Parameters.Add("MODIFIEDBY", SqlDbType.VarChar).Value = "currentuser";
                    cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = typeinfo.ID;
                    cmd.ExecuteNonQuery();

                    dr.Dispose();
                    cmd.Dispose();

                    result = "Pallet type successfully updated";


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

        public ActionResult palletTypes()
        {
            return View();
        }


        public ActionResult EditPalletType()
        {
            return View();
        }


        
        

        [HttpGet]
        public ActionResult getPalletTypeInfo(string id)
        {
            mdlPalletType model = new mdlPalletType();

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT ID,TYPE,ACTIVE FROM TYPES WHERE ID=@ID";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = id;

                SqlDataReader dr = cmd.ExecuteReader();
               
                if(dr.HasRows)
                {
                    dr.Read();
                    model.ID = dr["ID"].ToString();
                    model.TYPE = dr["TYPE"].ToString();
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
        public ActionResult getPalletTypes()
        {

            List<mdlPalletType> model = new List<mdlPalletType>();

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT ID,TYPE,Case When ACTIVE = 'A' Then 'Active' Else 'Inactive' End AS ACTIVE FROM TYPES ORDER BY TYPE ASC";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);

                SqlDataReader dr = cmd.ExecuteReader();
                mdlPalletType list = new mdlPalletType();
                while (dr.Read())
                {
                     list = new mdlPalletType();
                    list.ID = dr["ID"].ToString();
                    list.TYPE = dr["TYPE"].ToString();
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
        public ActionResult getActivePalletType()
        {

            List<mdlPalletType> model = new List<mdlPalletType>();

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT ID,TYPE FROM TYPES WHERE ACTIVE='A' ORDER BY TYPE ASC";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);

                SqlDataReader dr = cmd.ExecuteReader();
                mdlPalletType list = new mdlPalletType();
                while (dr.Read())
                {
                    list = new mdlPalletType();
                    list.ID = dr["ID"].ToString();
                    list.TYPE = dr["TYPE"].ToString();
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