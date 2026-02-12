namespace Villal.Application.Common.Interfaces
{
    public interface IUnitOfWork
    {
        IVillaRepository Villa { get; }

        Task SaveChangesAsync();
    }
}
