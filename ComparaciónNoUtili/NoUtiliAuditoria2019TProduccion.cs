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
    using System.Collections.Generic;
    
    public partial class NoUtiliAuditoria2019TProduccion
    {
        public int ID { get; set; }
        public Nullable<int> IdInmuebleArrendamiento { get; set; }
        public Nullable<int> Fk_IdInstitucion { get; set; }
        public string CargoUsuarioRegistro { get; set; }
        public Nullable<int> Fk_IdUsuarioRegistro { get; set; }
        public Nullable<System.DateTime> FechaRegistro { get; set; }
        public Nullable<System.DateTime> FechaFinOcupacion { get; set; }
        public string NombreInmueble { get; set; }
        public Nullable<int> Fk_IdPais { get; set; }
        public Nullable<int> Fk_IdEstado { get; set; }
        public Nullable<int> Fk_IdMinicipio { get; set; }
        public Nullable<int> Fk_IdLocalidad { get; set; }
        public Nullable<int> Fk_IdTipoVialidad { get; set; }
        public string NombreVialidad { get; set; }
        public string NumExterior { get; set; }
        public string NumInterior { get; set; }
        public Nullable<int> CodigoPostal { get; set; }
        public Nullable<int> CodigoPostalExtranjero { get; set; }
        public Nullable<int> EstadoExtranjero { get; set; }
        public Nullable<int> CiudadExtranjero { get; set; }
        public Nullable<int> MunicipioExtranjero { get; set; }
        public Nullable<int> FolioAplicaciónConcepto { get; set; }
        public Nullable<int> FolioContratoArrto { get; set; }
        public string OtraColonia { get; set; }
        public string DescripcionTipoContrato { get; set; }
        public string DescripcionTipoArrendamiento { get; set; }
        public Nullable<int> Espadre { get; set; }
        public Nullable<int> EstatusRegistroContrato { get; set; }
        public string RIUF { get; set; }
        public string Observaciones { get; set; }
        public Nullable<int> IsNotReusable { get; set; }
        public int Identificador { get; set; }
    }
}
