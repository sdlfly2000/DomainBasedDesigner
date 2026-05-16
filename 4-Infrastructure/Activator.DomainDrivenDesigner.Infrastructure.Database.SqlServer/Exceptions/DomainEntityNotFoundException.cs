namespace Activator.DomainDrivenDesigner.Infrastructure.Database.SqlServer.Exceptions;

public class DomainEntityNotFoundException(string message) : Exception(message)
{
    public static void ThrowIfNull<TEntity>(Guid entityId, TEntity? entity)
    { 
        if (entity == null)
        {
            throw new DomainEntityNotFoundException($"Entity of {typeof(TEntity).Name} with ID({entityId}) not found.");
        }
    }
}
