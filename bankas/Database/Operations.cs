//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace bankas.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class Operations
    {
        public int ID { get; set; }
        public Nullable<System.DateTime> TimeOper { get; set; }
        public Nullable<int> SumOper { get; set; }
    
        public virtual Bills Bills { get; set; }
    }
}