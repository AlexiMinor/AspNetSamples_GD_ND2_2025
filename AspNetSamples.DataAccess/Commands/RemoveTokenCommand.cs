using MediatR;

namespace AspNetSamples.DataAccess.Commands;

public class RemoveTokenCommand : IRequest
{
   public Guid Id { get; set; }
}