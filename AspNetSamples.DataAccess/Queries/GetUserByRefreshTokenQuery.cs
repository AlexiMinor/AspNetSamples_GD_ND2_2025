using AspNetSamples.Core.Dto;
using MediatR;

namespace AspNetSamples.DataAccess.Queries;

public class GetUserByRefreshTokenQuery : IRequest<UserDto?>
{
    public Guid RefreshToken { get; set; }

}