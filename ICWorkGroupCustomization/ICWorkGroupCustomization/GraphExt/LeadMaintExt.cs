using ICWorkGroupCustomization.DACExt;
using PX.Data;
using PX.Objects.CR;
using static PX.SM.EMailAccount.authenticationMethod;

namespace ICWorkGroupCustomization.GraphExt
{
    public class LeadMaintExt : PXGraphExtension<LeadMaint>
    {
        protected virtual void _(Events.RowSelected<CRLead> e)
        {
            CRLead row = e.Row;
            if (row == null) return;

            CRLeadClass leadClass = CRLeadClass.PK.Find(Base, row.ClassID);

            if (leadClass != null)
            {
                ICCRLeadClassExt leadClassExt = leadClass.GetExtension<ICCRLeadClassExt>();

                PXUIFieldAttribute.SetRequired<CRLead.workgroupID>(e.Cache, leadClassExt?.UsrICRestrictByWorkgroup ?? false);
            }
        }

        protected virtual void _(Events.RowPersisting<CRLead> e)
        {
            CRLead row = e.Row;
            if (row == null) return;

            CRLeadClass leadClass = CRLeadClass.PK.Find(Base, row.ClassID);

            if (leadClass != null)
            {
                ICCRLeadClassExt leadClassExt = leadClass.GetExtension<ICCRLeadClassExt>();
                if (leadClassExt?.UsrICRestrictByWorkgroup == true && row.WorkgroupID == null)
                {
                    e.Cache.RaiseExceptionHandling<CRLead.workgroupID>(row, row.WorkgroupID, new PXException("Error: '{0}' cannot be empty", "Workgroup", PXErrorLevel.Error));
                }
            }
        }
    }
}
