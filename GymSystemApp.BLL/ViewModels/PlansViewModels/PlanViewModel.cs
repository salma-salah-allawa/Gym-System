namespace GymManagementSystem.BLL.ViewModels.PlansViewModels
{
    public class PlanViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public int DurationDays { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
    }
}
