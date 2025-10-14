using MediatR;

namespace AspNetSamples.DataAccess.Commands;

public class CreateRefreshTokenCommand : IRequest
{
    public Guid UserId { get; set; }
    public Guid Token { get; set; }
    public string? Device { get; set; }
}
