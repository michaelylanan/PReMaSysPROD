using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations.Schema;

using PReMaSys.Data;
using System.ComponentModel.DataAnnotations;

namespace PReMaSys.Models
{
    public class Purchase
    {

        [Key]
        public int PurchaseId { get; set; }
        public virtual ApplicationUser? ApplicationUser { get; set; }
        public virtual AddToCart? AddToCart { get; set; }
        public string? EmployeeName { get; set; }
        public string? RewardImage { get; set; }
        public string? RewardName { get; set; }
        public decimal? RewardPrice { get; set; }
        public decimal? TotalPayment { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateModified { get; set; }
        public Stat? Stat { get; set; }

    }
    public enum Stat
    {
        Processing = 1,
        ClaimRewardNow = 2,
        Unclaimed = 3,
        Claimed = 4,
    }

}
