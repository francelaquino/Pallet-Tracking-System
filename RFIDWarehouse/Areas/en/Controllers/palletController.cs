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
    public class palletController : Controller
    {
        //
        // GET: /en/pallet/
        public ActionResult newPallet()
        {
            return View();
        }

        public ActionResult assignPallet()
        {
            return View();
        }

        public ActionResult Pallets()
        {
            return View();
        }

        [HttpPost]
        public string savePallet(mdlPallet palletinfo)
        {

            string result = "";

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT ID FROM PALLETS WHERE RFID=@RFID";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("RFID", SqlDbType.VarChar).Value = palletinfo.RFID;

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    result = "RFID already exist";
                }

                dr.Dispose();
                cmd.Dispose();


                strSQL = @"SELECT ID FROM PALLETS WHERE NAME=@NAME";

                 cmd = new SqlCommand(strSQL, dbconn.DbConn);
                 cmd.Parameters.Add("NAME", SqlDbType.VarChar).Value = palletinfo.NAME;

                 dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    result = "Pallet name already exist";
                }

                dr.Dispose();
                cmd.Dispose();


                if (result == "") { 

                    

                    strSQL = @"INSERT INTO PALLETS(GID,BARCODE,RFID,TYPE,STYLE,SIZE,LOCATION,SUPPLIER,ENCODEDBY,MODIFIEDBY,DATEMODIFIED,DATEENCODED,ACTIVE,AREA,NAME)
                    VALUES(CONVERT(varchar(10), right(newid(),10)),@BARCODE,@RFID,@TYPE,@STYLE,@SIZE,@LOCATION,@SUPPLIER,@ENCODEDBY,@MODIFIEDBY,GETDATE(),GETDATE(),@ACTIVE,@AREA,@NAME)";




                    cmd = new SqlCommand(strSQL, dbconn.DbConn);
                    cmd.Parameters.Add("BARCODE", SqlDbType.VarChar).Value = palletinfo.BARCODE;
                    cmd.Parameters.Add("RFID", SqlDbType.VarChar).Value = palletinfo.RFID;
                    cmd.Parameters.Add("TYPE", SqlDbType.VarChar).Value = palletinfo.TYPE;
                    cmd.Parameters.Add("STYLE", SqlDbType.VarChar).Value = palletinfo.STYLE;
                    cmd.Parameters.Add("SIZE", SqlDbType.VarChar).Value = palletinfo.SIZE;
                    cmd.Parameters.Add("LOCATION", SqlDbType.VarChar).Value = palletinfo.LOCATION;
                    cmd.Parameters.Add("SUPPLIER", SqlDbType.VarChar).Value = palletinfo.SUPPLIER;
                    cmd.Parameters.Add("ENCODEDBY", SqlDbType.VarChar).Value ="currentuser";
                    cmd.Parameters.Add("MODIFIEDBY", SqlDbType.VarChar).Value = "currentuser";
                    cmd.Parameters.Add("ACTIVE", SqlDbType.VarChar).Value = palletinfo.ACTIVE;
                    cmd.Parameters.Add("AREA", SqlDbType.VarChar).Value = palletinfo.AREA;
                    cmd.Parameters.Add("NAME", SqlDbType.VarChar).Value = palletinfo.NAME;
                    cmd.ExecuteNonQuery();

                    result = "Pallet successfully saved.";
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

        [HttpGet]
        public ActionResult getPallets()
        {

            List<mdlPallet> model = new List<mdlPallet>();

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT ID,GID,BARCODE,RFID,TYPE,LOCATION,NAME,EMPLOYEE,ACTIVE,AREA FROM VPALLETS ";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);

                SqlDataReader dr = cmd.ExecuteReader();
                mdlPallet list = new mdlPallet();
                while (dr.Read())
                {
                    list = new mdlPallet();
                    list.ID = dr["ID"].ToString();
                    list.GID = dr["GID"].ToString();
                    list.BARCODE = dr["BARCODE"].ToString();
                    list.RFID = dr["RFID"].ToString();
                    list.TYPE = dr["TYPE"].ToString();
                    list.EMPLOYEE = dr["EMPLOYEE"].ToString();
                    list.LOCATION = dr["LOCATION"].ToString();
                    list.NAME = dr["NAME"].ToString();
                    list.ACTIVE = dr["ACTIVE"].ToString();
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
        public ActionResult getActivePallets()
        {

            List<mdlPallet> model = new List<mdlPallet>();

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT ID,NAME FROM PALLETS WHERE ACTIVE='A'";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);

                SqlDataReader dr = cmd.ExecuteReader();
                mdlPallet list = new mdlPallet();
                while (dr.Read())
                {
                    list = new mdlPallet();
                    list.ID = dr["ID"].ToString();
                    list.NAME = dr["NAME"].ToString();
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

        public ActionResult EditPallet()
        {
            return View();
        }

        [HttpGet]
        public ActionResult getPalletInfo(string id,string gid)
        {
            mdlPallet model = new mdlPallet();

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT * FROM PALLETS WHERE ID=@ID AND GID=@GID";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("GID", SqlDbType.VarChar).Value = gid;

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    dr.Read();
                    model.ID = dr["ID"].ToString();
                    model.GID = dr["GID"].ToString();
                    model.BARCODE = dr["BARCODE"].ToString();
                    model.RFID = dr["RFID"].ToString();
                    model.TYPE = dr["TYPE"].ToString();
                    model.STYLE = dr["STYLE"].ToString();
                    model.SIZE = dr["SIZE"].ToString();
                    model.LOCATION = dr["LOCATION"].ToString();
                    model.SUPPLIER = dr["SUPPLIER"].ToString();
                    model.NAME = dr["NAME"].ToString();
                    model.ACTIVE = dr["ACTIVE"].ToString();
                    model.AREA = dr["AREA"].ToString();
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
        public string updatePallet(mdlPallet palletinfo)
        {

            string result = "";

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT ID FROM PALLETS WHERE RFID=@RFID";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("RFID", SqlDbType.VarChar).Value = palletinfo.RFID;

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    if (dr["ID"].ToString() != palletinfo.ID)
                    {
                        result = "Pallet RFID already exist";
                    }

                }

                dr.Dispose();
                cmd.Dispose();

                 strSQL = @"SELECT ID FROM PALLETS WHERE NAME=@NAME";

                 cmd = new SqlCommand(strSQL, dbconn.DbConn);
                 cmd.Parameters.Add("NAME", SqlDbType.VarChar).Value = palletinfo.NAME;

                 dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    if (dr["ID"].ToString() != palletinfo.ID)
                    {
                        result = "Pallet name already exist";
                    }

                }

                dr.Dispose();
                cmd.Dispose();

                if (result == "")
                {


                    strSQL = @"UPDATE PALLETS SET BARCODE=@BARCODE,RFID=@RFID,TYPE=@TYPE,STYLE=@STYLE,SIZE=@SIZE,LOCATION=@LOCATION,SUPPLIER=@SUPPLIER,ACTIVE=@ACTIVE,
                    MODIFIEDBY=@MODIFIEDBY,DATEMODIFIED=GETDATE(),AREA=@AREA,NAME=@NAME WHERE ID=@ID AND GID=@GID";

                    cmd = new SqlCommand(strSQL, dbconn.DbConn);
                    cmd.Parameters.Add("BARCODE", SqlDbType.VarChar).Value = palletinfo.BARCODE;
                    cmd.Parameters.Add("RFID", SqlDbType.VarChar).Value = palletinfo.RFID;
                    cmd.Parameters.Add("TYPE", SqlDbType.VarChar).Value = palletinfo.TYPE;
                    cmd.Parameters.Add("STYLE", SqlDbType.VarChar).Value = palletinfo.STYLE;
                    cmd.Parameters.Add("SIZE", SqlDbType.VarChar).Value = palletinfo.SIZE;
                    cmd.Parameters.Add("LOCATION", SqlDbType.VarChar).Value = palletinfo.LOCATION;
                    cmd.Parameters.Add("SUPPLIER", SqlDbType.VarChar).Value = palletinfo.SUPPLIER;
                    cmd.Parameters.Add("ACTIVE", SqlDbType.VarChar).Value = palletinfo.ACTIVE;
                    cmd.Parameters.Add("MODIFIEDBY", SqlDbType.VarChar).Value = "currentuser";
                    cmd.Parameters.Add("AREA", SqlDbType.VarChar).Value = palletinfo.AREA;
                    cmd.Parameters.Add("NAME", SqlDbType.VarChar).Value = palletinfo.NAME;
                    cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = palletinfo.ID;
                    cmd.Parameters.Add("GID", SqlDbType.VarChar).Value = palletinfo.GID;
                    
                    cmd.ExecuteNonQuery();

                    dr.Dispose();
                    cmd.Dispose();

                    result = "Pallet successfully updated";


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
        public string assignPallet(mdlPallet palletinfo)
        {

            string result = "";

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"UPDATE PALLETS SET ASSIGNEDTO=@ASSIGNEDTO WHERE ID=@ID";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("ASSIGNEDTO", SqlDbType.VarChar).Value = palletinfo.EMPLOYEE;
                cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = palletinfo.ID;
                cmd.ExecuteNonQuery();

                result = "Pallet successfull assigned";
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
