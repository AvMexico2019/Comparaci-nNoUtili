//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ComparaciónNoUtili
{
    using System;
    
    public partial class spuConsultadeTodoslosContratos_Result
    {
        public int IdInmuebleArrendamiento { get; set; }
        public int Fk_IdInstitucion { get; set; }
        public string CargoUsuarioRegistro { get; set; }
        public int Fk_IdUsuarioRegistro { get; set; }
        public string FechaRegistro { get; set; }
        public string FechaFinOcupacion { get; set; }
        public string NombreInmueble { get; set; }
        public int Fk_IdPais { get; set; }
        public Nullable<int> Fk_IdEstado { get; set; }
        public Nullable<int> Fk_IdMunicipio { get; set; }
        public Nullable<int> Fk_IdLocalidad { get; set; }
        public int Fk_IdTipoVialidad { get; set; }
        public string NombreVialidad { get; set; }
        public string NumExterior { get; set; }
        public string NumInterior { get; set; }
        public string CodigoPostal { get; set; }
        public string CodigoPostalExtranjero { get; set; }
        public string EstadoExtranjero { get; set; }
        public string CiudadExtranjero { get; set; }
        public string MunicipioExtranjero { get; set; }
        public Nullable<int> FolioAplicacionConcepto { get; set; }
        public Nullable<int> FolioContratoArrto { get; set; }
        public string OtraColonia { get; set; }
        public string DescripcionTipoContrato { get; set; }
        public string DescripcionTipoArrendamiento { get; set; }
        public Nullable<bool> EsPadre { get; set; }
        public Nullable<bool> EstatusRegistroContrato { get; set; }
        public string RIUF { get; set; }
        public string Observaciones { get; set; }
        public Nullable<int> IsNotReusable { get; set; }
    }
}
