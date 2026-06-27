namespace GymSystemApp.DAL.Entities
{
    public class Member:GymUser
    {
        public string Photo { get; set; } = null!;
        public HealthRecord HealthRecord { get; set; } = null!;

        public ICollection<Membership> Memberships { get; set; }= new HashSet<Membership>();

        public ICollection<Booking> Bookings { get; set; }=new HashSet<Booking>();
    }
}