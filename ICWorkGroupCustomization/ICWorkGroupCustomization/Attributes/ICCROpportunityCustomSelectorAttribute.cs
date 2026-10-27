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
using static PX.Data.EP.ActivityService;
using static PX.Data.PXGenericInqGrph;
using static PX.SM.SMPerformanceInfo;

namespace ICWorkGroupCustomization.Attributes
{
    public class ICCROpportunityCustomSelectorAttribute : PXCustomSelectorAttribute
    {
        public ICCROpportunityCustomSelectorAttribute() : base(
            typeof(CROpportunity.opportunityID),
                typeof(CROpportunity.subject),
                typeof(CROpportunity.status),
                typeof(CROpportunity.curyAmount),
                typeof(CROpportunity.curyID),
                typeof(CROpportunity.closeDate),
                typeof(CROpportunity.stageID),
                typeof(CROpportunity.classID),
                typeof(CROpportunity.isActive),
                typeof(BAccount.acctName),
                typeof(Contact.displayName)
            )
        {
            Headers = new string[] {
                "Opportunity ID",
                "Description",
                "Status",
                "Detail Total",
                "Currency",
                "Estimated Close Date",
                "Stage",
                "Opportunity Class",
                "Active",
                "Business Account Name",
                "Contact"
            };
            DescriptionField = typeof(CROpportunity.subject);
            Filterable = true;
        }
        protected virtual IEnumerable GetRecords()
        {
            List<CROpportunity> opportunities = new List<CROpportunity>();

            if (this._Graph is LeadMaint)
            {
                LeadMaint graph = (LeadMaint)this._Graph;

                var query = new SelectFrom<CROpportunity>.
                                LeftJoin<BAccount>.On<BAccount.bAccountID.IsEqual<CROpportunity.bAccountID>>.
                                LeftJoin<EPCompanyTreeMember>.On<EPCompanyTreeMember.workGroupID.IsEqual<CROpportunity.workgroupID>>.
                                LeftJoin<EPEmployee>.On<EPEmployee.defContactID.IsEqual<EPCompanyTreeMember.contactID>>.
                                LeftJoin<BAccountR>.On<BAccountR.bAccountID.IsEqual<CROpportunity.bAccountID>>.
                                LeftJoin<Contact>.On<Contact.contactID.IsEqual<CROpportunity.contactID>>.
                                Where<Brackets<BAccount.bAccountID.IsNull.Or<Match<BAccount, AccessInfo.userName.FromCurrent>>>.
                                    And<EPEmployee.userID.IsEqual<AccessInfo.userID.FromCurrent>.
                                    And<Brackets<EPCompanyTreeMember.membershipType.IsEqual<ICMemberShipTypeAttribute.permanent>.
                                            And<CROpportunity.ownerID.IsEqual<AccessInfo.contactID.FromCurrent>>.
                                        Or<EPCompanyTreeMember.membershipType.IsEqual<ICMemberShipTypeAttribute.temporary>>>>>>.
                                    View(graph);

                foreach (CROpportunity row in query.Select())
                {
                    opportunities.Add(row);
                }


            }

            return opportunities;
        }
    }
}
