﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ComparaciónNoUtili
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DB_CAT_NuevaEntities : DbContext
    {
        public DB_CAT_NuevaEntities()
            : base("name=DB_CAT_NuevaEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<BitacoraSistema> BitacoraSistema { get; set; }
        public DbSet<Cat_AdmonCarretera> Cat_AdmonCarretera { get; set; }
        public DbSet<Cat_Asentamiento> Cat_Asentamiento { get; set; }
        public DbSet<Cat_Bancos> Cat_Bancos { get; set; }
        public DbSet<Cat_Calendario> Cat_Calendario { get; set; }
        public DbSet<Cat_Cargo> Cat_Cargo { get; set; }
        public DbSet<Cat_CatalogadoPor> Cat_CatalogadoPor { get; set; }
        public DbSet<Cat_CategoriaGeneral> Cat_CategoriaGeneral { get; set; }
        public DbSet<Cat_ClasificacionReligiosa> Cat_ClasificacionReligiosa { get; set; }
        public DbSet<Cat_CodigoPostal> Cat_CodigoPostal { get; set; }
        public DbSet<Cat_ComponenteEspacial> Cat_ComponenteEspacial { get; set; }
        public DbSet<Cat_CRITERIOS> Cat_CRITERIOS { get; set; }
        public DbSet<Cat_DerechoTransito> Cat_DerechoTransito { get; set; }
        public DbSet<Cat_DiasInhabiles> Cat_DiasInhabiles { get; set; }
        public DbSet<Cat_DoctoAlterno> Cat_DoctoAlterno { get; set; }
        public DbSet<Cat_Emisor> Cat_Emisor { get; set; }
        public DbSet<Cat_Estado> Cat_Estado { get; set; }
        public DbSet<Cat_EstadoCivil> Cat_EstadoCivil { get; set; }
        public DbSet<Cat_Genero> Cat_Genero { get; set; }
        public DbSet<Cat_Institucion> Cat_Institucion { get; set; }
        public DbSet<Cat_InstitucionExpide> Cat_InstitucionExpide { get; set; }
        public DbSet<Cat_LeyendaAnual> Cat_LeyendaAnual { get; set; }
        public DbSet<Cat_Localidades> Cat_Localidades { get; set; }
        public DbSet<Cat_MargenCamino> Cat_MargenCamino { get; set; }
        public DbSet<Cat_Mes> Cat_Mes { get; set; }
        public DbSet<Cat_Municipio> Cat_Municipio { get; set; }
        public DbSet<Cat_NatJuridica> Cat_NatJuridica { get; set; }
        public DbSet<Cat_NivelGobierno> Cat_NivelGobierno { get; set; }
        public DbSet<Cat_Pais> Cat_Pais { get; set; }
        public DbSet<Cat_Parametro> Cat_Parametro { get; set; }
        public DbSet<Cat_ParametroHistorico> Cat_ParametroHistorico { get; set; }
        public DbSet<Cat_Periodo> Cat_Periodo { get; set; }
        public DbSet<Cat_Procedencia> Cat_Procedencia { get; set; }
        public DbSet<Cat_Sector> Cat_Sector { get; set; }
        public DbSet<Cat_Supuestos> Cat_Supuestos { get; set; }
        public DbSet<Cat_TerminoGenCamino> Cat_TerminoGenCamino { get; set; }
        public DbSet<Cat_TIPO_VALOR> Cat_TIPO_VALOR { get; set; }
        public DbSet<Cat_TipoAsenta> Cat_TipoAsenta { get; set; }
        public DbSet<Cat_TipoAsentamiento> Cat_TipoAsentamiento { get; set; }
        public DbSet<Cat_TipoDependencia> Cat_TipoDependencia { get; set; }
        public DbSet<Cat_TipoDocumentoAdjunto> Cat_TipoDocumentoAdjunto { get; set; }
        public DbSet<Cat_TipoIdentificacion> Cat_TipoIdentificacion { get; set; }
        public DbSet<Cat_TipoInmueble> Cat_TipoInmueble { get; set; }
        public DbSet<Cat_TipoInstitucion> Cat_TipoInstitucion { get; set; }
        public DbSet<Cat_TipoMoneda> Cat_TipoMoneda { get; set; }
        public DbSet<Cat_TipoNotario> Cat_TipoNotario { get; set; }
        public DbSet<Cat_TipoPatente> Cat_TipoPatente { get; set; }
        public DbSet<Cat_TipoPersona> Cat_TipoPersona { get; set; }
        public DbSet<Cat_TipoProcedimiento> Cat_TipoProcedimiento { get; set; }
        public DbSet<Cat_TipoRecepcion> Cat_TipoRecepcion { get; set; }
        public DbSet<Cat_TiposOrganizacion> Cat_TiposOrganizacion { get; set; }
        public DbSet<Cat_TipoTelefono> Cat_TipoTelefono { get; set; }
        public DbSet<Cat_TipoUsoInmueble> Cat_TipoUsoInmueble { get; set; }
        public DbSet<Cat_TipoVialidad> Cat_TipoVialidad { get; set; }
        public DbSet<Cat_TituloPersona> Cat_TituloPersona { get; set; }
        public DbSet<CAT_TRIMESTRE> CAT_TRIMESTRE { get; set; }
        public DbSet<Cat_UnidadMedida> Cat_UnidadMedida { get; set; }
        public DbSet<Cat_UnidadNegocio> Cat_UnidadNegocio { get; set; }
        public DbSet<Cat_UsoEspecifico> Cat_UsoEspecifico { get; set; }
        public DbSet<Cat_UsoGenerico> Cat_UsoGenerico { get; set; }
        public DbSet<Cat_Zona> Cat_Zona { get; set; }
        public DbSet<Rel_TipoIdentificacionInstExpide> Rel_TipoIdentificacionInstExpide { get; set; }
        public DbSet<RelCargoUnidadNeg> RelCargoUnidadNeg { get; set; }
        public DbSet<sysdiagrams> sysdiagrams { get; set; }
        public DbSet<Cat_Institucion_20171127> Cat_Institucion_20171127 { get; set; }
        public DbSet<Cat_TipoDomicilio> Cat_TipoDomicilio { get; set; }
        public DbSet<Cat_TipoUsuario> Cat_TipoUsuario { get; set; }
        public DbSet<Cat_TraductorInstitucion1> Cat_TraductorInstitucion1 { get; set; }
        public DbSet<Cat_TraductorSector1> Cat_TraductorSector1 { get; set; }
        public DbSet<Temp_tipoDoctoAdjunto> Temp_tipoDoctoAdjunto { get; set; }
    }
}
