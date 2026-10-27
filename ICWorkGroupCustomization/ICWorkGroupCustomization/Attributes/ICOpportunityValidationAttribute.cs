using PX.Data;
using PX.Data.BQL;

namespace ICWorkGroupCustomization.Attributes
{
    public class ICOpportunityValidationAttribute
    {
        #region Values
        public const string Allow = "A";
        public const string Warn = "W";
        public const string Block = "B";
        #endregion

        #region BQL Accessors
        public class allow : BqlString.Constant<allow> { public allow() : base(Allow) { } }

        public class warn : BqlString.Constant<warn> { public warn() : base(Warn) { } }

        public class block : BqlString.Constant<block> { public block() : base(Block) { } }
        #endregion

        #region Constructor
        public class ListAttribute : PXStringListAttribute
        {
            public ListAttribute()
                : base(new[] { Pair(Allow, "Allow"),
                               Pair(Warn, "Warn"),
                               Pair(Block, "Block")})
            { }
        }
        #endregion
    }
}
