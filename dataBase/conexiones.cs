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
        private string strConnExt;
        public conexiones() {
            strConn = ConfigurationManager.ConnectionStrings["sqlServer"].ConnectionString;
        }

        public List<pedidosDB> getPedidos(IFiltros pFiltros) {
            DateTime ldtFechaActual = DateTime.Now;
            // ldtFechaActual = ldtFechaActual.AddDays(-2);

            // setTablasPedidos();
            // setTablasSurtidores();

            List<pedidosDB> lPedidosDB = new List<pedidosDB>();

            using (SqlConnection conn = new SqlConnection(strConn)) {

                try
                {
                    SqlCommand sql = new SqlCommand("SELECT V.Name [folio], " +
                                                         " OCRD.CardName[Socio], " +
                                                         " estatus = '1', " +
                                                         " CONVERT(DATETIME, SUBSTRING(CONVERT(VARCHAR(10), V.U_SO1_FECHA, 103), 1, 10) + ' ' + " +
                                                         " SUBSTRING(RIGHT('0000' + CAST(V.U_SO1_HORA AS VARCHAR(4)), 4), 1, 2) + ':' + " +
                                                         " SUBSTRING(RIGHT('0000' + CAST(V.U_SO1_HORA AS VARCHAR(4)), 4), 3, 2), 103)[fecha], " +
                                                         " OSLP.SlpName[SlpName], " +
                                                         " fechaSurtiendo = NULL, " +
                                                         " fechaEntregado = NULL, " +
                                                         " fechaCerrado = NULL, " +
                                                         " empID = NULL, " +
                                                         " firstName = NULL, " +
                                                         " lastName = NULL, " +
                                                         " foto = NULL " +
                                                         " FROM dbo.[@SO1_01VENTA] V WITH(NOLOCK) " +
                                                               " JOIN dbo.OSLP WITH(NOLOCK) ON OSLP.SlpCode = V.U_SO1_VENDEDOR " +
                                                               " JOIN dbo.OCRD WITH(NOLOCK) ON OCRD.CardCode = V.U_SO1_CLIENTE " +
                                                         " WHERE CONVERT(VARCHAR(10), V.U_SO1_FECHA, 112) = CONVERT(VARCHAR(10), @fechaActual, 112) " +
                                                               " AND V.U_SO1_TIPO = 'PE' " +
                                                               " AND NOT EXISTS(SELECT 1 FROM dcPEDIDOS T1 WHERE T1.folio = V.Name) " +
                                                               " AND(UPPER(V.U_SO1_COMENTARIO) LIKE '%MOSTRADOR%' OR V.U_SO1_COMENTARIO LIKE '%CAJA%') " +
                                                        " UNION ALL " +
                                                        " SELECT folio, " +
                                                                " Socio, " +
                                                                " estatus = dcPEDIDOS.estatus, " +
                                                                " fecha, " +
                                                                " SlpName, " +
                                                                " fechaSurtiendo, " +
                                                                " fechaEntregado, " +
                                                                " fechaCerrado, " +
                                                                " empID = dcPEDIDOS.empID, " +
                                                                " firstName = OHEM.firstName, " +
                                                                " lastName = OHEM.lastName, " +
                                                                " foto = dcSURTIDOR.foto" +
                                                        " FROM dcPEDIDOS INNER JOIN dbo.OHEM WITH(NOLOCK) ON OHEM.empID = dcPEDIDOS.empID" +
                                                                        " LEFT JOIN dcSURTIDOR WITH(NOLOCK) ON dcSURTIDOR.empID = dcPEDIDOS.empID" +
                                                        " WHERE CONVERT(VARCHAR(10), fecha, 112) = CONVERT(VARCHAR(10), @fechaActual, 112)  ", conn);

                    sql.CommandType = CommandType.Text;
                    sql.Parameters.AddWithValue("@fechaActual", ldtFechaActual);

                    conn.Open();
                    SqlDataReader resultado = sql.ExecuteReader();

                    while (resultado.Read())
                    {
                        string folio = resultado["folio"] as string;
                        string socio = resultado["Socio"] as string;
                        string estatus = resultado["estatus"] as string;
                        string fecha = Convert.ToDateTime(resultado["fecha"]).ToString("yyyy/MM/dd HH:mm");
                        string slpname = resultado["SlpName"] as string;
                        string fechaSurtiendo = resultado["fechaSurtiendo"] == DBNull.Value ? "" : Convert.ToDateTime(resultado["fechaSurtiendo"]).ToString("yyyy/MM/dd HH:mm");
                        string fechaCerrado = resultado["fechaCerrado"] == DBNull.Value ? "" : Convert.ToDateTime(resultado["fechaCerrado"]).ToString("yyyy/MM/dd HH:mm");
                        string fechaEntregado = resultado["fechaEntregado"] == DBNull.Value ? "" : Convert.ToDateTime(resultado["fechaEntregado"]).ToString("yyyy/MM/dd HH:mm");
                        string empid = Convert.ToString(resultado["empID"]);
                        string nomSurtidor = resultado["firstName"] as string;
                        string lastName = resultado["lastName"] as string;
                        string foto = resultado["foto"] as string;
                        lPedidosDB.Add(new pedidosDB(folio, socio, estatus, fecha, slpname, fechaSurtiendo, fechaCerrado, empid, nomSurtidor + " " + lastName, foto, fechaEntregado));
                    }

                    resultado.Close();
                }
                catch (Exception e)
                {

                    throw;
                }

                
            }

            return lPedidosDB; 
        }

        public string setSurtiendo(IFiltros filtros)
        {
            DateTime ldtFechaActual = DateTime.Now;

            List<pedidosDB> lPedidosDB = new List<pedidosDB>();

            using (SqlConnection conn = new SqlConnection(strConn))
            {
                SqlCommand sql = new SqlCommand("INSERT INTO dcPEDIDOS(folio, socio, fecha, SlpName, estatus, fechaSurtiendo, empID) " +
                                                " VALUES(@folio, @socio, @fecha, @SlpName, @estatus, @fechaSurtiendo, @empID)", conn);

                sql.CommandType = CommandType.Text;
                sql.Parameters.AddWithValue("@folio", filtros.folio);
                sql.Parameters.AddWithValue("@socio", filtros.socio);
                sql.Parameters.AddWithValue("@fecha", filtros.fecha);
                sql.Parameters.AddWithValue("@SlpName", filtros.SlpName);
                sql.Parameters.AddWithValue("@estatus", "2");
                sql.Parameters.AddWithValue("@fechaSurtiendo", ldtFechaActual);
                sql.Parameters.AddWithValue("@empID", filtros.empID);

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
                SqlCommand sql = new SqlCommand("select empID = OHEM.empID, lastName = OHEM.lastName, firstName = OHEM.firstName, middleName = OHEM.middleName, foto = dcSURTIDOR.foto  " +
                                                "               from dbo.OHEM WITH(NOLOCK) " +
                                                "                       LEFT JOIN dcSURTIDOR WITH (NOLOCK) ON dcSURTIDOR.empID = OHEM.empID " +
                                                "               WHERE OHEM.position = 32 and OHEM.branch = 1 ", conn);

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
                try
                {
                    SqlCommand sql = new SqlCommand("update dcPEDIDOS set fechaCerrado = @fechaCerrado, " +
                                                "                     estatus = '4', " +
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
                catch (Exception e)
                {

                    throw;
                }
                
            }
        }

        public string setEntregado(IFiltros filtros)
        {
            DateTime ldtFechaActual = DateTime.Now;

            List<pedidosDB> lPedidosDB = new List<pedidosDB>();

            using (SqlConnection conn = new SqlConnection(strConn))
            {
                try
                {
                    SqlCommand sql = new SqlCommand("update dcPEDIDOS set fechaEntregado = @fechaEntregado, " +
                                                "                     estatus = '3', " +
                                                "                       empID = @empID " +
                                                "where folio = @folio ", conn);

                    sql.CommandType = CommandType.Text;
                    sql.Parameters.AddWithValue("@fechaEntregado", ldtFechaActual);
                    sql.Parameters.AddWithValue("@empID", filtros.empID);
                    sql.Parameters.AddWithValue("@folio", filtros.folio);

                    conn.Open();
                    sql.ExecuteNonQuery();
                    return "Surtimiento Entregado.";
                }
                catch (Exception)
                {

                    throw;
                }
                
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
                SqlCommand sql = new SqlCommand("SELECT empID = MAX(dcPEDIDOS.empID) ,surtidor = OHEM.firstName + ' ' + OHEM.lastName, porSurtir = ISNULL(AVG( DATEDIFF(mi, fecha, fechaSurtiendo)), 0), surtiendo = ISNULL(AVG(DATEDIFF(MI, fechaSurtiendo, fechaCerrado)), 0) " +
                                                    " FROM dcPEDIDOS  WITH (NOLOCK) INNER JOIN dbo.OHEM WITH(NOLOCK) ON OHEM.empID = dcPEDIDOS.empID " +
                                                    " WHERE dcPEDIDOS.estatus > 1 " +
                                                    " AND DATEPART(YEAR, fecha) = @ANNIO " +
                                                    " AND DATEPART(MONTH, fecha) = @MES " +
                                                    " GROUP BY OHEM.firstName + ' ' + OHEM.lastName", conn);

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
            List<conGenerales> lconFinal = new List<conGenerales>();

            using (SqlConnection conn = new SqlConnection(strConn))
            {
                try
                {
                    

                    SqlCommand sql = new SqlCommand(" SELECT dcPEDIDOS.folio, " +
                                                 " statusText = CASE WHEN dcPEDIDOS.estatus = '2' THEN 'Surtiendo' " +
                                                 "                  WHEN dcPEDIDOS.estatus = '3' THEN 'Mostrador' " +
                                                 "                  WHEN dcPEDIDOS.estatus = '4' THEN 'Cerrado' END, " +
                                                 " dcPEDIDOS.fecha, " +
                                                 " porSurtir = ISNULL(DATEDIFF(mi, fecha, fechaSurtiendo), 0), " +
                                                 " dcPEDIDOS.fechaSurtiendo, " +
                                                 " surtiendo = ISNULL(DATEDIFF(mi, fechaSurtiendo, fechaEntregado), 0),  " +
                                                 " dcPEDIDOS.fechaEntregado, " +
                                                 " mostrador = ISNULL(DATEDIFF(mi, fechaEntregado, fechaCerrado), 0), " +
                                                 " dcPEDIDOS.fechaCerrado, " +
                                                 " total = ISNULL(DATEDIFF(mi, fecha, fechaCerrado), 0), " +
                                                 " articulo = VD.U_SO1_NUMEROARTICULO, " +
                                                 " descArticulo = VD.U_SO1_DESCRIPCION, " +
                                                 " OHEM.firstName + ' ' + OHEM.lastName as nomSurtidor, " +
                                                 " dcPEDIDOS.socio, " +
                                                 " dcPEDIDOS.SlpName, " +
                                                 " dcPEDIDOS.empID " +
                                                 " FROM dcPEDIDOS  WITH(NOLOCK) INNER JOIN dbo.OHEM WITH(NOLOCK) ON OHEM.empID = dcPEDIDOS.empID " +
                                                 "      INNER JOIN[@SO1_01VENTA] V WITH(NOLOCK) ON V.Name = dcPEDIDOS.folio " +
                                                 "      INNER JOIN[@SO1_01VENTADETALLE] VD WITH(NOLOCK) ON VD.U_SO1_FOLIO = V.Name " +
                                                 "      INNER JOIN OITM t2 WITH(NOLOCK) ON t2.ItemCode = VD.U_SO1_NUMEROARTICULO " +
                                                 " WHERE dcPEDIDOS.estatus > 1 " +
                                                 "      AND DATEPART(YEAR, fecha) = @ANNIO " +
                                                 "       AND DATEPART(MONTH, fecha) = @MES " +
                                                 " ORDER BY total DESC, dcPEDIDOS.folio ASC ", conn);

                    sql.CommandType = CommandType.Text;
                    sql.Parameters.AddWithValue("@ANNIO", filtros.annio);
                    sql.Parameters.AddWithValue("@MES", filtros.mes);

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
                        string lfirstName = resultado["nomSurtidor"] as string;
                        string lsocio = resultado["socio"] as string;
                        string lSlpName = resultado["SlpName"] as string;
                        string lEmpID = Convert.ToString(resultado["empID"]) ;
                        string lMostrador = Convert.ToInt16(resultado["mostrador"]).ToString();
                        string lTotal = Convert.ToInt16(resultado["total"]).ToString();
                        string lDescArticulo = resultado["descArticulo"] as string;

                        lconGenerales.Add(new conGenerales(lfolio, lstatusText, lfecha, lporSurtir, lfechaSurtiendo, lsurtiendo, lfechaCerrado, lfirstName, lsocio, lSlpName, lEmpID, lMostrador, lTotal, lDescArticulo));
                    }

                    resultado.Close();


                }
                catch (Exception e)
                {

                    throw;
                }

                
            }

            lconGenerales.ForEach(renglon => {
                bool ldiaSiNo = filtros.diasList.Any(x => x == Convert.ToDateTime(renglon.fecha).Day || x == 0);
                bool lsurtidorSiNo = filtros.surtidoresList.Any(x => x == renglon.empID || x == "0");
                if (ldiaSiNo && lsurtidorSiNo)
                    lconFinal.Add(renglon);
            } );

            return lconFinal;
        }

        public string insFoto(IFiltros filtros)
        {

            List<bannerDB> lbannerDB = new List<bannerDB>();

            using (SqlConnection conn = new SqlConnection(strConn))
            {
                SqlCommand sql = new SqlCommand("INSERT INTO dcSURTIDOR(empID, foto) " +
                                                " VALUES(@empID, @foto) ", conn);

                sql.CommandType = CommandType.Text;
                sql.Parameters.AddWithValue("@foto", filtros.Foto);
                sql.Parameters.AddWithValue("@empID", filtros.empID);

                conn.Open();
                sql.ExecuteNonQuery();
                return "Foto ingresada.";
            }
        }
        public string updFoto(IFiltros filtros)
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