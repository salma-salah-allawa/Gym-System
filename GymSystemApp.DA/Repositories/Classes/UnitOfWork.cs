using GymSystemApp.Contexts;
using GymSystemApp.DAL.Entities;
using GymSystemApp.DAL.Repositories.Contracts;


namespace GymSystemApp.DAL.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GymAppDbContext context;
        private readonly Dictionary<string, object> repos = [];
        public UnitOfWork(GymAppDbContext context)
        {
            this.context = context;
            SessionRepo = new SessionRepo(context);
            MembershipRepo = new MembershipRepo(context);
            BookingRepo = new BookingRepo(context);
        }

        public ISessionRepo SessionRepo {  get; }
        public IMembershipRepo MembershipRepo { get; }

        public IBookingRepo BookingRepo { get; }

        public IGenericRepo<TEntity> GetRepo<TEntity>() where TEntity : BaseEntity, new()
        {
            var TypeName = typeof(TEntity).Name;
            if(repos.TryGetValue(TypeName, out object OldRepo)) 
                return (IGenericRepo<TEntity>)OldRepo;
            
            var NewRepo= new GenericRepo<TEntity>(context);
            repos[TypeName] = NewRepo;
            return NewRepo;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }
    }
}
