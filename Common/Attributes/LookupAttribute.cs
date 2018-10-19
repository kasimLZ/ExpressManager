using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace Common.Attributes
{
	/// <summary>
	/// Popup window properties
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class LookupAttribute : DataTypeAttribute , IEnumerableField , ITranslatable
	{
		const string customDataType = "Lookup";
		private LookupType _lookupModel_ = LookupType.Base;
		private LookupLink _linkModel_ = LookupLink.None;

		public LookupAttribute() : base(customDataType)
        {
        }

		public LookupAttribute(LookupType lookupType) : base(customDataType)
		{
			_lookupModel_ = lookupType;
		}


		public LookupAttribute(LookupType lookupType, LookupLink lookupLink, object linkConfig) : base(customDataType)
		{
			_lookupModel_ = lookupType;
			_linkModel_ = lookupLink;
			LinkConfig = linkConfig;
		}

		public LookupAttribute(LookupType lookupType, LookupLink lookupLink, object linkConfig, string value) : base(customDataType)
		{
			_lookupModel_ = lookupType;
			_linkModel_ = lookupLink;
			LinkConfig = linkConfig;
			Value = value;
		}

		/// <summary>
		/// Get or set the type of popup, refer to <see cref="LookupType"/>
		/// </summary>
		public LookupType LookupModel { get { return _lookupModel_; } set { _lookupModel_ = value; } }

		/// <summary>
		/// Get or set whether the popup window is multi-select
		/// </summary>
		public bool Multi { get; set; }

		/// <summary>
		/// Get or set the type of popup link configuration, refer to <see cref="LookupLink"/>
		/// </summary>
		public LookupLink LinkModel { get { return _linkModel_; } set { _linkModel_ = value; } }

		/// <summary>
		/// Get or set the link configuration. 
		/// <para>You can use either anonymous type or string configuration.</para>
		/// </summary>
		public object LinkConfig { get; set; }

		/// <summary>
		/// Get or set the pop key field of the pop-up model.
		/// <para>This property is only valid when using the default pop-up model.</para> 
		/// <para>When this field is set, the model's primary key field or first field is used by default</para>
		/// </summary>
		public string Key { get; set; }

		/// <summary>
		/// Get or set the popup window's display field. 
		/// <para>This property is only valid when using the default popup window model. </para>
		/// <para>When this field is not set, the model's primary key field or first field is used by default</para>
		/// </summary>
		public string Value { get; set; }

		/// <summary>
		/// Get or set the filter condition. 
		/// <para>This property uses the default view and the enable connection will be passed back as a URL parameter.</para>
		/// <para> When the link is not enabled, this parameter can be found in the page component.</para>
		/// </summary>
		public string Condition { get; set; }

	    public char Split { get; set; } = ',';
		public Type ModelType { get; set; }
		public TranslatMode Mode { get; set; } = TranslatMode.Property | TranslatMode.Dict;

		public string FunctionOrPropertyName { get; set; }
	}

	/// <summary>
	/// Type of popup
	/// </summary>
	public enum LookupType {
		/// <summary>
		/// Standard mode, maximize the size of popups, use standard form to select data
		/// </summary>
		Base,

		/// <summary>
		/// Simplified mode, use a simple choice of layout
		/// </summary>
        Simple
    }

	/// <summary>
	/// Type of popup link configuration
	/// </summary>
	public enum LookupLink
	{
		/// <summary>
		/// Do not parse
		/// </summary>
		None,

		/// <summary>
		/// Routing mode
		/// </summary>
		Route,

		/// <summary>
		/// Link mode
		/// </summary>
		Link
	}
}
