using ContactProvider.Data;
using ContactProvider.Entities;
using ContactProvider.Factories;
using ContactProvider.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace ContactProvider.Repositories;

public class ContactRepository(DataContext context, ILogger logger)
{
    private readonly DataContext _context = context;
    private readonly ILogger _logger = logger;

    public async Task<ResponseResult> CreateAsync(ContactEntity entity)
    {
        try
        {
            await _context.Contacts.AddAsync(entity);
            await _context.SaveChangesAsync();
            return ResponseFactory.Ok(entity);
        }
        catch (Exception ex)
        {
            _logger.LogDebug($"ERROR : ContactProvider.ContactRepository.CreateAsync() : {ex.Message}");
            return ResponseFactory.Error(ex.Message);
        }
    }
    public async Task<ResponseResult> GetAllAsync()
    {
        try
        {
            IEnumerable<ContactEntity> list = await _context.Contacts.ToListAsync();
            if (!list.Any())
                return ResponseFactory.NotFound();
            return ResponseFactory.Ok(list);
        }
        catch (Exception ex)
        {
            _logger.LogDebug($"ERROR : ContactProvider.ContactRepository.GetAllAsync() : {ex.Message}");
            return ResponseFactory.Error(ex.Message);
        }
    }
    public async Task<ResponseResult> GetAsync(Expression<Func<ContactEntity, bool>> predicate)
    {
        try
        {
            var entity = await _context.Contacts.FirstOrDefaultAsync(predicate);
            if (entity == null)
                return ResponseFactory.NotFound();
            return ResponseFactory.Ok(entity);
        }
        catch (Exception ex)
        {
            _logger.LogDebug($"ERROR : ContactProvider.ContactRepository.GetAsync() : {ex.Message}");
            return ResponseFactory.Error(ex.Message);
        }
    }
    public async Task<ResponseResult> UpdateAsync(Expression<Func<ContactEntity, bool>> predicate, ContactEntity entity)
    {
        try
        {
            var result = await ExistsAsync(predicate);
            if (result.StatusCode == StatusCode.EXISTS)
            {
                _context.Contacts.Update(entity);
                await _context.SaveChangesAsync();
                return ResponseFactory.Ok(entity);
            }
            else if (result.StatusCode == StatusCode.NOT_FOUND)
                return ResponseFactory.NotFound();
            return ResponseFactory.Error();
        }
        catch (Exception ex)
        {
            _logger.LogDebug($"ERROR : ContactProvider.ContactRepository.UpdateAsync() : {ex.Message}");
            return ResponseFactory.Error(ex.Message);
        }
    }
    public async Task<ResponseResult> DeleteAsync(Expression<Func<ContactEntity, bool>> predicate)
    {
        try
        {
            var result = await GetAsync(predicate);
            if(result.StatusCode == StatusCode.OK)
            {
                var entity = (ContactEntity)result.ContentResult!;
                _context.Contacts.Remove(entity);
                await _context.SaveChangesAsync();
                return ResponseFactory.Ok();
            }
            else if(result.StatusCode == StatusCode.NOT_FOUND)
                return ResponseFactory.NotFound();
            return ResponseFactory.Error();
        }
        catch (Exception ex)
        {
            _logger.LogDebug($"ERROR : ContactProvider.ContactRepository.DeleteAsync() : {ex.Message}");
            return ResponseFactory.Error(ex.Message);
        }
    }
    public async Task<ResponseResult> ExistsAsync(Expression<Func<ContactEntity, bool>> predicate)
    {
        try
        {
            if (await _context.Contacts.AnyAsync(predicate))
                return ResponseFactory.Exists();
            return ResponseFactory.NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogDebug($"ERROR : ContactProvider.ContactRepository.ExistsAsync() : {ex.Message}");
            return ResponseFactory.Error(ex.Message);
        }
    }
}
