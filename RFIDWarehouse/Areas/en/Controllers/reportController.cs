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
    public class reportController : Controller
    {
        //
        // GET: /en/report/
        public ActionResult checklocations()
        {
            return View();
        }

        public ActionResult palletlocation()
        {
            return View();
        }

        [HttpGet]
        public ActionResult searchRFIDLocation(string id)
        {

            List<mdlPallet> model = new List<mdlPallet>();

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT ID,RFID,READTIME,LOCATION,READERNAME FROM VRFID_LOGS WHERE LOCATIONID=@LOCATIONID ";

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