using AspNetSamples.Core.Dto;
using AspNetSamples.Database;
using AspNetSamples.Database.Entities;
using AspNetSamples.Mappers;
using AspNetSamples.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace AspNetSamples.Services
{
    public class SourceService : ISourceService
    {
        private readonly GoodArticleAggregatorContext _context;
        private readonly SourceMapper _sourceMapper;

        public SourceService(GoodArticleAggregatorContext context, SourceMapper sourceMapper)
        {
            _context = context;
            _sourceMapper = sourceMapper;
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

        public async Task<IEnumerable<SourceDto>> GetAllSourcesAsync(CancellationToken token = default)
        {
            return await _context.Sources
                .AsNoTracking()
                .Select(source => _sourceMapper.MapSourceToSourceDto(source))
                .ToListAsync(token);
        }

        public async Task<List<SourceDto>> GetAllSourcesWithRssAsync(CancellationToken token = default)
        {
            return await _context.Sources
                .AsNoTracking()
                .Where(source => !string.IsNullOrWhiteSpace(source.RssLink))
                .Select(source => _sourceMapper.MapSourceToSourceDto(source))
                .ToListAsync(token);
        }
    }
}