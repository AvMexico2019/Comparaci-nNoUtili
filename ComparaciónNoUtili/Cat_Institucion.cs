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
    
    public partial class Cat_Institucion
    {
        public int IdInstitucion { get; set; }
        public string DescripcionInstitucion { get; set; }
        public Nullable<int> Fk_IdNatJuridica { get; set; }
        public Nullable<int> Fk_IdSector { get; set; }
        public Nullable<int> Fk_IdTipoInstitucion { get; set; }
        public bool EstatusRegistro { get; set; }
        public int Fk_IdUsuarioRegistro { get; set; }
        public System.DateTime FechaRegistro { get; set; }
    }
}
