USE [ArrendamientoInmueble]
GO

/****** Object:  Table [dbo].[NoUtiliAuditoria2019T]    Script Date: 08/11/2019 12:51:10 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
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
	[Identificador] [int]
	CONSTRAINT [PK_NoUtiliAuditoria2019T] PRIMARY KEY CLUSTERED 
(
	[Identificador] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


