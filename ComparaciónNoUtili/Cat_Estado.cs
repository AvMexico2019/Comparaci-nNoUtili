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
    
    public partial class Cat_Estado
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Cat_Estado()
        {
            this.Cat_CodigoPostal = new HashSet<Cat_CodigoPostal>();
            this.Cat_Municipio = new HashSet<Cat_Municipio>();
        }
    
        public int IdEstado { get; set; }
        public int Fk_IdPais { get; set; }
        public string DescripcionEstado { get; set; }
        public string Abreviatura { get; set; }
        public bool EstatusRegistro { get; set; }
        public int Fk_IdUsuarioRegistro { get; set; }
        public System.DateTime FechaRegistro { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cat_CodigoPostal> Cat_CodigoPostal { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cat_Municipio> Cat_Municipio { get; set; }
        public virtual Cat_Pais Cat_Pais { get; set; }
    }
}
