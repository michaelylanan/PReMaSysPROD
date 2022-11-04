using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;
using PReMaSys.Data;

namespace PReMaSys.Models
{
    public class Rewards
    {
        [Key]
        public int RewardsInformationId { get;set; }
        //Foreign Key of Id in AspNetUsers
        public virtual ApplicationUser ApplicationUser { get; set; }

        [Display(Name = "Image")]
        public string Picture { get;set; }

        [Required(ErrorMessage = "Required.")]
        [Display(Name = "Reward Name")]
        public string RewardName { get;set; }

        [Required(ErrorMessage = "Required*")]
        public Category Category { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Range(0.00, 100000.00, ErrorMessage ="Invalid Cost Range.")]
        [Display(Name = "Reward Cost")]
        public decimal RewardCost { get; set; }
       
        [Range(0.00, 100000.00, ErrorMessage = "Invalid Point Cost Range.")]
        [Display(Name = "Point Cost")]
        public decimal PointsCost { get; set; }

        public Status? Status { get; set; }

        [Display(Name ="Date Added")]
        public DateTime DateAdded { get; set; }

        [Display(Name ="Date Modified")]
        public DateTime? DateModified { get; set; }

       /* [Required(ErrorMessage ="Required.")]
        public virtual Category Category { get; set; }*/

    }

    public enum Category
    {
        Food =1,
        Travel =2,
        Discounts =3,
        Others =4,
    }

    public enum Status
    {
        OnReview = 1,
        Approved = 2,
        Reject = 3,
    }
}
