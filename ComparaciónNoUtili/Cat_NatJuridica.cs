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
    
    public partial class Cat_NatJuridica
    {
        public Cat_NatJuridica()
        {
            this.Cat_Institucion = new HashSet<Cat_Institucion>();
        }
    
        public int IdNatJuridica { get; set; }
        public string DescripcionNatJuridica { get; set; }
        public bool EstatusRegistro { get; set; }
        public int Fk_IdUsuarioRegistro { get; set; }
        public System.DateTime FechaRegistro { get; set; }
    
        public virtual ICollection<Cat_Institucion> Cat_Institucion { get; set; }
    }
}
