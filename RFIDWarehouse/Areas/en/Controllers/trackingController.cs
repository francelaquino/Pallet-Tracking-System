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
    public class trackingController : Controller
    {
        //
        // GET: /en/tracking/
        public ActionResult movements()
        {
            return View();
        }

        public ActionResult palletactivity()
        {
            return View();
        }

        public ActionResult palletinventory()
        {
            return View();
        }


        public ActionResult palletlocation()
        {
            return View();
        }

        public ActionResult palletCount()
        {
            return View();
        }

          [HttpGet]
        public ActionResult getPalletCount()
        {
            Db_Connection dbconn = new Db_Connection();
            Db_Connection dbconn1 = new Db_Connection();
            List<mdlPallet> model = new List<mdlPallet>();
            try
            {

                dbconn.openConnection();
                dbconn1.openConnection();


                string strSQL = @"SELECT DISTINCT RFID,NAME,EMPLOYEE FROM VPALLET_COUNT WHERE EMPLOYEE IS NOT NULL ORDER BY EMPLOYEE ASC";


               


                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    DateTime rundate = new DateTime();
                    bool isStarted = false;
                    int cnt = 0;


                    strSQL = "SELECT READTIME FROM VPALLET_COUNT WHERE RFID=@RFID AND EMPLOYEE=@EMPLOYEE ORDER BY READTIME ASC";
                    SqlCommand cmd1 = new SqlCommand(strSQL, dbconn1.DbConn);
                    cmd1.Parameters.Add("RFID", SqlDbType.VarChar).Value = dr["RFID"].ToString();
                    cmd1.Parameters.Add("EMPLOYEE", SqlDbType.VarChar).Value = dr["EMPLOYEE"].ToString();
                    SqlDataReader dr1 = cmd1.ExecuteReader();

                    while (dr1.Read())
                    {
                        if (isStarted == false)
                        {
                            isStarted = true;
                            cnt++;

                        }
                        else
                        {
                            TimeSpan time = Convert.ToDateTime(dr1["READTIME"].ToString()) - rundate;
                            if (time.TotalMinutes >= 5)
                            {
                                cnt++;
                            }
                        }
                        rundate = Convert.ToDateTime(dr1["READTIME"].ToString());


                    }

                    if (cnt > 0)
                    {
                        mdlPallet list = new mdlPallet();
                        list.EMPLOYEE = dr["NAME"].ToString();
                        list.CNT = cnt.ToString();
                        list.RFID = dr["RFID"].ToString();
                        model.Add(list);
                    }
                    cmd1.Dispose();
                    dr1.Dispose();
                }
                dr.Dispose();
                cmd.Dispose();


                dbconn.closeConnection();
                dbconn1.closeConnection();
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
        public ActionResult getPalletMovements(string id)
        {

            List<mdlPallet> model = new List<mdlPallet>();

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT RFID,READTIME,AREA,READERNAME,TYPE,STYLE FROM VRFID_LOGS_15MIN WHERE AREAID=@AREAID ORDER BY READTIME ASC";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("AREAID", SqlDbType.VarChar).Value = id;

                SqlDataReader dr = cmd.ExecuteReader();
                mdlPallet list = new mdlPallet();
                while (dr.Read())
                {
                    list = new mdlPallet();
                    list.RFID = dr["RFID"].ToString();
                    list.READTIME = Convert.ToDateTime(dr["READTIME"].ToString()).ToString("dd-MMM-yyyy H:mm:ss tt");
                    list.AREA = dr["AREA"].ToString();
                    list.READERNAME = dr["READERNAME"].ToString();
                    list.TYPE = dr["TYPE"].ToString();
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


        public ActionResult getPalletActivity(string id)
        {

            List<mdlPallet> model = new List<mdlPallet>();

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();
                if (string.IsNullOrEmpty(id))
                {
                    id = "0";
                }

                string strSQL = @"SELECT DATEDIFF(day, dbo.vrfid_logs_max.READTIME, GETDATE()) AS DAYS, dbo.vpallets.BARCODE, dbo.vpallets.RFID, dbo.vpallets.TYPE, dbo.vpallets.STYLE, dbo.vrfid_logs_max.READTIME, 
                         dbo.vrfid_logs_max.AREA FROM dbo.vpallets LEFT OUTER JOIN
                         dbo.vrfid_logs_max ON dbo.vpallets.RFID = dbo.vrfid_logs_max.RFID
                        WHERE (dbo.vpallets.ACTIVE ='Active') AND (DATEDIFF(day, dbo.vrfid_logs_max.READTIME, GETDATE()) >= @DAY OR DBO.VRFID_LOGS_MAX.READTIME IS NULL)";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("DAY", SqlDbType.VarChar).Value = id;

                SqlDataReader dr = cmd.ExecuteReader();
                mdlPallet list = new mdlPallet();
                while (dr.Read())
                {
                    list = new mdlPallet();
                    list.BARCODE = dr["BARCODE"].ToString();
                    list.AREA = dr["AREA"].ToString();
                    list.RFID = dr["RFID"].ToString();
                    
                    if (!string.IsNullOrEmpty(dr["DAYS"].ToString()))
                    {
                        list.DAYS = " - Last "+dr["DAYS"].ToString()+" day(s) ";
                    }
                    if (!string.IsNullOrEmpty(dr["READTIME"].ToString()))
                    {
                        list.READTIME = Convert.ToDateTime(dr["READTIME"].ToString()).ToString("dd-MMM-yyyy H:mm:ss tt");
                    }
                    else
                    {
                        list.READTIME = "No Activity";
                        list.AREA = "No Activity";
                    }

                   
                    list.TYPE = dr["TYPE"].ToString();
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


        public ActionResult getAreaTypeInventory()
        {

            List<mdlPallet> model = new List<mdlPallet>();

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT COUNT(dbo.vpallets.TYPE) AS CNT, dbo.vrfid_logs_max.AREA, dbo.vpallets.TYPE
                FROM dbo.vpallets INNER JOIN
                 dbo.vrfid_logs_max ON dbo.vpallets.RFID = dbo.vrfid_logs_max.RFID
                WHERE (dbo.vrfid_logs_max.READTIME IS NOT NULL) AND (dbo.vpallets.ACTIVE = 'Active')
                GROUP BY dbo.vrfid_logs_max.AREA, dbo.vpallets.TYPE
                ORDER BY dbo.vrfid_logs_max.AREA";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);

                SqlDataReader dr = cmd.ExecuteReader();
                mdlPallet list = new mdlPallet();
                while (dr.Read())
                {
                    list = new mdlPallet();
                    list.AREA = dr["AREA"].ToString();
                    list.TYPE = dr["TYPE"].ToString();
                    list.CNT = dr["CNT"].ToString();
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

        public ActionResult getAreaInventory()
        {

            List<mdlPallet> model = new List<mdlPallet>();

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT dbo.vrfid_logs_max.AREA, COUNT(dbo.vrfid_logs_max.AREA) AS CNT
                FROM dbo.vrfid_logs_max INNER JOIN
                 dbo.pallets ON dbo.vrfid_logs_max.RFID = dbo.pallets.RFID
                WHERE (dbo.vrfid_logs_max.READTIME IS NOT NULL)
                GROUP BY dbo.vrfid_logs_max.AREA ORDER BY dbo.vrfid_logs_max.AREA ";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);

                SqlDataReader dr = cmd.ExecuteReader();
                mdlPallet list = new mdlPallet();
                while (dr.Read())
                {
                    list = new mdlPallet();
                    list.AREA = dr["AREA"].ToString();
                    list.CNT = dr["CNT"].ToString();
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


        public ActionResult getPalletLocation(string id)
        {

            List<mdlPallet> model = new List<mdlPallet>();

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();

                string strSQL = @"SELECT DATEDIFF(day, dbo.vrfid_logs_max.READTIME, GETDATE()) AS DAYS, dbo.vpallets.BARCODE, dbo.vpallets.RFID, dbo.vpallets.TYPE, dbo.vpallets.STYLE, dbo.vrfid_logs_max.READTIME, 
                         dbo.vrfid_logs_max.AREA, dbo.vpallets.ACTIVE, dbo.vpallets.LOCATION
                        FROM dbo.vpallets LEFT OUTER JOIN
                         dbo.vrfid_logs_max ON dbo.vpallets.RFID = dbo.vrfid_logs_max.RFID
                        WHERE (dbo.vpallets.ACTIVE = 'Active') AND (dbo.vrfid_logs_max.READTIME IS NOT NULL) AND (dbo.vrfid_logs_max.AREAID=@AREA)";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("AREA", SqlDbType.VarChar).Value = id;

                SqlDataReader dr = cmd.ExecuteReader();
                mdlPallet list = new mdlPallet();
                while (dr.Read())
                {
                    list = new mdlPallet();
                    list.BARCODE = dr["BARCODE"].ToString();
                    list.RFID = dr["RFID"].ToString();
                    list.LOCATION = dr["LOCATION"].ToString();

                    if (!string.IsNullOrEmpty(dr["DAYS"].ToString()))
                    {
                        list.DAYS = " - Last " + dr["DAYS"].ToString() + " day(s) ";
                    }
                    if (!string.IsNullOrEmpty(dr["READTIME"].ToString()))
                    {
                        list.READTIME = Convert.ToDateTime(dr["READTIME"].ToString()).ToString("dd-MMM-yyyy H:mm:ss tt");
                    }
                    list.AREA = dr["AREA"].ToString();
                    list.TYPE = dr["TYPE"].ToString();
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


        [HttpGet]
        public ActionResult searchPalletLocation(string id)
        {

            List<mdlPallet> model = new List<mdlPallet>();

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT ID,RFID,READTIME,LOCATION,READERNAME,TYPE,STYLE FROM VRFID_LOGS WHERE LOCATIONID=@LOCATIONID ";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("LOCATIONID", SqlDbType.VarChar).Value = id;

                SqlDataReader dr = cmd.ExecuteReader();
                mdlPallet list = new mdlPallet();
                while (dr.Read())
                {
                    list = new mdlPallet();
                    list.ID = dr["ID"].ToString();
                    list.RFID = dr["RFID"].ToString();
                    list.READTIME = Convert.ToDateTime(dr["READTIME"].ToString()).ToString("dd-MMM-yyyy H:mm:ss tt");
                    list.LOCATION = dr["LOCATION"].ToString();
                    list.READERNAME = dr["READERNAME"].ToString();
                    list.TYPE = dr["TYPE"].ToString();
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
