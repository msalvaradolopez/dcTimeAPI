
USE dbaux
-- data:image/jpeg;base64,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
-- update dcPEDIDOS set estatus = 1, fechaCerrado = null, fechaSurtiendo = null, empID = null
select * from dcPEDIDOS
SELECT * FROM dcSURTIDOR



CREATE TABLE dcPEDIDOS (
	folio nvarchar(200) not null,
	Socio nvarchar(200) not null,
	fecha datetime not null,
	SlpName nvarchar(310) not null,
	estatus char(1) not null default 0,
	fechaSurtiendo datetime null,
	fechaCerrado datetime null,
	empID int NULL,
	CONSTRAINT PK_dcPEDIDOS PRIMARY KEY (folio)
)
go

create table dcSURTIDOR (
	empID int NOT NULL,
	lastName nvarchar(100) NULL, 
	firstName nvarchar(100) NULL,  
	middleName nvarchar(100) NULL, 
	foto VARCHAR(MAX) NULL,
	estatus char(1) NOT NULL DEFAULT 'A',
	CONSTRAINT PK_dcSURTIDOR PRIMARY KEY (empID)
)

create table dcBANNER (
	bannerID NVARCHAR(50) NOT NULL,
	texto NVARCHAR(200) NOT NULL,
	CONSTRAINT PK_dcBANNER PRIMARY KEY (bannerID)
)
select LEN(texto) from dcBANNER
delete dcBANNER
-- ' Estimado cliente: Le informamos que el surtido de CABLES por metro requerimos un tiempo para elaborar el corte (10 - 15 min dependiendo del calibre), agradecemos su comprención.'
INSERT INTO dcBANNER
	select 'TEXTO 1', ' Estimado cliente: Le informamos que el surtido de CABLES por metro requerimos un tiempo para elaborar el corte (10 - 15 min dependiendo del calibre), agradecemos su comprención.'
	UNION ALL
	SELECT 'TEXTO 2', ' Eléctrico ofreciéndole el mejor servicio y ateción a nuestros clientes.' 

-- Name : nvarchar , 200
-- Socio : nvarchar, 200
-- SlpName : nvarchar , 310
EXEC spPedidosDCTime
CREATE PROCEDURE spPedidosDCTime
as

	DECLARE @fechaActual DateTime
	SET @fechaActual = getdate()

	INSERT INTO dcPEDIDOS(folio, Socio, fecha, SlpName, estatus)
	SELECT V.Name [FOLIO], 
			OCRD.CardName [SOCIO], 
			CONVERT(DATETIME, SUBSTRING(CONVERT(VARCHAR(10), V.U_SO1_FECHA, 103), 1, 10) + ' ' + 
			SUBSTRING(RIGHT('0000'+CAST(V.U_SO1_HORA AS VARCHAR(4)), 4), 1,2) + ':' + 
			SUBSTRING(RIGHT('0000'+CAST(V.U_SO1_HORA AS VARCHAR(4)), 4), 3,2) , 103) [FECHA],  
			OSLP.SlpName [VENDEDOR],
			'1'
	FROM [SRVRALTR1\SRVRALT].[Retail One].dbo.[@SO1_01VENTA] V WITH (NOLOCK) 
			JOIN [SRVRALTR1\SRVRALT].[Retail One].dbo.OSLP WITH (NOLOCK) ON OSLP.SlpCode = V.U_SO1_VENDEDOR
			JOIN [SRVRALTR1\SRVRALT].[Retail One].dbo.OCRD WITH (NOLOCK) ON OCRD.CardCode = V.U_SO1_CLIENTE
	WHERE CONVERT(VARCHAR(10), V.U_SO1_FECHA, 112) = CONVERT(VARCHAR(10), @fechaActual, 112)
			AND V.U_SO1_TIPO = 'PE'
			AND NOT EXISTS (SELECT 1 FROM dcPEDIDOS T1 WHERE T1.folio = V.Name)
			AND (UPPER(V.U_SO1_COMENTARIO) LIKE '%MOSTRADOR%' OR V.U_SO1_COMENTARIO LIKE '%CAJA%')

	insert into dcSURTIDOR( empID, lastName, firstName, middleName, estatus)
	SELECT empID, lastName, firstName, middleName, 'A' 
	FROM [SRVRALTR1\SRVRALT].[Retail One].dbo.OHEM T1 WITH (NOLOCK) 
	WHERE dept = 7 and branch = 1
			AND NOT EXISTS (SELECT 1 FROM dcSURTIDOR T2 WHERE T2.empID = T1.empID)

GO