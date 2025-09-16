using AspNetSamples.DataAccess.Commands;
using AspNetSamples.Database;
using AspNetSamples.Mappers;
using MediatR;

namespace AspNetSamples.DataAccess.Handlers.CommandHandlers;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand>
{
    private readonly GoodArticleAggregatorContext _context;
    private readonly UserMapper _userMapper;

    public RegisterUserCommandHandler(GoodArticleAggregatorContext context, UserMapper userMapper)
    {
        _context = context;
        _userMapper = userMapper;
    }

    public async Task Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var user = _userMapper.MapUserDtoToUser(command.User);
        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

