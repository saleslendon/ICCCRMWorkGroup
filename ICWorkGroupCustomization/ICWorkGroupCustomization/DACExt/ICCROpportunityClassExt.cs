using PX.Data;
using PX.Objects.CR;

namespace ICWorkGroupCustomization.DACExt
{
    public sealed class ICCROpportunityClassExt : PXCacheExtension<CROpportunityClass>
    {
        public static bool IsActive() => true;
        #region UsrICRestrictByWorkgroup
        public abstract class usrICRestrictByWorkgroup : PX.Data.BQL.BqlBool.Field<usrICRestrictByWorkgroup> { }

        [PXDBBool]
        [PXDefault(false)]
        [PXUIField(DisplayName = "Restrict by Workgroup")]
        public bool? UsrICRestrictByWorkgroup { get; set; }
        #endregion

    }
}
