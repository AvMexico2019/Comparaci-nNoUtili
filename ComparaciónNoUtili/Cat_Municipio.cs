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
    
    public partial class Cat_Municipio
    {
        public Cat_Municipio()
        {
            this.Cat_CodigoPostal = new HashSet<Cat_CodigoPostal>();
            this.Cat_Localidades = new HashSet<Cat_Localidades>();
        }
    
        public int IdMunicipio { get; set; }
        public int Fk_IdEstado { get; set; }
        public int CveMunicipio { get; set; }
        public string DescripcionMunicipio { get; set; }
        public bool EstatusRegistro { get; set; }
        public int Fk_IdUsuarioRegistro { get; set; }
        public System.DateTime FechaRegistro { get; set; }
    
        public virtual ICollection<Cat_CodigoPostal> Cat_CodigoPostal { get; set; }
        public virtual Cat_Estado Cat_Estado { get; set; }
        public virtual ICollection<Cat_Localidades> Cat_Localidades { get; set; }
    }
}
