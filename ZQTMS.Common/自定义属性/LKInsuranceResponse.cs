using System;
using System.Collections.Generic;
using System.Text;

namespace ZQTMS.Common
{
    public class LKInsuranceResponse
    {
        public string SequenceCode { get; set; }
        public string InsuranceBillCode { get; set; }
        public string InsuranceStatusCode { get; set; }
        public string ErrorDescription { get; set; }
        public string StartTime { get; set; }
        public string NetworkAccessAddress { get; set; }
        public string Remark { get; set; }

        public ChargeInformation[] ChargeInformation { get; set; }
        public FranchiseClause[] FranchiseClause { get; set; }

    }
    public class ChargeInformation
    {
        public string Rate { get; set; }
        public string MonetaryAmount { get; set; }
    }
    public class FranchiseClause
    {
        public string FreeText { get; set; }
    }
}
