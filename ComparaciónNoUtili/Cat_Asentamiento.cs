//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class Cat_Asentamiento
    {
        public int IdAsentamiento { get; set; }
        public string DescripcionAsentamiento { get; set; }
        public int Fk_IdEstado { get; set; }
        public int Fk_IdMunicipio { get; set; }
        public Nullable<int> Fk_IdLocalidad { get; set; }
        public int Fk_IdTipoAsentamiento { get; set; }
        public bool EstatusRegistro { get; set; }
        public int Fk_IdUsuarioRegistro { get; set; }
        public System.DateTime FechaRegistro { get; set; }
    
        public virtual Cat_Localidades Cat_Localidades { get; set; }
        public virtual Cat_TipoAsentamiento Cat_TipoAsentamiento { get; set; }
    }
}
