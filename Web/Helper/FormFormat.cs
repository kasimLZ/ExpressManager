using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Helper
{
	[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
	public class FormFormatAttribute : CustomModelBinderAttribute
	{

		public override IModelBinder GetBinder()
		{
			return new FormFormatModelBinder();
		}
	}

	public class FormFormatModelBinder : DefaultModelBinder, IModelBinder
	{

		public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			var model = base.BindModel(controllerContext, bindingContext);

			ModelHelper.UpdateModel(model, controllerContext.HttpContext.Request.Form);

			BindAttribute bindAttr = (BindAttribute)GetTypeDescriptor(controllerContext, bindingContext).GetAttributes()[typeof(BindAttribute)];

			bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => model, bindingContext.ModelType);
			bindingContext.PropertyFilter = (bindAttr != null) ? propertyName => (bindAttr.IsPropertyAllowed(propertyName) && bindingContext.PropertyFilter(propertyName)) : bindingContext.PropertyFilter;

			string ModelName = bindingContext.ModelName;
			bindingContext.ModelName = string.Empty;
			OnModelUpdated(controllerContext, bindingContext);
			bindingContext.ModelName = ModelName;

			return bindingContext.Model;
		}


		protected override void OnModelUpdated(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			Dictionary<string, bool> startedValid = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);

			foreach (ModelValidationResult validationResult in ModelValidator.GetModelValidator(bindingContext.ModelMetadata, controllerContext).Validate(null))
			{
				string subPropertyName = CreateSubPropertyName(bindingContext.ModelName, validationResult.MemberName);

				if (!startedValid.ContainsKey(subPropertyName))
				{
					startedValid[subPropertyName] = bindingContext.ModelState.IsValidField(subPropertyName);
				}

				if (startedValid[subPropertyName])
				{
					bindingContext.ModelState.AddModelError(subPropertyName, validationResult.Message);
				}
			}
		}
	}
}