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
    
    public partial class Sys_ReportModule
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Sys_ReportModule()
        {
            this.S_UserRightReport = new HashSet<S_UserRightReport>();
        }
    
        public int ReportModuleID { get; set; }
        public Nullable<int> MainModuleID { get; set; }
        public string ReportModuleName { get; set; }
        public Nullable<int> SortCode { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<S_UserRightReport> S_UserRightReport { get; set; }
        public virtual Sys_MainModule Sys_MainModule { get; set; }
    }
}
