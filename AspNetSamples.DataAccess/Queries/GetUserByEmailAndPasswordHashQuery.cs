using AspNetSamples.Core.Dto;
using MediatR;

namespace AspNetSamples.DataAccess.Queries;

public class GetUserByEmailAndPasswordHashQuery : IRequest<UserDto?>
{
    public string Email { get; set; }
    public string PasswordHash { get; set; }

}