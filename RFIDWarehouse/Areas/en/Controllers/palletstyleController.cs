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
    public class palletstyleController : Controller
    {
        //
        // GET: /en/palletstyle/
        public ActionResult newPalletStyle()
        {
            return View();
        }

        [HttpPost]
        public string savePalletStyle(mdlPalletStyle styleinfo)
        {

            string result = "";

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT ID FROM STYLES WHERE STYLE=@STYLE";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("STYLE", SqlDbType.VarChar).Value = styleinfo.STYLE;

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    result = "Pallet type already exist";
                }
                else
                {
                    dr.Dispose();
                    cmd.Dispose();

                    strSQL = @"INSERT INTO STYLES(STYLE,ACTIVE,ENCODEDBY,MODIFIEDBY,DATEMODIFIED,DATEENCODED)
                    VALUES(@STYLE,@ACTIVE,@ENCODEDBY,@MODIFIEDBY,GETDATE(),GETDATE())";

                    cmd = new SqlCommand(strSQL, dbconn.DbConn);
                    cmd.Parameters.Add("STYLE", SqlDbType.VarChar).Value = styleinfo.STYLE;
                    cmd.Parameters.Add("ACTIVE", SqlDbType.VarChar).Value = styleinfo.ACTIVE;
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
        public string updatePalletStyle(mdlPalletStyle styleinfo)
        {

            string result = "";

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT ID FROM STYLES WHERE STYLE=@STYLE";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("STYLE", SqlDbType.VarChar).Value = styleinfo.STYLE;

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    if (dr["ID"].ToString() != styleinfo.ID)
                    {
                        result = "Pallet type already exist";
                    }

                }

                dr.Dispose();
                cmd.Dispose();

                if (result == "")
                {


                    strSQL = @"UPDATE STYLES SET STYLE=@STYLE,ACTIVE=@ACTIVE,MODIFIEDBY=@MODIFIEDBY,DATEMODIFIED=GETDATE()
                        WHERE ID=@ID";

                    cmd = new SqlCommand(strSQL, dbconn.DbConn);
                    cmd.Parameters.Add("STYLE", SqlDbType.VarChar).Value = styleinfo.STYLE;
                    cmd.Parameters.Add("ACTIVE", SqlDbType.VarChar).Value = styleinfo.ACTIVE;
                    cmd.Parameters.Add("MODIFIEDBY", SqlDbType.VarChar).Value = "currentuser";
                    cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = styleinfo.ID;
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

        public ActionResult palletStyles()
        {
            return View();
        }


        public ActionResult EditPalletStyle()
        {
            return View();
        }





        [HttpGet]
        public ActionResult getPalletStyleInfo(string id)
        {
            mdlPalletStyle model = new mdlPalletStyle();

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT ID,STYLE,ACTIVE FROM STYLES WHERE ID=@ID";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = id;

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    dr.Read();
                    model.ID = dr["ID"].ToString();
                    model.STYLE = dr["STYLE"].ToString();
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
        public ActionResult getPalletStyles()
        {

            List<mdlPalletStyle> model = new List<mdlPalletStyle>();

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT ID,STYLE,Case When ACTIVE = 'A' Then 'Active' Else 'Inactive' End AS ACTIVE FROM STYLES ORDER BY STYLE";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);

                SqlDataReader dr = cmd.ExecuteReader();
                mdlPalletStyle list = new mdlPalletStyle();
                while (dr.Read())
                {
                    list = new mdlPalletStyle();
                    list.ID = dr["ID"].ToString();
                    list.STYLE = dr["STYLE"].ToString();
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
        public ActionResult getActivePalletStyle()
        {

            List<mdlPalletStyle> model = new List<mdlPalletStyle>();

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT ID,STYLE FROM STYLES WHERE ACTIVE='A' ORDER BY STYLE";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);

                SqlDataReader dr = cmd.ExecuteReader();
                mdlPalletStyle list = new mdlPalletStyle();
                while (dr.Read())
                {
                    list = new mdlPalletStyle();
                    list.ID = dr["ID"].ToString();
                    list.STYLE = dr["STYLE"].ToString();
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