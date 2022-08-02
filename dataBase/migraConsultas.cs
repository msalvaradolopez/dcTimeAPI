using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using dcTimeAPI.Models;

namespace dcTimeAPI.dataBase
{
    public class migraConsultas
    {

        private string strConn;
        private string strConnExt;
        public migraConsultas()
        {
            strConn = ConfigurationManager.ConnectionStrings["sqlServer"].ConnectionString;
            strConnExt = ConfigurationManager.ConnectionStrings["sqlServerExt"].ConnectionString;
        }

        public void migSurtidores() {
            List<surtidoresDB> lSurtidoresExt = new List<surtidoresDB>();
            lSurtidoresExt = getSurtidoresExt();

            lSurtidoresExt.ForEach(rowExt =>
            {
                using (SqlConnection conn = new SqlConnection(strConn))
                {
                    SqlCommand sql = new SqlCommand("INSERT INTO dcSURTIDOR(empID, estatus, foto)" +
                                                        " VALUES(@empID, @estatus, @foto)", conn);

                    sql.CommandType = CommandType.Text;
                    sql.Parameters.AddWithValue("@empID", rowExt.empID);
                    sql.Parameters.AddWithValue("@estatus", 'A');
                    sql.Parameters.AddWithValue("@foto", rowExt.Foto);

                    conn.Open();
                    sql.ExecuteNonQuery();
                }

            });
        }

        private List<surtidoresDB> getSurtidoresExt() {
            List<surtidoresDB> lSurtidoresExt = new List<surtidoresDB>();

            using (SqlConnection conn = new SqlConnection(strConnExt))
            {
                SqlCommand sql = new SqlCommand("SELECT empID, foto FROM dcSURTIDOR WHERE estatus = 'A' ", conn);

                sql.CommandType = CommandType.Text;

                conn.Open();
                SqlDataReader resultado = sql.ExecuteReader();

                while (resultado.Read())
                {
                    string empID = Convert.ToString( resultado["empID"] );
                    string foto = resultado["foto"] as string;
                    lSurtidoresExt.Add(new surtidoresDB(empID, null, null, null, foto));
                }

                resultado.Close();
            }

            return lSurtidoresExt;
        }

        #region METODOS PARA INTEGRAR LOS PEDIDOS DE R1 AL SERVIDOR DE SAP.

        public List<tablaPedidosDB> getTablaPedidosExt()
        {
            DateTime ldtFechaActual = DateTime.Now;
            // ldtFechaActual = ldtFechaActual.AddDays(-1);

            List<tablaPedidosDB> lPedidosDB = new List<tablaPedidosDB>();

            using (SqlConnection conn = new SqlConnection(strConnExt))
            {
                SqlCommand sql = new SqlCommand("SELECT V.Name [FOLIO], " +
                                                " OCRD.CardName[SOCIO], " +
                                                " CONVERT(DATETIME, SUBSTRING(CONVERT(VARCHAR(10), V.U_SO1_FECHA, 103), 1, 10) + ' ' + " +
                                                " SUBSTRING(RIGHT('0000' + CAST(V.U_SO1_HORA AS VARCHAR(4)), 4), 1, 2) + ':' + " +
                                                " SUBSTRING(RIGHT('0000' + CAST(V.U_SO1_HORA AS VARCHAR(4)), 4), 3, 2), 103)[FECHA], " +
                                                " OSLP.SlpName[VENDEDOR], " +
                                                " '1' [ESTATUS] " +
                                                " FROM dbo.[@SO1_01VENTA] V WITH(NOLOCK) " +
                                                " JOIN dbo.OSLP WITH(NOLOCK) ON OSLP.SlpCode = V.U_SO1_VENDEDOR " +
                                                " JOIN dbo.OCRD WITH(NOLOCK) ON OCRD.CardCode = V.U_SO1_CLIENTE " +
                                                " WHERE CONVERT(VARCHAR(10), V.U_SO1_FECHA, 112) = CONVERT(VARCHAR(10), @fechaActual, 112) " +
                                                " AND V.U_SO1_TIPO = 'PE' " +
                                                " AND(UPPER(V.U_SO1_COMENTARIO) LIKE '%MOSTRADOR%' OR V.U_SO1_COMENTARIO LIKE '%CAJA%') ", conn);

                sql.CommandType = CommandType.Text;
                sql.Parameters.AddWithValue("@fechaActual", ldtFechaActual);

                conn.Open();
                SqlDataReader resultado = sql.ExecuteReader();

                while (resultado.Read())
                {
                    string folio = resultado["FOLIO"] as string;
                    string socio = resultado["SOCIO"] as string;
                    string fecha = Convert.ToDateTime(resultado["FECHA"]).ToString("yyyy/MM/dd HH:mm");
                    string vendedor = resultado["VENDEDOR"] as string;
                    string estatus = resultado["ESTATUS"] as string;
                    lPedidosDB.Add(new tablaPedidosDB(folio, socio, fecha, vendedor, estatus));
                }

                resultado.Close();
            }

            return lPedidosDB;
        }

        public List<tablaPedidosDB> getTablaPedidosLocal()
        {
            DateTime ldtFechaActual = DateTime.Now;
            // ldtFechaActual = ldtFechaActual.AddDays(-1);

            List<tablaPedidosDB> lPedidosDB = new List<tablaPedidosDB>();

            using (SqlConnection conn = new SqlConnection(strConn))
            {
                SqlCommand sql = new SqlCommand("SELECT folio [FOLIO], Socio [SOCIO], fecha [FECHA], slpname [VENDEDOR], estatus [ESTATUS]" +
                                                " FROM dcPEDIDOS " +
                                                " WHERE CONVERT(VARCHAR(10),fecha, 112) = CONVERT(VARCHAR(10), @fechaActual, 112)", conn);

                sql.CommandType = CommandType.Text;
                sql.Parameters.AddWithValue("@fechaActual", ldtFechaActual);

                conn.Open();
                SqlDataReader resultado = sql.ExecuteReader();

                while (resultado.Read())
                {
                    string folio = resultado["FOLIO"] as string;
                    string socio = resultado["SOCIO"] as string;
                    string fecha = Convert.ToDateTime(resultado["FECHA"]).ToString("yyyy/MM/dd HH:mm");
                    string vendedor = resultado["VENDEDOR"] as string;
                    string estatus = resultado["ESTATUS"] as string;
                    lPedidosDB.Add(new tablaPedidosDB(folio, socio, fecha, vendedor, estatus));
                }

                resultado.Close();
            }

            return lPedidosDB;
        }

        private void setTablasPedidos()
        {

            DateTime ldtFechaActual = DateTime.Now;

            List<tablaPedidosDB> lTablaPedidoExt = new List<tablaPedidosDB>();
            List<tablaPedidosDB> lTablaPedidoLocal = new List<tablaPedidosDB>();


            lTablaPedidoExt = getTablaPedidosExt();
            lTablaPedidoLocal = getTablaPedidosLocal();


            lTablaPedidoExt.ForEach(rowExt =>
            {

                bool lbRowExtSiNo = lTablaPedidoLocal.Any(x => x.FOLIO == rowExt.FOLIO);
                if (!lbRowExtSiNo)
                {
                    using (SqlConnection conn = new SqlConnection(strConn))
                    {
                        SqlCommand sql = new SqlCommand("INSERT INTO dcPEDIDOS(folio, Socio, fecha, SlpName, estatus)" +
                            " VALUES(@FOLIO, @SOCIO, @FECHA, @VENDEDOR, @ESTATUS)", conn);

                        sql.CommandType = CommandType.Text;
                        sql.Parameters.AddWithValue("@FOLIO", rowExt.FOLIO);
                        sql.Parameters.AddWithValue("@SOCIO", rowExt.SOCIO);
                        sql.Parameters.AddWithValue("@FECHA", ldtFechaActual);
                        sql.Parameters.AddWithValue("@VENDEDOR", rowExt.VENDEDOR);
                        sql.Parameters.AddWithValue("@ESTATUS", "1");

                        conn.Open();
                        sql.ExecuteNonQuery();
                    }
                }
            });
        }

        public List<tablaSurtidoresExt> getTablaSurtidoresExt()
        {
            DateTime ldtFechaActual = DateTime.Now;

            List<tablaSurtidoresExt> lSurtidorDB = new List<tablaSurtidoresExt>();

            using (SqlConnection conn = new SqlConnection(strConnExt))
            {
                SqlCommand sql = new SqlCommand(" SELECT empID, lastName, firstName, middleName " +
                                                  " FROM dbo.OHEM T1 WITH(NOLOCK) " +
                                                  " WHERE position = 32 and branch = 1 ", conn);

                sql.CommandType = CommandType.Text;

                conn.Open();
                SqlDataReader resultado = sql.ExecuteReader();

                while (resultado.Read())
                {
                    string empID = resultado["empID"] as string;
                    string lastName = resultado["lastName"] as string;
                    string fisrtName = resultado["firstName"] as string;
                    string middleName = resultado["middleName"] as string;
                    string estatus = "A";
                    lSurtidorDB.Add(new tablaSurtidoresExt(empID, lastName, fisrtName, middleName, estatus));
                }

                resultado.Close();
            }

            return lSurtidorDB;
        }

        public List<tablaSurtidoresExt> getTablaSurtidoresLocal()
        {
            DateTime ldtFechaActual = DateTime.Now;

            List<tablaSurtidoresExt> lSurtidorDB = new List<tablaSurtidoresExt>();

            using (SqlConnection conn = new SqlConnection(strConn))
            {
                SqlCommand sql = new SqlCommand(" SELECT empID, lastName, firstName, middleName " +
                                                  " FROM dcSURTIDOR T1 WITH(NOLOCK) " +
                                                  " WHERE estatus = 'A' ", conn);

                sql.CommandType = CommandType.Text;

                conn.Open();
                SqlDataReader resultado = sql.ExecuteReader();

                while (resultado.Read())
                {
                    string empID = resultado["empID"] as string;
                    string lastName = resultado["lastName"] as string;
                    string fisrtName = resultado["firstName"] as string;
                    string middleName = resultado["middleName"] as string;
                    string estatus = "A";
                    lSurtidorDB.Add(new tablaSurtidoresExt(empID, lastName, fisrtName, middleName, estatus));
                }

                resultado.Close();
            }

            return lSurtidorDB;
        }

        private void setTablasSurtidores()
        {

            DateTime ldtFechaActual = DateTime.Now;

            List<tablaSurtidoresExt> lTablaSurtidoresExt = new List<tablaSurtidoresExt>();
            List<tablaSurtidoresExt> lTablaSurtidoresLocal = new List<tablaSurtidoresExt>();


            lTablaSurtidoresExt = getTablaSurtidoresExt();
            lTablaSurtidoresLocal = getTablaSurtidoresLocal();


            lTablaSurtidoresExt.ForEach(rowExt =>
            {

                bool lbRowExtSiNo = lTablaSurtidoresLocal.Any(x => x.EMPID == rowExt.EMPID);
                if (!lbRowExtSiNo)
                {
                    using (SqlConnection conn = new SqlConnection(strConn))
                    {
                        SqlCommand sql = new SqlCommand("insert into dcSURTIDOR( empID, lastName, firstName, middleName, estatus)" +
                            " VALUES(@EMPID, @LASTNAME, @FIRSTNAME, @MIDDLENAME, @ESTATUS)", conn);

                        sql.CommandType = CommandType.Text;
                        sql.Parameters.AddWithValue("@EMPID", rowExt.EMPID);
                        sql.Parameters.AddWithValue("@LASTNAME", rowExt.LASTNAME);
                        sql.Parameters.AddWithValue("@FISRTNAME", rowExt.FISRTNAME);
                        sql.Parameters.AddWithValue("@MIDDLENAME", rowExt.MIDDLENAME);
                        sql.Parameters.AddWithValue("@ESTATUS", "A");

                        conn.Open();
                        sql.ExecuteNonQuery();
                    }
                }
            });

            lTablaSurtidoresLocal.ForEach(rowExt =>
            {

                bool lbRowExtSiNo = lTablaSurtidoresExt.Any(x => x.EMPID == rowExt.EMPID);
                if (!lbRowExtSiNo)
                {
                    using (SqlConnection conn = new SqlConnection(strConn))
                    {
                        SqlCommand sql = new SqlCommand("UPDATE dcSURTIDOR" +
                                                        " SET estatus = @ESTATUS)" +
                                                        " WHERE empID = @EMPID", conn);

                        sql.CommandType = CommandType.Text;
                        sql.Parameters.AddWithValue("@ESTATUS", "B");

                        conn.Open();
                        sql.ExecuteNonQuery();
                    }
                }
            });
        }

        #endregion
    }
}