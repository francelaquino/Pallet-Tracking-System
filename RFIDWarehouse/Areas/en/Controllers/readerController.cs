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
    public class readerController : Controller
    {
        //
        // GET: /en/reader/
        public ActionResult newReader()
        {
            return View();
        }

        public ActionResult rfidlogs()
        {
            return View();
        }

        public ActionResult Reader()
        {
            return View();

        }
        public ActionResult editReader()
        {
            return View();
        }

        [HttpGet]
        public ActionResult getReaderTime()
        {

            List<mdlTime> model = new List<mdlTime>();



           

            for (int x = 1; x <= 10; x++)
            {
                mdlTime list = new mdlTime();
                list.TIME = x.ToString();
                model.Add(list);
            }
            return Json(model, JsonRequestBehavior.AllowGet);


        }

        [HttpPost]
        public string saveReader(mdlReader readerinfo)
        {

            string result = "";

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT ID FROM READERS WHERE IP=@IP";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("IP", SqlDbType.VarChar).Value = readerinfo.IPADDRESS;

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    result = "Ip address already exist";
                }
                else
                {
                    dr.Dispose();
                    cmd.Dispose();

                    strSQL = @"INSERT INTO READERS(GID,READERMODEL,READERNAME,PORT,IP,TIME,GATE,ENCODEDBY,MODIFIEDBY,DATEMODIFIED,DATEENCODED,ACTIVE)
                    VALUES(CONVERT(varchar(10), right(newid(),10)),@READERMODEL,@READERNAME,@PORT,@IP,@TIME,@GATE,@ENCODEDBY,@MODIFIEDBY,GETDATE(),GETDATE(),@ACTIVE)";




                    cmd = new SqlCommand(strSQL, dbconn.DbConn);
                    cmd.Parameters.Add("READERMODEL", SqlDbType.VarChar).Value = readerinfo.MODEL;
                    cmd.Parameters.Add("READERNAME", SqlDbType.VarChar).Value = readerinfo.READERNAME;
                    cmd.Parameters.Add("PORT", SqlDbType.VarChar).Value = readerinfo.PORT;
                    cmd.Parameters.Add("IP", SqlDbType.VarChar).Value = readerinfo.IPADDRESS;
                    cmd.Parameters.Add("TIME", SqlDbType.VarChar).Value = readerinfo.TIME;
                    cmd.Parameters.Add("GATE", SqlDbType.VarChar).Value = readerinfo.GATE;
                    cmd.Parameters.Add("ENCODEDBY", SqlDbType.VarChar).Value = "currentuser";
                    cmd.Parameters.Add("MODIFIEDBY", SqlDbType.VarChar).Value = "currentuser";
                    cmd.Parameters.Add("ACTIVE", SqlDbType.VarChar).Value = readerinfo.ACTIVE;
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


        [HttpPost]
        public string saveTag(mdlReader readerinfo)
        {

            string result = "";

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT * FROM READERS WHERE ID=@ID";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = readerinfo.ID;

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    readerinfo.IPADDRESS = dr["IP"].ToString();
                    readerinfo.PORT = dr["PORT"].ToString();
                    readerinfo.GATE = dr["GATE"].ToString();
                    dr.Dispose();
                    cmd.Dispose();

                    strSQL = @"INSERT INTO RFID_LOGS(RFID,READTIME,IPADDRESS,PORT,GATE,READER,EMPLOYEE)
                    VALUES(@RFID,GETDATE(),@IPADDRESS,@PORT,@GATE,@READER,(SELECT ASSIGNEDTO FROM PALLETS WHERE RFID=@RFID))";




                    cmd = new SqlCommand(strSQL, dbconn.DbConn);
                    cmd.Parameters.Add("RFID", SqlDbType.VarChar).Value = readerinfo.TAG;
                    cmd.Parameters.Add("IPADDRESS", SqlDbType.VarChar).Value = readerinfo.IPADDRESS;
                    cmd.Parameters.Add("PORT", SqlDbType.VarChar).Value = readerinfo.PORT;
                    cmd.Parameters.Add("GATE", SqlDbType.VarChar).Value = readerinfo.GATE;
                    cmd.Parameters.Add("READER", SqlDbType.VarChar).Value = readerinfo.ID;
                    cmd.ExecuteNonQuery();

                    result = "Tag successfully saved.";
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
        public ActionResult getReaders()
        {

            List<mdlReader> model = new List<mdlReader>();

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT ID,GID,READERMODEL,READERNAME,PORT,IP,TIME,GATE,ACTIVE FROM VREADERS ";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);

                SqlDataReader dr = cmd.ExecuteReader();
                mdlReader list = new mdlReader();
                while (dr.Read())
                {
                    list = new mdlReader();
                    list.ID = dr["ID"].ToString();
                    list.GID = dr["GID"].ToString();
                    list.MODEL = dr["READERMODEL"].ToString();
                    list.READERNAME = dr["READERNAME"].ToString();
                    list.PORT = dr["PORT"].ToString();
                    list.IPADDRESS = dr["IP"].ToString();
                    list.TIME = dr["TIME"].ToString();
                    list.GATE = dr["GATE"].ToString();
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
        public ActionResult getReaderInfo(string id)
        {
            mdlReader model = new mdlReader();

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT * FROM READERS WHERE ID=@ID";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = id;

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    dr.Read();
                    model.ID = dr["ID"].ToString();
                    model.GID = dr["GID"].ToString();
                    model.MODEL = dr["READERMODEL"].ToString();
                    model.READERNAME = dr["READERNAME"].ToString();
                    model.PORT = dr["PORT"].ToString();
                    model.IPADDRESS = dr["IP"].ToString();
                    model.TIME = dr["TIME"].ToString();
                    model.GATE = dr["GATE"].ToString();
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
        public string updateReader(mdlReader readerinfo)
        {

            string result = "";

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT ID FROM READERS WHERE IP=@IP";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("IP", SqlDbType.VarChar).Value = readerinfo.IPADDRESS;

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    if (dr["ID"].ToString() != readerinfo.ID)
                    {
                        result = "Reader Ip already exist";
                    }

                }

                dr.Dispose();
                cmd.Dispose();

                if (result == "")
                {


                    strSQL = @"UPDATE READERS SET READERMODEL=@READERMODEL,READERNAME=@READERNAME,PORT=@PORT,IP=@IP,TIME=@TIME,GATE=@GATE,ACTIVE=@ACTIVE,
                    MODIFIEDBY=@MODIFIEDBY,DATEMODIFIED=GETDATE() WHERE ID=@ID AND GID=@GID";

                    cmd = new SqlCommand(strSQL, dbconn.DbConn);
                    cmd.Parameters.Add("READERMODEL", SqlDbType.VarChar).Value = readerinfo.MODEL;
                    cmd.Parameters.Add("READERNAME", SqlDbType.VarChar).Value = readerinfo.READERNAME;
                    cmd.Parameters.Add("PORT", SqlDbType.VarChar).Value = readerinfo.PORT;
                    cmd.Parameters.Add("IP", SqlDbType.VarChar).Value = readerinfo.IPADDRESS;
                    cmd.Parameters.Add("TIME", SqlDbType.VarChar).Value = readerinfo.TIME;
                    cmd.Parameters.Add("GATE", SqlDbType.VarChar).Value = readerinfo.GATE;
                    cmd.Parameters.Add("ACTIVE", SqlDbType.VarChar).Value = readerinfo.ACTIVE;
                    cmd.Parameters.Add("MODIFIEDBY", SqlDbType.VarChar).Value = "currentuser";
                    cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = readerinfo.ID;
                    cmd.Parameters.Add("GID", SqlDbType.VarChar).Value = readerinfo.GID;
                    cmd.ExecuteNonQuery();

                    dr.Dispose();
                    cmd.Dispose();

                    result = "Reader successfully updated";


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
