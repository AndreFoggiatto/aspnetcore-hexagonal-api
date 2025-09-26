using aspnetcore_hexagonal_api.Features.FeatureExample.Domain.Models;
using aspnetcore_hexagonal_api.Features.FeatureExample.Interfaces;
using aspnetcore_hexagonal_api.Features.FeatureExample.Models;
using Microsoft.EntityFrameworkCore;

namespace aspnetcore_hexagonal_api.Features.FeatureExample.Adapters;

public class FeatureExampleRepository : IFeatureExampleRepository
{
    private readonly ApplicationDbContext _context;

    public FeatureExampleRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<FeatureExampleEntity?> GetByIdAsync(int id)
    {
        return await _context.FeatureExamples
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id && x.IsActive);
    }

    public async Task<FeatureExampleEntity> CreateAsync(FeatureExampleEntity entity)
    {
        entity.CreatedAt = DateTime.UtcNow;
        entity.IsActive = true;

        _context.FeatureExamples.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<FeatureExampleEntity> UpdateAsync(FeatureExampleEntity entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;

        _context.FeatureExamples.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.FeatureExamples.FindAsync(id);
        if (entity == null) return false;

        entity.IsActive = false;
        entity.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<(IEnumerable<FeatureExampleEntity> Items, int TotalCount)> GetAllAsync(FeatureExampleQueryRequest request)
    {
        var query = _context.FeatureExamples
            .AsNoTracking()
            .Where(x => x.IsActive);

        if (!string.IsNullOrEmpty(request.Name))
        {
            query = query.Where(x => x.Name.Contains(request.Name));
        }

        if (request.Status.HasValue)
        {
            query = query.Where(x => x.Status == request.Status.Value);
        }

        if (request.CreatedFrom.HasValue)
        {
            query = query.Where(x => x.CreatedAt >= request.CreatedFrom.Value);
        }

        if (request.CreatedTo.HasValue)
        {
            query = query.Where(x => x.CreatedAt <= request.CreatedTo.Value);
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.FeatureExamples
            .AsNoTracking()
            .AnyAsync(x => x.Id == id && x.IsActive);
    }
}