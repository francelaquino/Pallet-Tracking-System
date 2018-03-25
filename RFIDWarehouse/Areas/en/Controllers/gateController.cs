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
    public class gateController : Controller
    {
        //
        // GET: /en/gate/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult newGate()
        {
            return View();
        }
        [HttpPost]
        public string saveGate(mdlGate gateinfo)
        {

            string result = "";

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT ID FROM GATES WHERE GATE=@GATE";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("GATE", SqlDbType.VarChar).Value = gateinfo.GATE;

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    result = "Gate already exist";
                }
                else
                {
                    dr.Dispose();
                    cmd.Dispose();

                    strSQL = @"INSERT INTO GATES(GATE,ENCODEDBY,MODIFIEDBY,DATEMODIFIED,DATEENCODED,ACTIVE,MODE)
                    VALUES(@GATE,@ENCODEDBY,@MODIFIEDBY,GETDATE(),GETDATE(),@ACTIVE,@MODE)";




                    cmd = new SqlCommand(strSQL, dbconn.DbConn);
                    cmd.Parameters.Add("GATE", SqlDbType.VarChar).Value = gateinfo.GATE;
                    cmd.Parameters.Add("ENCODEDBY", SqlDbType.VarChar).Value = "currentuser";
                    cmd.Parameters.Add("MODIFIEDBY", SqlDbType.VarChar).Value = "currentuser";
                    cmd.Parameters.Add("ACTIVE", SqlDbType.VarChar).Value = gateinfo.ACTIVE;
                    cmd.Parameters.Add("MODE", SqlDbType.VarChar).Value = gateinfo.MODE;
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

        public ActionResult Gate()
        {
            return View();

        }

        [HttpGet]
        public ActionResult getGates()
        {

            List<mdlGate> model = new List<mdlGate>();

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT ID,GATE,MODE,ACTIVE FROM VGATES";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);

                SqlDataReader dr = cmd.ExecuteReader();
                mdlGate list = new mdlGate();
                while (dr.Read())
                {
                    list = new mdlGate();
                    list.ID = dr["ID"].ToString();
                    list.GATE = dr["GATE"].ToString();
                    list.ACTIVE = dr["ACTIVE"].ToString();
                    list.MODE = dr["MODE"].ToString();
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


        public ActionResult getActiveGates()
        {

            List<mdlGate> model = new List<mdlGate>();

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT ID,GATE FROM GATES WHERE ACTIVE='A'";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);

                SqlDataReader dr = cmd.ExecuteReader();
                mdlGate list = new mdlGate();
                while (dr.Read())
                {
                    list = new mdlGate();
                    list.ID = dr["ID"].ToString();
                    list.GATE = dr["GATE"].ToString();
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

        public ActionResult editGate()
        {
            return View();
        }

        [HttpGet]
        public ActionResult getGateInfo(string id)
        {
            mdlGate model = new mdlGate();

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT * FROM GATES WHERE ID=@ID";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = id;

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    dr.Read();
                    model.ID = dr["ID"].ToString();
                    model.GATE = dr["GATE"].ToString();
                    model.MODE = dr["MODE"].ToString();
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
        public string updateGate(mdlGate gateinfo)
        {

            string result = "";

            Db_Connection dbconn = new Db_Connection();
            try
            {

                dbconn.openConnection();


                string strSQL = @"SELECT ID FROM GATES WHERE GATE=@GATE";

                SqlCommand cmd = new SqlCommand(strSQL, dbconn.DbConn);
                cmd.Parameters.Add("GATE", SqlDbType.VarChar).Value = gateinfo.GATE;

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    if (dr["ID"].ToString() != gateinfo.ID)
                    {
                        result = "Gate already exist";
                    }

                }

                dr.Dispose();
                cmd.Dispose();

                if (result == "")
                {


                    strSQL = @"UPDATE GATES SET GATE=@GATE,ACTIVE=@ACTIVE,
                    MODIFIEDBY=@MODIFIEDBY,DATEMODIFIED=GETDATE(),MODE=@MODE WHERE ID=@ID";

                    cmd = new SqlCommand(strSQL, dbconn.DbConn);
                    cmd.Parameters.Add("GATE", SqlDbType.VarChar).Value = gateinfo.GATE;
                    cmd.Parameters.Add("ACTIVE", SqlDbType.VarChar).Value = gateinfo.ACTIVE;
                    cmd.Parameters.Add("MODIFIEDBY", SqlDbType.VarChar).Value = "currentuser";
                    cmd.Parameters.Add("MODE", SqlDbType.VarChar).Value = gateinfo.MODE;
                    cmd.Parameters.Add("ID", SqlDbType.VarChar).Value = gateinfo.ID;
                    cmd.ExecuteNonQuery();

                    dr.Dispose();
                    cmd.Dispose();

                    result = "Gate successfully updated";


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