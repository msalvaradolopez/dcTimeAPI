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
                SqlCommand sql = new SqlCommand("select *,t2.firstName from dcPEDIDOS t1  WITH (NOLOCK) " +
                    "                               left join dcSURTIDOR t2  WITH (NOLOCK) on t2.empID = t1.empID " +
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
                    string lastName = resultado["lastName"] as string;
                    string foto = resultado["foto"] as string;
                    lPedidosDB.Add(new pedidosDB(folio, socio, estatus, fecha, slpname, fechaSurtiendo, fechaCerrado, empid, nomSurtidor + " " + lastName , foto));
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
                SqlCommand sql = new SqlCommand("select * from dcSURTIDOR  WITH (NOLOCK) where estatus = 'A'", conn);

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
                SqlCommand sql = new SqlCommand("select * from dcBANNER  WITH (NOLOCK) ", conn);

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

        public string insBanner(IFiltros filtros)
        {

            List<bannerDB> lbannerDB = new List<bannerDB>();

            using (SqlConnection conn = new SqlConnection(strConn))
            {
                SqlCommand sql = new SqlCommand("INSERT INTO dcBANNER(bannerID, texto) " +
                                                " VALUES(@bannerID, @texto) ", conn);

                sql.CommandType = CommandType.Text;
                sql.Parameters.AddWithValue("@bannerID", filtros.bannerID);
                sql.Parameters.AddWithValue("@texto", filtros.texto);

                conn.Open();
                sql.ExecuteNonQuery();
                return "Texto ingresado.";
            }
        }

        public string setBanner(IFiltros filtros)
        {

            List<bannerDB> lbannerDB = new List<bannerDB>();

            using (SqlConnection conn = new SqlConnection(strConn))
            {
                SqlCommand sql = new SqlCommand("update dcBANNER set texto= @texto " +
                                                "where bannerID = @bannerID ", conn);

                sql.CommandType = CommandType.Text;
                sql.Parameters.AddWithValue("@bannerID", filtros.bannerID);
                sql.Parameters.AddWithValue("@texto", filtros.texto);

                conn.Open();
                sql.ExecuteNonQuery();
                return "Texto actualizado.";
            }
        }

        public string delBanner(IFiltros filtros)
        {

            List<bannerDB> lbannerDB = new List<bannerDB>();

            using (SqlConnection conn = new SqlConnection(strConn))
            {
                SqlCommand sql = new SqlCommand("delete dcBANNER " +
                                                "where bannerID = @bannerID ", conn);

                sql.CommandType = CommandType.Text;
                sql.Parameters.AddWithValue("@bannerID", filtros.bannerID);

                conn.Open();
                sql.ExecuteNonQuery();
                return "Texto eliminado.";
            }
        }

        public List<conPorDias> getConPorDias(IFiltros filtros)
        {

            List<conPorDias> lconPorDias = new List<conPorDias>();

            using (SqlConnection conn = new SqlConnection(strConn))
            {
                SqlCommand sql = new SqlCommand("SELECT fechaOrigen = CAST(fecha AS DATE) ,porSurtir = ISNULL(AVG( DATEDIFF(mi, fecha, fechaSurtiendo)), 0), surtiendo = ISNULL(AVG(DATEDIFF(MI, fechaSurtiendo, fechaCerrado)), 0) " +
                                                    " FROM dcPEDIDOS  WITH (NOLOCK) " +
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
                    string lfechaOrigen = Convert.ToDateTime(resultado["fechaOrigen"]).ToString("yyyy/MM/dd HH:mm");
                    string lporsurtir = Convert.ToInt16(resultado["porSurtir"]).ToString();
                    string lsurtiendo = Convert.ToInt16(resultado["surtiendo"]).ToString();

                    lconPorDias.Add(new conPorDias(lfechaOrigen, lporsurtir, lsurtiendo));
                }

                resultado.Close();
            }

            return lconPorDias;
        }

        public List<conPorSurtidor> getConPorSurtidor(IFiltros filtros)
        {

            List<conPorSurtidor> lconPorSurtidor = new List<conPorSurtidor>();

            using (SqlConnection conn = new SqlConnection(strConn))
            {
                SqlCommand sql = new SqlCommand("SELECT empID = MAX(dcPEDIDOS.empID) ,surtidor = dcSURTIDOR.firstName, porSurtir = ISNULL(AVG( DATEDIFF(mi, fecha, fechaSurtiendo)), 0), surtiendo = ISNULL(AVG(DATEDIFF(MI, fechaSurtiendo, fechaCerrado)), 0) " +
                                                    " FROM dcPEDIDOS  WITH (NOLOCK) INNER JOIN dcSURTIDOR  WITH (NOLOCK) ON dcSURTIDOR.empID = dcPEDIDOS.empID " +
                                                    " WHERE dcPEDIDOS.estatus > 1 " +
                                                    " AND DATEPART(YEAR, fecha) = @ANNIO " +
                                                    " AND DATEPART(MONTH, fecha) = @MES " +
                                                    " GROUP BY dcSURTIDOR.firstName", conn);

                sql.CommandType = CommandType.Text;
                sql.Parameters.AddWithValue("@ANNIO", filtros.annio);
                sql.Parameters.AddWithValue("@MES", filtros.mes);

                conn.Open();
                SqlDataReader resultado = sql.ExecuteReader();

                while (resultado.Read())
                {
                    string lSurtidor = resultado["surtidor"] as string;
                    string lporsurtir = Convert.ToInt16(resultado["porSurtir"]).ToString();
                    string lsurtiendo = Convert.ToInt16(resultado["surtiendo"]).ToString();
                    string lempID = Convert.ToInt16(resultado["empID"]).ToString();

                    lconPorSurtidor.Add(new conPorSurtidor(lSurtidor, lporsurtir, lsurtiendo, lempID));
                }

                resultado.Close();
            }

            return lconPorSurtidor;
        }

        public List<conGenerales> getConGeneral(IFiltros filtros)
        {

            List<conGenerales> lconGenerales = new List<conGenerales>();

            using (SqlConnection conn = new SqlConnection(strConn))
            {
                SqlCommand sql = new SqlCommand("SELECT dcPEDIDOS.folio, " +
                                                "statusText = CASE WHEN dcPEDIDOS.estatus = '2' THEN 'Surtiendo' " +
                                                                  " WHEN dcPEDIDOS.estatus = '3' THEN 'Cerrado' END, " +
                                                " dcPEDIDOS.fecha, " +
                                                " porSurtir = ISNULL(DATEDIFF(mi, fecha, fechaSurtiendo), 0), " +
                                                " dcPEDIDOS.fechaSurtiendo, " +
                                                " surtiendo = ISNULL(DATEDIFF(mi, fechaSurtiendo, fechaCerrado), 0), " +
                                                " dcPEDIDOS.fechaCerrado, " +
                                                " dcSURTIDOR.firstName, " +
                                                " dcPEDIDOS.socio, " +
                                                " dcPEDIDOS.SlpName " +
                                                " FROM dcPEDIDOS  WITH (NOLOCK) INNER JOIN dcSURTIDOR  WITH (NOLOCK) ON dcSURTIDOR.empID = dcPEDIDOS.empID " +
                                                " WHERE dcPEDIDOS.estatus > 1 " +
                                                " AND DATEPART(YEAR, fecha) = @ANNIO " +
                                                " AND DATEPART(MONTH, fecha) = @MES " +
                                                " AND (DATEPART(DAY, fecha) = @DIA OR @DIA = 0)" +
                                                " AND (dcPEDIDOS.empID = @empID OR @empID = 0) ", conn);

                sql.CommandType = CommandType.Text;
                sql.Parameters.AddWithValue("@ANNIO", filtros.annio);
                sql.Parameters.AddWithValue("@MES", filtros.mes);
                sql.Parameters.AddWithValue("@DIA", filtros.dia);
                sql.Parameters.AddWithValue("@empID", filtros.empID);

                conn.Open();
                SqlDataReader resultado = sql.ExecuteReader();

                while (resultado.Read())
                {
                    string lfolio = resultado["folio"] as string;
                    string lstatusText = resultado["statusText"] as string;
                    string lfecha = Convert.ToDateTime(resultado["fecha"]).ToString("yyyy/MM/dd HH:mm");
                    string lporSurtir = Convert.ToInt16(resultado["porSurtir"]).ToString();
                    string lfechaSurtiendo = resultado["fechaSurtiendo"] == DBNull.Value ? null : Convert.ToDateTime(resultado["fechaSurtiendo"]).ToString("yyyy/MM/dd HH:mm");
                    string lsurtiendo = Convert.ToInt16(resultado["surtiendo"]).ToString();
                    string lfechaCerrado = resultado["fechaCerrado"] == DBNull.Value ? null : Convert.ToDateTime(resultado["fechaCerrado"]).ToString("yyyy/MM/dd HH:mm");
                    string lfirstName = resultado["firstName"] as string;
                    string lsocio = resultado["socio"] as string;
                    string lSlpName = resultado["SlpName"] as string;

                    lconGenerales.Add(new conGenerales(lfolio, lstatusText, lfecha, lporSurtir, lfechaSurtiendo, lsurtiendo, lfechaCerrado, lfirstName, lsocio, lSlpName));
                }

                resultado.Close();
            }

            return lconGenerales;
        }

        public string setFoto(IFiltros filtros)
        {

            List<bannerDB> lbannerDB = new List<bannerDB>();

            using (SqlConnection conn = new SqlConnection(strConn))
            {
                SqlCommand sql = new SqlCommand("update dcSURTIDOR set foto = @foto " +
                                                "where empID = @empID ", conn);

                sql.CommandType = CommandType.Text;
                sql.Parameters.AddWithValue("@foto", filtros.Foto);
                sql.Parameters.AddWithValue("@empID", filtros.empID);

                conn.Open();
                sql.ExecuteNonQuery();
                return "Foto actualizado.";
            }
        }

        public string getParam()
        {
            string lstrVideo = "";
            List<paramDB> lParamDB = new List<paramDB>();

            using (SqlConnection conn = new SqlConnection(strConn))
            {
                SqlCommand sql = new SqlCommand("select * from dcPARAM  WITH (NOLOCK)", conn);

                sql.CommandType = CommandType.Text;

                conn.Open();
                SqlDataReader resultado = sql.ExecuteReader();

                while (resultado.Read())
                {
                    string lVideo = resultado["video"] as string;

                    lstrVideo = lVideo;
                }

                resultado.Close();
            }

            return lstrVideo;
        }

        public string setParam(IFiltros pFiltros)
        {
            string lstrVideo = "";
            List<paramDB> lParamDB = new List<paramDB>();

            using (SqlConnection conn = new SqlConnection(strConn))
            {
                SqlCommand sql = new SqlCommand("UPDATE dcPARAM SET video = @video", conn);

                sql.CommandType = CommandType.Text;
                sql.Parameters.AddWithValue("@video", pFiltros.Video);

                conn.Open();
                sql.ExecuteNonQuery();
                return "Foto actualizado.";
            }
        }
    }
}