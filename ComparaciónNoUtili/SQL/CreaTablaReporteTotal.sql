USE [ArrendamientoInmueble]
GO

/****** Object:  Table [dbo].[ReporteTotal]    Script Date: 12/11/2019 11:53:42 a. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ReporteTotal](
	[NumeroSecuencial] [int] IDENTITY(1,1) NOT NULL,
	[Id] [int] NOT NULL,
	[Fk_IdInstitucion] [int] NOT NULL,
	[No] [int] NOT NULL,
	[FolioContrato] [int] NOT NULL,
	[FolioContratoAnterior] [int] NULL,
	[TipoContrato] [nvarchar](50) NOT NULL,
	[TipoOcupacion] [nvarchar](50) NULL,
	[Secuencial] [nvarchar](50) NULL,
	[Propietario] [nvarchar](250) NOT NULL,
	[FechaDictamen] [datetime] NULL,
	[Responsable] [nvarchar](250) NULL,
	[Sector] [nvarchar](50) NULL,
	[Promovente] [nvarchar](250) NOT NULL,
	[FkIdPais] [int] NOT NULL,
	[Pais] [int] NOT NULL,
	[Fk_IdEstado] [int] NULL,
	[Estado] [nvarchar](50) NULL,
	[Fk_IdMunicipio] [int] NULL,
	[Municipio] [nvarchar](250) NULL,
	[Fk_IdLocalidad] [int] NULL,
	[RetrieveBusColonia] [smallint] NOT NULL,
	[Colonia] [nvarchar](250) NULL,
	[Calle] [nvarchar](250) NULL,
	[CodigoPostal] [nvarchar](250) NULL,
	[NumInterior] [nvarchar](250) NULL,
	[NumExterior] [nvarchar](250) NULL,
	[Ciudad] [nvarchar](250) NULL,
	[MontoDictaminado] [money] NOT NULL,
	[FechaContratoDesde] [datetime] NOT NULL,
	[FechaCntratoHasta] [datetime] NOT NULL,
	[Fk_IdTipoArrendamiento] [smallint] NOT NULL,
	[DescripcionTipoArrendamiento] [nvarchar](50) NOT NULL,
	[Fk_IdTipoInmueble] [smallint] NOT NULL,
	[TipoInmueble] [nvarchar](50) NOT NULL,
	[MontoPagoPorCajonesEstacionamiento] [money] NOT NULL,
	[Fk_IdTipoContratacion] [int] NULL,
	[DescripcionTipoContratacion] [nvarchar](250) NULL,
	[RentaUnitariaMensual] [money] NULL,
	[MontoPagoMensual] [money] NULL,
	[CuotaMantenimiento] [money] NULL,
	[AreaOcupadaM2] [float] NULL,
	[Fk_IdTipoUsoInmueble] [int] NOT NULL,
	[TipoUsoInmueble] [nvarchar](50) NOT NULL,
	[OtroUsoInmueble] [nvarchar](250) NOT NULL,
	[TablaSMOI] [nvarchar](50) NOT NULL,
	[ResultadosOpinion] [nvarchar](50) NOT NULL,
	[MontoAnterior] [money] NOT NULL,
	[SMOI] [float] NOT NULL,
	[Fecha] [datetime] NULL,
	[RIUF] [nvarchar](50) NOT NULL,
	[GeoRefLatitud] [decimal](18, 0) NULL,
	[GeoRefLongitud] [decimal](18, 0) NULL,
 CONSTRAINT [PK_ReporteToTal] PRIMARY KEY CLUSTERED 
(
	[NumeroSecuencial] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

