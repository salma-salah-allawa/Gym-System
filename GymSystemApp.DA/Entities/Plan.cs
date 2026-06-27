namespace GymSystemApp.DAL.Entities
{
    public class Plan:BaseEntity
    { 
        public string Name { get; set; } = null!;
        public string Description { get; set; }= null!;   
        public int DurationDays { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Membership> Memberships { get; set; } = new HashSet<Membership>();

    }
}
