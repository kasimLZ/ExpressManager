using Common.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataBase.Base.Interface;
using System.Linq;

namespace DataBase.Base.Model
{
    [Order(Field = "SystemId", Order = "ASC")]
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

        [MaxLength(50), Required, ShowName]
        public string ControllerDisplayName { get; set; }

        [MaxLength(50)]
        public string ControllerName { get; set; }

        public bool Display { get; set; }

        [DataType("Ico")]
        public string Ico { get; set; }

        [MaxLength(50)]
        public string Parameter { get; set; }

        [Display(Name = "Action")]
        [SelectList(typeof(ISysActionInterface), "SelectA", true)]
        [DataFor(DataScoure = "SysControllerSysActions",Value = "SysActionId", Mode = UpdateMode.Replace)]
        public List<long> SysActionsId { get; set; }
        
        public virtual ICollection<SysControllerSysAction> SysControllerSysActions { get; set; }

        [MaxLength(50), Required]
        public string SystemId { get; set; }

        [ForeignKey("SysArea")]
        [Lookup(LookupType.Base, LookupLink.Route, "Area=SysManager,Controller=SysArea,Action=Lookup", "AreaDisplayName")]
        public long? SysAreaId { get; set; }

        [ScaffoldColumn(false)]
        public virtual SysArea SysArea { get; set; }
    }



}
