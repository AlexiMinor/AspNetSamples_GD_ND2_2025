using AspNetSamples.Core.Dto;
using MediatR;

namespace AspNetSamples.DataAccess.Commands;

public class RegisterUserCommand : IRequest
{
   public UserDto User { get; set; } 
}