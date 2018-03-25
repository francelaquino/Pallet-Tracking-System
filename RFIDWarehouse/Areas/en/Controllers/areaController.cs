using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Db_Connections;
using System.Data;
using System.Data.SqlClient;
using RFIDWarehouse.Areas.en.Models;
using System.IO;
namespace RFIDWarehouse.Areas.en.Controllers
{
    public class areaController : Controller
    {
        //
        // GET: /en/palletlocation/
        public ActionResult newArea()
        {
            return View();
        }

        [HttpPost]
        public string saveArea(mdlArea areainfo)
        {

            string result = "";

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT ID FROM AREAS WHERE AREA=@AREA";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("AREA", SqlDbType.VarChar).Value = areainfo.AREA;

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    result = "Area already exist";
                }
                else
                {
                    dr.Dispose();
                    cmd.Dispose();

                    strSQL = @"INSERT INTO AREAS(AREA,ACTIVE,ENCODEDBY,MODIFIEDBY,DATEMODIFIED,DATEENCODED,AREATYPE)
                    VALUES(@AREA,@ACTIVE,@ENCODEDBY,@MODIFIEDBY,GETDATE(),GETDATE(),@AREATYPE);SELECT SCOPE_IDENTITY()";

                    cmd = new SqlCommand(strSQL, dbconn.DbConn);
                    cmd.Parameters.Add("AREA", SqlDbType.VarChar).Value = areainfo.AREA;
                    cmd.Parameters.Add("ACTIVE", SqlDbType.VarChar).Value = areainfo.ACTIVE;
                    cmd.Parameters.Add("ENCODEDBY", SqlDbType.VarChar).Value = "currentuser";
                    cmd.Parameters.Add("MODIFIEDBY", SqlDbType.VarChar).Value = "currentuser";
                    cmd.Parameters.Add("AREATYPE", SqlDbType.VarChar).Value = areainfo.AREATYPE;
                    var id=cmd.ExecuteScalar();

                    result = id.ToString();
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
        public string updateArea(mdlArea areainfo)
        {

            string result = "";

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT ID FROM AREAS WHERE AREA=@AREA";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("AREA", SqlDbType.VarChar).Value = areainfo.AREA;

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    if (dr["ID"].ToString() != areainfo.ID)
                    {
                        result = "Area already exist";
                    }

                }

                dr.Dispose();
                cmd.Dispose();

                if (result == "")
                {


                    strSQL = @"UPDATE AREAS SET AREA=@AREA,AREATYPE=@AREATYPE,ACTIVE=@ACTIVE,MODIFIEDBY=@MODIFIEDBY,DATEMODIFIED=GETDATE()
                        WHERE ID=@ID";

                    cmd = new SqlCommand(strSQL, dbconn.DbConn);
                    cmd.Parameters.Add("AREA", SqlDbType.VarChar).Value = areainfo.AREA;
                    cmd.Parameters.Add("AREATYPE", SqlDbType.VarChar).Value = areainfo.AREATYPE;
                    cmd.Parameters.Add("ACTIVE", SqlDbType.VarChar).Value = areainfo.ACTIVE;
                    cmd.Parameters.Add("MODIFIEDBY", SqlDbType.VarChar).Value = "currentuser";
                    cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = areainfo.ID;
                    cmd.ExecuteNonQuery();

                    dr.Dispose();
                    cmd.Dispose();

                    result = "Area successfully updated";


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

        public ActionResult areas()
        {
            return View();
        }


        public ActionResult EditArea()
        {
            return View();
        }


        
        

        [HttpGet]
        public ActionResult getAreaInfo(string id)
        {
            mdlArea model = new mdlArea();

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT ID,AREA,AREATYPE,ACTIVE FROM AREAS WHERE ID=@ID";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = id;

                SqlDataReader dr = cmd.ExecuteReader();
               
                if(dr.HasRows)
                {
                    dr.Read();
                    model.ID = dr["ID"].ToString();
                    model.AREATYPE = dr["AREATYPE"].ToString();
                    model.AREA = dr["AREA"].ToString();
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
        public ActionResult getAreas()
        {

            List<mdlArea> model = new List<mdlArea>();

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT dbo.areas.ID, dbo.areas.AREA, Case When dbo.areas.ACTIVE = 'A' Then 'Active' Else 'Inactive' End AS ACTIVE , dbo.areatype.AREATYPE, dbo.areas.MAP
                FROM dbo.areas LEFT OUTER JOIN dbo.areatype ON dbo.areas.AREATYPE = dbo.areatype.ID
                ORDER BY dbo.areas.AREA";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);

                SqlDataReader dr = cmd.ExecuteReader();
                mdlArea list = new mdlArea();
                while (dr.Read())
                {
                     list = new mdlArea();
                    list.ID = dr["ID"].ToString();
                    list.AREA = dr["AREA"].ToString();
                    list.AREATYPE = dr["AREATYPE"].ToString();
                    list.ACTIVE = dr["ACTIVE"].ToString();
                    list.MAP = dr["MAP"].ToString();
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
        public ActionResult getActiveArea()
        {

            List<mdlArea> model = new List<mdlArea>();

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT ID,AREA FROM AREAS WHERE ACTIVE='A' ORDER BY AREA ASC";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);

                SqlDataReader dr = cmd.ExecuteReader();
                mdlArea list = new mdlArea();
                while (dr.Read())
                {
                    list = new mdlArea();
                    list.ID = dr["ID"].ToString();
                    list.AREA = dr["AREA"].ToString();
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
        public ActionResult getAreaType()
        {

            List<mdlArea> model = new List<mdlArea>();

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT ID,AREATYPE FROM AREATYPE WHERE ACTIVE='A' ";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);

                SqlDataReader dr = cmd.ExecuteReader();
                mdlArea list = new mdlArea();
                while (dr.Read())
                {
                    list = new mdlArea();
                    list.ID = dr["ID"].ToString();
                    list.AREATYPE = dr["AREATYPE"].ToString();
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


        [HttpPost]
        public string uploadMap()
        {
            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();

                string txtid = Request.Form["txtid"].ToString();

                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];

                    if (file != null && file.ContentLength > 0)
                    {

                        int fileSize = file.ContentLength;
                        if (fileSize > 2100000)
                        {
                            return "Filesize of " + fileSize.ToString() + " is too large. Maximum file size permitted is 2 MB";
                        }



                        string ext = Path.GetExtension(file.FileName);

                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/Map/"), txtid + ext);
                        file.SaveAs(path);



                        string strSQL = @"UPDATE AREAS SET MAP=@MAP WHERE ID=@ID";

                        SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                        cmd.Parameters.Add("MAP", SqlDbType.VarChar).Value = txtid + ext;
                        cmd.Parameters.Add("ID", SqlDbType.VarChar).Value =txtid;
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();


                       

                    }
                }



                dbconn.closeConnection();
                return "";
                //return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return "";
            }

            finally
            {
                dbconn.closeConnection();
            }




        }
	}
}