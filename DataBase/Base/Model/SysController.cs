using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataBase.Base.Model
{
    public class SysController : DbSetBase
    {
        // Methods
        public SysController()
        {
           SystemId = "000";
           ControllerName = "Index";
           ActionName = "Index";
           Display = true;
           Ico = "icon-list-ul";
        }
        
        [MaxLength(50)]
        public string ActionName { get; set; }

        [MaxLength(50), Required]
        public string ControllerDisplayName { get; set; }

        [MaxLength(50)]
        public string ControllerName { get; set; }

        public bool Display { get; set; }

        [DataType("Ico")]
        public string Ico { get; set; }

        [MaxLength(50)]
        public string Parameter { get; set; }

        [Display(Name = "Action"), DataType("MultiSelectList")]
        public List<long> SysActionsId { get; set; }
        
        public virtual ICollection<SysControllerSysAction> SysControllerSysActions { get; set; }

        [MaxLength(50), Required]
        public string SystemId { get; set; }
        
        [ForeignKey("SysArea")]
        public long? SysAreaId { get; set; }

        [ScaffoldColumn(false)]
        public virtual SysArea SysArea { get; set; }
    }



}
