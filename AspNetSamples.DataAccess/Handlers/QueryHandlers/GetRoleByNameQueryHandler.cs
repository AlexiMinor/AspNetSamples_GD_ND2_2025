using AspNetSamples.Core.Dto;
using AspNetSamples.DataAccess.Queries;
using AspNetSamples.Database;
using AspNetSamples.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AspNetSamples.DataAccess.Handlers.QueryHandlers;

public class GetRoleByNameQueryHandler : IRequestHandler<GetRoleByNameQuery, RoleDto?>
{
    private readonly GoodArticleAggregatorContext _context;
    private readonly RoleMapper _roleMapper;

    public GetRoleByNameQueryHandler(GoodArticleAggregatorContext context, RoleMapper roleMapper)
    {
        _context = context;
        _roleMapper = roleMapper;
    }

    public async Task<RoleDto?> Handle(GetRoleByNameQuery request, CancellationToken cancellationToken)
    {
        var role = await _context.Roles
            .AsNoTracking()
            .SingleOrDefaultAsync(role => role.Name.Equals(request.RoleName), cancellationToken);

        return _roleMapper.MapRoleToRoleDto(role);
    }
}