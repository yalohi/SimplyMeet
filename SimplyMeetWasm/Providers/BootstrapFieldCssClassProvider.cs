using System.Linq;
using Microsoft.AspNetCore.Components.Forms;

namespace SimplyMeetWasm.Providers
{
	public class BootstrapFieldCssClassProvider : FieldCssClassProvider
	{
		//===========================================================================================
		// Public Methods
		//===========================================================================================
		public override string GetFieldCssClass(EditContext InContext, in FieldIdentifier InFieldIdentifier)
		{
			var IsValid = !InContext.GetValidationMessages(InFieldIdentifier).Any();
			return IsValid ? "is-valid" : "is-invalid";
		}
	}
}