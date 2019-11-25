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
    
    public partial class S_Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public S_Product()
        {
            this.T_TranSale = new HashSet<T_TranSale>();
        }
    
        public int ProductID { get; set; }
        public string Code { get; set; }
        public Nullable<int> SortCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<int> SubMenuID { get; set; }
        public Nullable<decimal> PurPrice { get; set; }
        public Nullable<decimal> SalePrice { get; set; }
        public Nullable<bool> IsUnit { get; set; }
        public Nullable<bool> IsStock { get; set; }
        public byte[] Photo { get; set; }
        public Nullable<int> AlertQty { get; set; }
        public Nullable<short> DisPercent { get; set; }
        public string Barcode { get; set; }
    
        public virtual S_SubMenu S_SubMenu { get; set; }
        public virtual S_Unit S_Unit { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<T_TranSale> T_TranSale { get; set; }
    }
}
