using MediatR;

namespace AspNetSamples.DataAccess.Commands;

public class RevokeTokenCommand : IRequest
{
   public Guid Id { get; set; }
}