using ICWorkGroupCustomization.Attributes;
using PX.Data;
using PX.Objects.CR;

namespace ICWorkGroupCustomization.DACExt
{
    public sealed class ICCRSetupExt : PXCacheExtension<CRSetup>
    {
        public static bool IsActive() => true;
        #region UsrICValidateOpportunities
        public abstract class usrICValidateOpportunities : PX.Data.BQL.BqlBool.Field<usrICValidateOpportunities> { }

        [PXDBBool]
        [PXDefault(false)]
        [PXUIField(DisplayName = "Validate Opportunities")]
        public bool? UsrICValidateOpportunities { get; set; }
        #endregion

        #region UsrICOpportunityValidation
        public abstract class usrICOpportunityValidation : PX.Data.BQL.BqlString.Field<usrICOpportunityValidation> { }

        [PXDBString(1, IsFixed = true)]
        [PXDefault(ICOpportunityValidationAttribute.Allow)]
        [ICOpportunityValidationAttribute.List]
        [PXUIField(DisplayName = "Opportunity Validation")]
        public string UsrICOpportunityValidation { get; set; }
        #endregion

    }
}
