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
    
    public partial class EmisionOpinonContrato
    {
        public int IdEmisionOpinionContrato { get; set; }
        public int Fk_IdAplicacionConcepto { get; set; }
        public int NumContrato { get; set; }
        public bool EsHistorico { get; set; }
        public bool EstatusRegistro { get; set; }
        public System.DateTime FechaRegistro { get; set; }
        public int Fk_IdUsuarioRegistro { get; set; }
    
        public virtual AplicacionConcepto AplicacionConcepto { get; set; }
    }
}
