USE [ArrendamientoInmueble]
GO

/****** Object:  StoredProcedure [dbo].[NoUtiliAuditoria2019P]    Script Date: 08/11/2019 01:00:00 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[NoUtiliAuditoria2019P]
AS
BEGIN
	IF (EXISTS (SELECT name FROM sysobjects WHERE type = 'U' AND name = 'NoUtiliAuditoria2019T')) DROP TABLE NoUtiliAuditoria2019T;

	CREATE TABLE [dbo].[NoUtiliAuditoria2019T](
	[IdInmuebleArrendamiento] [int] NULL,
	[Fk_IdInstitucion] [int] NULL,
	[CargoUsuarioRegistro] [nvarchar](250) NULL,
	[Fk_IdUsuarioRegistro] [int] NULL,
	[FechaRegistro] [datetime] NULL,
	[FechaFinOcupacion] [datetime] NULL,
	[NombreInmueble] [nvarchar](250) NULL,
	[Fk_IdPais] [int] NULL,
	[Fk_IdEstado] [int] NULL,
	[Fk_IdMinicipio] [int] NULL,
	[Fk_IdLocalidad] [int] NULL,
	[Fk_IdTipoVialidad] [int] NULL,
	[NombreVialidad] [nvarchar](250) NULL,
	[NumExterior] [nvarchar](250) NULL,
	[NumInterior] [nvarchar](250) NULL,
	[CodigoPostal] [int] NULL,
	[CodigoPostalExtranjero] [int] NULL,
	[EstadoExtranjero] [int] NULL,
	[CiudadExtranjero] [int] NULL,
	[MunicipioExtranjero] [int] NULL,
	[FolioAplicaciónConcepto] [int] NULL,
	[FolioContratoArrto] [int] NULL,
	[OtraColonia] [nvarchar](250) NULL,
	[DescripcionTipoContrato] [nvarchar](250) NULL,
	[DescripcionTipoArrendamiento] [nvarchar](250) NULL,
	[Espadre] [int] NULL,
	[EstatusRegistroContrato] [int] NULL,
	[RIUF] [nvarchar](250) NULL,
	[Observaciones] [nvarchar](250) NULL,
	[IsNotReusable] [int] NULL,
	[Identificador] [int] identity(1,1)
	CONSTRAINT [PK_NoUtiliAuditoria2019T] PRIMARY KEY CLUSTERED 
(
	[Identificador] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];

		INSERT INTO NoUtiliAuditoria2019T exec [dbo].[spuConsultadeTodoslosContratos];
END

--DROP PROCEDURE NoUtiliAuditoria2019P
--EXEC NoUtiliAuditoria2019P

SELECT *
FROM NoUtiliAuditoria2019T
GO


