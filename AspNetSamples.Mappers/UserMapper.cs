using AspNetSamples.Core.Dto;
using AspNetSamples.Database.Entities;
using AspNetSamples.Models;
using Riok.Mapperly.Abstractions;

namespace AspNetSamples.Mappers;

[Mapper]
public partial class UserMapper
{
    public partial UserDto? MapUserToUserDto(User? user);

    public partial UserDto RegisterModelToUserDto(RegisterModel article);


    public partial User MapUserDtoToUser(UserDto commandUser);
}