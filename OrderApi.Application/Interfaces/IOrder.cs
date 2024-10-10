using System.Linq.Expressions;
using eCommerce.SharedLibrary.Interface;
using OrderApi.Domain.Entities;

namespace OrderApi.Application.Interfaces;

public interface IOrder : IGenericInterface<Order, Guid>
{
    Task<IEnumerable<Order>> GetAllAsync(Expression<Func<Order, bool>> predicate = null!);
}