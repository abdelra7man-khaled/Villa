using Villal.Domain.Entities;

namespace Villal.Application.Common.Interfaces
{
    public interface IAmenityRepository : IRepository<Amenity>
    {
        void Update(Amenity entity);
    }
}
