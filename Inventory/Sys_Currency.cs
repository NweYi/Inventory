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
    
    public partial class Sys_Currency
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Sys_Currency()
        {
            this.T_MasterSale = new HashSet<T_MasterSale>();
        }
    
        public int CurrencyID { get; set; }
        public string Keyword { get; set; }
        public Nullable<decimal> Rate { get; set; }
        public Nullable<bool> IsDefault { get; set; }
        public Nullable<bool> IsMultiCurrency { get; set; }
        public string CurrencyName { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<T_MasterSale> T_MasterSale { get; set; }
    }
}
