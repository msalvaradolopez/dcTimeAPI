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
    public class conexiones
    {
        private string strConn;
        public conexiones() {
            strConn = ConfigurationManager.ConnectionStrings["sqlServer"].ConnectionString;
        }

        public List<pedidosDB> getPedidos() {
            DateTime ldtFechaActual = DateTime.Now;

            List<pedidosDB> lPedidosDB = new List<pedidosDB>();

            using (SqlConnection conn = new SqlConnection(strConn)) {
                SqlCommand sql = new SqlCommand("select *,t2.firstName from dcPEDIDOS t1 " +
                    "                               left join dcSURTIDOR t2 on t2.empID = t1.empID " +
                    "                           where CONVERT(VARCHAR(10),fecha, 112) = CONVERT(VARCHAR(10), @fechaActual, 112)", conn);

                sql.CommandType = CommandType.Text;
                sql.Parameters.AddWithValue("@fechaActual", ldtFechaActual);

                conn.Open();
                SqlDataReader resultado = sql.ExecuteReader();

                while (resultado.Read()) {
                    string folio = resultado["folio"] as string;
                    string socio = resultado["Socio"] as string;
                    string estatus = resultado["estatus"] as string;
                    string fecha =  Convert.ToDateTime(resultado["fecha"]).ToString("yyyy/MM/dd HH:mm");
                    string slpname = resultado["SlpName"] as string;
                    string fechaSurtiendo = resultado["fechaSurtiendo"] == DBNull.Value ? "" : Convert.ToDateTime(resultado["fechaSurtiendo"]).ToString("yyyy/MM/dd HH:mm");
                    string fechaCerrado = resultado["fechaCerrado"] == DBNull.Value ? "" : Convert.ToDateTime(resultado["fechaCerrado"]).ToString("yyyy/MM/dd HH:mm");
                    string empid = Convert.ToString(resultado["empID"]);
                    string nomSurtidor = resultado["firstName"] as string;
                    string foto = resultado["foto"] as string;
                    lPedidosDB.Add(new pedidosDB(folio, socio, estatus, fecha, slpname, fechaSurtiendo, fechaCerrado, empid, nomSurtidor, foto));
                }

                resultado.Close();
            }

            return lPedidosDB; 
        }

        public string setSurtiendo(IFiltros filtros)
        {
            DateTime ldtFechaActual = DateTime.Now;

            List<pedidosDB> lPedidosDB = new List<pedidosDB>();

            using (SqlConnection conn = new SqlConnection(strConn))
            {
                SqlCommand sql = new SqlCommand("update dcPEDIDOS set fechaSurtiendo = @fechaSurtiendo, " + 
                                                "                     estatus = '2', " +
                                                "                       empID = @empID " +
                                                "where folio = @folio ", conn);

                sql.CommandType = CommandType.Text;
                sql.Parameters.AddWithValue("@fechaSurtiendo", ldtFechaActual);
                sql.Parameters.AddWithValue("@empID", filtros.empID);
                sql.Parameters.AddWithValue("@folio", filtros.folio);

                conn.Open();
                sql.ExecuteNonQuery();
                return "Surtidor asignado.";
            }
        }

        public List<surtidoresDB> GetSurtidores()
        {
            DateTime ldtFechaActual = DateTime.Now;

            List<surtidoresDB> lSurtidoresDB = new List<surtidoresDB>();

            using (SqlConnection conn = new SqlConnection(strConn))
            {
                SqlCommand sql = new SqlCommand("select * from dcSURTIDOR where estatus = 'A'", conn);

                sql.CommandType = CommandType.Text;

                conn.Open();
                SqlDataReader resultado = sql.ExecuteReader();

                while (resultado.Read())
                {
                    string empid = Convert.ToString(resultado["empID"]);
                    string lastname = resultado["lastName"] as string;
                    string firstname = resultado["firstName"] as string;
                    string middlename = resultado["middleName"] as string;
                    string foto = resultado["foto"] as string;

                    lSurtidoresDB.Add(new surtidoresDB(empid, lastname, firstname, middlename, foto));
                }

                resultado.Close();
            }

            return lSurtidoresDB;
        }

        public string setCerrado(IFiltros filtros)
        {
            DateTime ldtFechaActual = DateTime.Now;

            List<pedidosDB> lPedidosDB = new List<pedidosDB>();

            using (SqlConnection conn = new SqlConnection(strConn))
            {
                SqlCommand sql = new SqlCommand("update dcPEDIDOS set fechaCerrado = @fechaCerrado, " +
                                                "                     estatus = '3', " +
                                                "                       empID = @empID " +
                                                "where folio = @folio ", conn);

                sql.CommandType = CommandType.Text;
                sql.Parameters.AddWithValue("@fechaCerrado", ldtFechaActual);
                sql.Parameters.AddWithValue("@empID", filtros.empID);
                sql.Parameters.AddWithValue("@folio", filtros.folio);

                conn.Open();
                sql.ExecuteNonQuery();
                return "Surtimiento Cerrado.";
            }
        }

        public List<bannerDB> getBanners()
        {

            List<bannerDB> lBannersDB = new List<bannerDB>();

            using (SqlConnection conn = new SqlConnection(strConn))
            {
                SqlCommand sql = new SqlCommand("select * from dcBANNER", conn);

                sql.CommandType = CommandType.Text;

                conn.Open();
                SqlDataReader resultado = sql.ExecuteReader();

                while (resultado.Read())
                {
                    string lBannerID = resultado["bannerID"] as string;
                    string lTexto = resultado["texto"] as string;

                    lBannersDB.Add(new bannerDB(lBannerID, lTexto));
                }

                resultado.Close();
            }

            return lBannersDB;
        }

        public List<bannerDB> getConPorDias(IFiltros filtros)
        {

            List<conPorDias> lconPorDias = new List<conPorDias>();

            using (SqlConnection conn = new SqlConnection(strConn))
            {
                SqlCommand sql = new SqlCommand("SELECT fechaOrigen = CAST(fecha AS DATE) ,porSurtir = AVG( DATEDIFF(mi, fecha, fechaSurtiendo)), surtiendo = AVG(DATEDIFF(MI, fechaSurtiendo, fechaCerrado)) " +
                                                    " FROM dcPEDIDOS " +
                                                    " WHERE estatus > 1 " +
                                                    " AND DATEPART(YEAR, fecha) = @ANNIO " +
                                                    " AND DATEPART(MONTH, fecha) = @MES " +
                                                    " GROUP BY CAST(fecha AS DATE)", conn);

                sql.CommandType = CommandType.Text;
                sql.Parameters.AddWithValue("@ANNIO", filtros.annio);
                sql.Parameters.AddWithValue("@MES", filtros.mes);

                conn.Open();
                SqlDataReader resultado = sql.ExecuteReader();

                while (resultado.Read())
                {
                    string lfechaOrigen = resultado["fechaOrigen"] as string;
                    string lporsurtir = resultado["porSurtir"] as string;
                    string lsurtiendo = resultado["surtiendo"] as string;

                    lconPorDias.Add(new conPorDias(lfechaOrigen, lporsurtir, lsurtiendo));
                }

                resultado.Close();
            }

            return lBannersDB;
        }

    }
}