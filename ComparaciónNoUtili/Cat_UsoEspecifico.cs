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
    
    public partial class Cat_UsoEspecifico
    {
        public int IdUsoEspecifico { get; set; }
        public int Fk_IdUsoGenerico { get; set; }
        public string DescripcionUsoEspecifico { get; set; }
        public bool EstatusRegistro { get; set; }
        public int Fk_IdUsuarioRegistro { get; set; }
        public System.DateTime FechaRegistro { get; set; }
    
        public virtual Cat_UsoGenerico Cat_UsoGenerico { get; set; }
    }
}
