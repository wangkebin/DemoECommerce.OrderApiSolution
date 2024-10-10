using System.Linq.Expressions;
using eCommerce.SharedLibrary.Logs;
using eCommerce.SharedLibrary.Response;
using Microsoft.EntityFrameworkCore;
using OrderApi.Application.Interfaces;
using OrderApi.Domain.Entities;
using OrderApi.Infrastructure.Data;

namespace OrderApi.Infrastructure.Repositories;

public class OrderRespository(OrderDbContext context) : IOrder
{
    public async Task<Response> CreateAsync(Order entity)
    {
        try
        {
            var getOrder = await GetByAsync(_=>_.Id == entity.Id);
            if (getOrder is not null && getOrder.Id != Guid.Empty)
            {
                return new Response(false, $"Order with id {entity.Id} already exists");
            }
            var curOrder = context.Orders.Add(entity).Entity;
            await context.SaveChangesAsync();
            if (curOrder is not null && curOrder.Id != Guid.Empty)
            {
                return new Response(true, "Order with id {Id} has been created");
            }
            else
            {
                return new Response(false, "Order with id {Id} failed to be created");
            }

        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);
            return new Response(false, "error creating order");
        }
    }

    public async Task<Response> UpdateAsync(Order entity)
    {
        try
        {
            var getOrder = await GetByAsync(_ => _.Id == entity.Id);
            if (getOrder is null || getOrder.Id == Guid.Empty)
            {
                return new Response(false, $"Order with id {entity.Id} does not exists");
            }

            var curOrder = context.Orders.Update(entity).Entity;
            var rowupdatedcount = await context.SaveChangesAsync();
            if (rowupdatedcount > 0)
            {
                return new Response(true, "Order with id {Id} has been updated");
            }
            else
            {
                return new Response(false, "Order with id {Id} failed to update");
            }

        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);
            return new Response(false, "error updating order");
        }
    }

    public async Task<Response> DeleteAsync(Order entity)
    {
        try
        {
            var getOrder = await GetByAsync(_ => _.Id == entity.Id);
            if (getOrder is null || getOrder.Id == Guid.Empty)
            {
                return new Response(false, $"Order with id {entity.Id} does not exists");
            }

            var curOrder = context.Orders.Remove(entity).Entity;
            var rowupdatedcount = await context.SaveChangesAsync();
            if (rowupdatedcount > 0)
            {
                return new Response(true, "Order with id {Id} has been delete");
            }
            else
            {
                return new Response(false, "Order with id {Id} failed to delete");
            }
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);
            return new Response(false, "error deleting order");
        }
    }

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        try
        {
            var orders = await context.Orders.AsNoTracking().ToListAsync();
            return orders is not null ? orders : null!;
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);
            throw new Exception("cannot get all orders");
        }
    }

    public async Task<Order> GetByIdAsync(Guid id)
    {
        try
        {
            var order = await context.Orders.FindAsync(id);
            context.Entry(order).State = EntityState.Detached;
            return order is not null ? order : null!;
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);
            throw new Exception($"cannot get order with id {id}");
        }
    }

    public async Task<Order> GetByAsync(Expression<Func<Order, bool>> predicate)
    {
        try
        {
            var order = await context.Orders.Where(predicate).FirstOrDefaultAsync();
            return order is not null ? order : null!;
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);
            throw new Exception($"cannot get orders");   
        }
    }

    public async Task<IEnumerable<Order>> GetAllAsync(Expression<Func<Order, bool>> predicate = null)
    {
        try
        {
            var orders = context.Orders.Where(predicate);
            return orders is not null ? await orders.ToListAsync() : null!;
        }
        catch (Exception ex)
        {
            LogException.LogExceptions(ex);
            throw new Exception($"cannot get orders");
        }
    }
}