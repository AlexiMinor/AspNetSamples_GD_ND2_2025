using AspNetSamples.Core.Dto;
using AspNetSamples.DataAccess.Queries;
using AspNetSamples.Database;
using AspNetSamples.Mappers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AspNetSamples.DataAccess.Handlers.QueryHandlers;

public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, UserDto?>
{
    private readonly GoodArticleAggregatorContext _context;
    private readonly UserMapper _userMapper;

    public GetUserByEmailQueryHandler(GoodArticleAggregatorContext context, UserMapper userMapper)
    {
        _context = context;
        _userMapper = userMapper;
    }

    public async Task<UserDto?> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .AsNoTracking()
            .Include(us => us.Role)
            .SingleOrDefaultAsync(user => user.Email.Equals(request.Email), cancellationToken);

        return _userMapper.MapUserToUserDto(user);
    }
}