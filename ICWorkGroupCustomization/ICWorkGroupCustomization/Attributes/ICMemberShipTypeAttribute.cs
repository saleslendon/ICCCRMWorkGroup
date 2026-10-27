using PX.Data;
using PX.Data.BQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICWorkGroupCustomization.Attributes
{
    public class ICMemberShipTypeAttribute
    {
        #region Values
        public const string Permanent = "PERM";
        public const string Temporary = "TEMP";
        #endregion

        #region BQL Accessors
        public class permanent : BqlString.Constant<permanent> { public permanent() : base(Permanent) { } }

        public class temporary : BqlString.Constant<temporary> { public temporary() : base(Temporary) { } }
        #endregion

        #region Constructor
        public class ListAttribute : PXStringListAttribute
        {
            public ListAttribute()
                : base(new[] { Pair(Permanent, "Permanent"),
                               Pair(Temporary, "Temporary")})
            { }
        }
        #endregion
    }
}
