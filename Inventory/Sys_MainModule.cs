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
    
    public partial class Sys_MainModule
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Sys_MainModule()
        {
            this.Sys_EntryModule = new HashSet<Sys_EntryModule>();
            this.Sys_ReportModule = new HashSet<Sys_ReportModule>();
            this.Sys_SetupModule = new HashSet<Sys_SetupModule>();
        }
    
        public int MainModuleID { get; set; }
        public string MainModuleName { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sys_EntryModule> Sys_EntryModule { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sys_ReportModule> Sys_ReportModule { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sys_SetupModule> Sys_SetupModule { get; set; }
    }
}
