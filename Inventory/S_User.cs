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
    
    public partial class S_User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public S_User()
        {
            this.S_UserRightEntry = new HashSet<S_UserRightEntry>();
            this.S_UserRightReport = new HashSet<S_UserRightReport>();
            this.S_UserRightSetup = new HashSet<S_UserRightSetup>();
            this.T_MasterSale = new HashSet<T_MasterSale>();
        }
    
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Nullable<int> BranchID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<S_UserRightEntry> S_UserRightEntry { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<S_UserRightReport> S_UserRightReport { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<S_UserRightSetup> S_UserRightSetup { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<T_MasterSale> T_MasterSale { get; set; }
    }
}
