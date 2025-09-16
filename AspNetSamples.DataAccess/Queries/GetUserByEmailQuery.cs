using AspNetSamples.Core.Dto;
using MediatR;

namespace AspNetSamples.DataAccess.Queries;

public class GetUserByEmailQuery : IRequest<UserDto?>
{
    public string Email { get; set; }

}