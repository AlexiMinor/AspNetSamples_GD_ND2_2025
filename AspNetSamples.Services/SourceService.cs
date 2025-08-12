using AspNetSamples.Core.Dto;
using AspNetSamples.Database;
using AspNetSamples.Database.Entities;
using AspNetSamples.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace AspNetSamples.Services
{
    public class SourceService : ISourceService
    {
        private readonly GoodArticleAggregatorContext _context;

        public SourceService(GoodArticleAggregatorContext context)
        {
            _context = context;
        }

        public async Task AddSourcesAsync(IEnumerable<SourceDto> sourceDtos, CancellationToken token = default)
        {
            var sources = sourceDtos.Select(source => new Source
            {
                Id = source.Id,
                Name = source.Name,
                DomainName = source.DomainName
            });

            await _context.Sources.AddRangeAsync(sources, token);
            await _context.SaveChangesAsync(token);
        }

        public async Task<IEnumerable<SourceDto>> GetAllAsync(CancellationToken token = default)
        {
            var sources = await _context.Sources.AsNoTracking().Select(
                source => new SourceDto { Id = source.Id, Name = source.Name, DomainName = source.DomainName })
                .ToListAsync(token);

            return sources;
        }
    }
}