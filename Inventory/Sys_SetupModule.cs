//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Inventory
{
    using System;
    using System.Collections.Generic;
    
    public partial class Sys_SetupModule
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Sys_SetupModule()
        {
            this.S_UserRightSetup = new HashSet<S_UserRightSetup>();
        }
    
        public int SetupModuleID { get; set; }
        public Nullable<int> MainModuleID { get; set; }
        public string SetupModuleName { get; set; }
        public Nullable<int> SortCode { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<S_UserRightSetup> S_UserRightSetup { get; set; }
        public virtual Sys_MainModule Sys_MainModule { get; set; }
    }
}