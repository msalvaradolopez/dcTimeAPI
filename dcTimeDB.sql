drop table dcPEDIDOS
go

CREATE TABLE dcPEDIDOS (
	folio varchar(100) not null,
	cliente varchar(100) not null,
	fecha datetime not null,
	estatus char(1) not null default 0,
	fechaSurtiendo datetime null,
	fechaCerrado datetime null,
	idSurtidor varchar(50) NULL
)
go

create table dcSURTIDOR (
	idSurtidor varchar(50) NOT NULL,
	nomSurtidor VARCHAR(200) NULL,
	foto VARCHAR(MAX) NULL,
	estatus char(1) NOT NULL DEFAULT 'A',
	CONSTRAINT PK_dcSURTIDOR PRIMARY KEY (idSurtidor)
)

INSERT INTO dcSURTIDOR(idSurtidor, nomSurtidor, estatus)
	select '0001', 'JUAN PAREZ', 'A' 
	UNION ALL
	select '0002', 'PACO RODRIGUEZ', 'A' 
	UNION ALL
	select '0003', 'PEDRO MARTINEZ', 'A' 
	UNION ALL
	select '0004', 'SANDRA CANTU', 'A' 
	UNION ALL
	select '0005', 'BENITO GOMEZ', 'A' 
	
select * from dcSURTIDOR
select * from dcPEDIDOS


insert into dcPEDIDOS(folio, cliente, fecha, estatus)
	select 'ALT-01010101', 'Salvador Alvarado', '20220621 13:35', 1
	UNION ALL
	select 'ALT-01010110', 'JUAN PEREZ', '20220621 13:40', 2
	UNION ALL
	select 'ALT-01010120', 'KATIA MTZ', '20220621 13:42', 3
	UNION ALL
	select 'ALT-01010130', 'CLIENTES VARIOS', '20220621 13:45', 1
	UNION ALL
	select 'ALT-01010160', 'OTROS VALORES', '20220621 13:45', 1


select * from dcPEDIDOS

update dcPEDIDOS
set estatus = '2'
where folio = 'ALT-01010120'


--- *******************************************************************

DECLARE @fechaActual datetime 

set @fechaActual = GETDATE()

SELECT NumAtCard, DocNum, CardCode, CardName, DocDate, CANCELED, DocStatus
FROM ORDR T1 -- INNER JOIN ELECTRICO..NNM1 T2 ON T2.Series = T1.Series AND T2.SeriesName = 'ALT'
WHERE CONVERT(VARCHAR(10), DocDate, 112) = CONVERT(VARCHAR(10), @fechaActual, 112)
	
select empID, lastName, firstName, middleName, jobTitle, * from OHEM

-- NumAtCard : nvarchar, 200
-- DocEntry : int
-- DocNum : int
-- CardCode nvarchar, 30
-- CardName : nvarchar, 200
-- DocDate : datetime
-- CANCELED : char, 1
-- DocStatus: char, 1

