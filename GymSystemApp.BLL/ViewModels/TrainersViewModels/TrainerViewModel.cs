namespace GymManagementSystem.BLL.ViewModels.TrainersViewModels
{
    public class TrainerViewModel
    {
        public int Id { get; set; }
        public string? Photo { get; set; }
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public string DateOfBirth { get; set; } = default!;
        public string Gender { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string Specialties { get; set; } = default!;
    }
}
