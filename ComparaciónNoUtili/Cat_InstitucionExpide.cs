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
    
    public partial class Cat_InstitucionExpide
    {
        public Cat_InstitucionExpide()
        {
            this.Rel_TipoIdentificacionInstExpide = new HashSet<Rel_TipoIdentificacionInstExpide>();
        }
    
        public int IdInstExpide { get; set; }
        public string DescripcionInsExpide { get; set; }
        public bool EstatusRegistro { get; set; }
        public int Fk_IdUsuarioRegistro { get; set; }
        public System.DateTime FechaRegistro { get; set; }
    
        public virtual ICollection<Rel_TipoIdentificacionInstExpide> Rel_TipoIdentificacionInstExpide { get; set; }
    }
}
