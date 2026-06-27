using System;
using System.Collections.Generic;
using System.Text;

namespace GymSystemApp.BLL.ViewModels.MembershipsViewModels
{
    public class CreateMembershipViewModel
    {
        public int MemberId { get; set; }
        public int PlanId { get; set; }
        public DateTime? StartDate {  get; set; }
    }
}
