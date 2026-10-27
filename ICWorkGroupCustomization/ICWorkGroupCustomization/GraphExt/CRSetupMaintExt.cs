using ICWorkGroupCustomization.DACExt;
using PX.Data;
using PX.Objects.CR;

namespace ICWorkGroupCustomization.GraphExt
{
    public class CRSetupMaintExt : PXGraphExtension<CRSetupMaint>
    {
        protected virtual void _(Events.RowSelected<CRSetup> e)
        {
            CRSetup row = e.Row;
            if (row == null) return;

            ICCRSetupExt setupExt = row.GetExtension<ICCRSetupExt>();

            PXUIFieldAttribute.SetEnabled<ICCRSetupExt.usrICOpportunityValidation>(e.Cache, row, setupExt?.UsrICValidateOpportunities == true);
        }
    }
}
