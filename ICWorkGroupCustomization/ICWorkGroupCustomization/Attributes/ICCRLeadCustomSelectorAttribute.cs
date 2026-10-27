using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.CR;
using PX.Objects.EP;
using PX.TM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PX.Data.PXGenericInqGrph;
using static PX.SM.SMPerformanceInfo;

namespace ICWorkGroupCustomization.Attributes
{
    public class ICPACRLeadCustomSelectorAttribute : PXCustomSelectorAttribute
    {
        public ICPACRLeadCustomSelectorAttribute() : base(
            typeof(CRLead.contactID),
            typeof(CRLead.memberName),
            typeof(CRLead.fullName),
            typeof(CRLead.salutation),
            typeof(CRLead.eMail),
            typeof(CRLead.phone1),
            typeof(CRLead.status),
            typeof(CRLead.duplicateStatus)
            )
        {
            Headers = new string[] { "Contact", "Account Name", "Job Title", "Email", "Phone 1", "Status", "Duplicate" };
            DescriptionField = typeof(CRLead.memberName);
            Filterable = true;
        }
        protected virtual IEnumerable GetRecords()
        {
            List<CRLead> leads = new List<CRLead>();

            if (this._Graph is LeadMaint)
            {
                LeadMaint graph = (LeadMaint)this._Graph;

                var query = new SelectFrom<CRLead>.
                                LeftJoin<BAccount>.On<BAccount.bAccountID.IsEqual<CRLead.bAccountID>>.
                                LeftJoin<EPCompanyTreeMember>.On<EPCompanyTreeMember.workGroupID.IsEqual<CRLead.workgroupID>>.
                                LeftJoin<EPEmployee>.On<EPEmployee.defContactID.IsEqual<EPCompanyTreeMember.contactID>>.
                                Where<Brackets<BAccount.bAccountID.IsNull.Or<Match<BAccount, AccessInfo.userName.FromCurrent>>>.
                                    And<EPEmployee.userID.IsEqual<AccessInfo.userID.FromCurrent>.
                                    And<Brackets<EPCompanyTreeMember.membershipType.IsEqual<ICMemberShipTypeAttribute.permanent>.
                                            And<CRLead.ownerID.IsEqual<AccessInfo.contactID.FromCurrent>>.
                                        Or<EPCompanyTreeMember.membershipType.IsEqual<ICMemberShipTypeAttribute.temporary>>>>>>.
                                    View(graph);

                foreach (CRLead row in query.Select())
                {
                    leads.Add(row);
                }


            }

            return leads;
        }
    }
}
