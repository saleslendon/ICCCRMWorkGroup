using ICWorkGroupCustomization.Attributes;
using ICWorkGroupCustomization.DACExt;
using PX.Data;
using PX.Objects.CR;
using PX.Objects.EP;
using static PX.SM.EMailAccount.authenticationMethod;

namespace ICWorkGroupCustomization.GraphExt
{
    public class OpportunityMaintExt : PXGraphExtension<OpportunityMaint>
    {
        #region Messages
        public const string DuplicateBAccountWarning = "A duplicate of the Business Account exists in an Open Opportunity";
        public const string DuplicateBAccountError = "Please select a different Business Account. A duplicate of the Business Account exists in an Open Opportunity";
        #endregion

        protected virtual void _(Events.RowSelected<CROpportunity> e)
        {
            CROpportunity row = e.Row;
            if (row == null) return;

            CROpportunityClass opportunityClass = CROpportunityClass.PK.Find(Base, row.ClassID);

            if (opportunityClass != null)
            {
                ICCROpportunityClassExt opportunityClassExt = opportunityClass.GetExtension<ICCROpportunityClassExt>();

                PXUIFieldAttribute.SetRequired<CROpportunity.workgroupID>(e.Cache, opportunityClassExt?.UsrICRestrictByWorkgroup ?? false);
            }

            if(row.OwnerID != null) {
                BAccount bAccount = PXSelect<BAccount,
                Where<BAccount.defContactID, Equal<Required<BAccount.defContactID>>>>.Select(Base, row.OwnerID);
                if (bAccount != null)
                {
                    EPEmployee emp = PXSelect<EPEmployee,
                    Where<EPEmployee.bAccountID, Equal<Required<EPEmployee.bAccountID>>>>.Select(Base, bAccount.BAccountID);

                    if (emp != null && emp.DefaultWorkgroupID != null)
                    {
                        e.Cache.SetValueExt<CROpportunity.workgroupID>(row, emp.DefaultWorkgroupID);
                    }
                    else
                    {
                        e.Cache.SetValueExt<CROpportunity.workgroupID>(row, null);
                    }
                }

            }

            SetDuplicateBAccountMessage(e.Cache, row);
        }

        protected virtual void _(Events.RowPersisting<CROpportunity> e)
        {
            CROpportunity row = e.Row;
            if (row == null) return;

            CROpportunityClass opportunityClass = CROpportunityClass.PK.Find(Base, row.ClassID);

            if (opportunityClass != null)
            {
                ICCROpportunityClassExt opportunityClassExt = opportunityClass.GetExtension<ICCROpportunityClassExt>();
                if (opportunityClassExt?.UsrICRestrictByWorkgroup == true && row.WorkgroupID == null)
                {
                    e.Cache.RaiseExceptionHandling<CROpportunity.workgroupID>(row, row.WorkgroupID, new PXException("Error: '{0}' cannot be empty", "Workgroup", PXErrorLevel.Error));
                }
            }

            if (e.Operation.Command() != PXDBOperation.Delete
                && GetOpportunityValidation() == ICOpportunityValidationAttribute.Block
                && HasDuplicateBAccount(row))
            {
                throw new PXRowPersistingException(nameof(CROpportunity.bAccountID), row.BAccountID, DuplicateBAccountError);
            }
        }

        #region Business Account Validation
        /// <summary>
        /// Returns the Opportunity Validation mode from Customer Management Preferences,
        /// or null when Validate Opportunities is unticked.
        /// </summary>
        protected virtual string GetOpportunityValidation()
        {
            CRSetup setup = Base.Setup.Current;
            ICCRSetupExt setupExt = setup?.GetExtension<ICCRSetupExt>();

            if (setupExt?.UsrICValidateOpportunities != true) return null;

            return setupExt.UsrICOpportunityValidation;
        }

        /// <summary>
        /// Checks whether another "New" or "Open" opportunity uses the same Business Account.
        /// </summary>
        protected virtual bool HasDuplicateBAccount(CROpportunity row)
        {
            if (row?.BAccountID == null) return false;

            CROpportunity duplicate = PXSelectReadonly<CROpportunity,
                Where<CROpportunity.bAccountID, Equal<Required<CROpportunity.bAccountID>>,
                    And<CROpportunity.opportunityID, NotEqual<Required<CROpportunity.opportunityID>>,
                    And<Where<CROpportunity.status, Equal<OpportunityStatus.@new>,
                        Or<CROpportunity.status, Equal<OpportunityStatus.open>>>>>>>
                .SelectWindowed(Base, 0, 1, row.BAccountID, row.OpportunityID ?? string.Empty);

            return duplicate != null;
        }

        protected virtual void SetDuplicateBAccountMessage(PXCache cache, CROpportunity row)
        {
            string validation = GetOpportunityValidation();
            bool isDuplicate = (validation == ICOpportunityValidationAttribute.Warn || validation == ICOpportunityValidationAttribute.Block)
                && HasDuplicateBAccount(row);

            if (isDuplicate && validation == ICOpportunityValidationAttribute.Warn)
            {
                PXUIFieldAttribute.SetWarning<CROpportunity.bAccountID>(cache, row, DuplicateBAccountWarning);
            }
            else if (isDuplicate && validation == ICOpportunityValidationAttribute.Block)
            {
                PXUIFieldAttribute.SetError<CROpportunity.bAccountID>(cache, row, DuplicateBAccountError);
            }
            else
            {
                // Clear only our own message so unrelated errors on the field are kept
                string current = PXUIFieldAttribute.GetError<CROpportunity.bAccountID>(cache, row)
                    ?? PXUIFieldAttribute.GetWarning<CROpportunity.bAccountID>(cache, row);

                if (current == DuplicateBAccountWarning || current == DuplicateBAccountError)
                {
                    PXUIFieldAttribute.SetError<CROpportunity.bAccountID>(cache, row, null);
                }
            }
        }
        #endregion

        //protected virtual void _(Events.FieldUpdated<CROpportunity.ownerID> e)
        //{
        //    CROpportunity row = (CROpportunity)e.Row;
        //    if (row == null) return;

        //    // Do not overwrite existing Workgroup
        //    //if (row.WorkgroupID != null) return;

        //    //int? ownerContactID = e.NewValue as int?;
        //    //if (ownerContactID == null) return;

        //    BAccount bAccount = PXSelect<BAccount,
        //        Where<BAccount.defContactID, Equal<Required<BAccount.defContactID>>>>.Select(Base, e.NewValue);
        //    if(bAccount != null)
        //    {
        //        EPEmployee emp = PXSelect<EPEmployee,
        //        Where<EPEmployee.bAccountID, Equal<Required<EPEmployee.bAccountID>>>>.Select(Base, bAccount.BAccountID);

        //        if (emp != null && emp.DefaultWorkgroupID != null)
        //        {
        //            e.Cache.SetValueExt<CROpportunity.workgroupID>(row, emp.DefaultWorkgroupID);
        //        }
        //    }
        //}
    }
}
