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
    
    public partial class Cat_Parametro
    {
        public short IdParametro { get; set; }
        public string ParametroNombre { get; set; }
        public string DescripcionParametro { get; set; }
        public string ValorParametro { get; set; }
        public string ValorAdicionalParametro { get; set; }
        public bool EstatusRegistro { get; set; }
        public int Fk_IdUsuarioRegistro { get; set; }
        public System.DateTime FechaRegistro { get; set; }
    }
}
