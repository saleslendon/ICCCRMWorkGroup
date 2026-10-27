using PX.Data;
using PX.Objects.CR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICWorkGroupCustomization.DACExt
{
    public sealed class ICCRLeadClassExt : PXCacheExtension<CRLeadClass>
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
