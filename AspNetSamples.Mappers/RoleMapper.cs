using AspNetSamples.Core.Dto;
using AspNetSamples.Database.Entities;
using Riok.Mapperly.Abstractions;

namespace AspNetSamples.Mappers;

[Mapper]
public partial class RoleMapper
{
    public partial RoleDto MapRoleToRoleDto(Role role);
    public partial Role MapRoleDtoToRole(RoleDto roleDto);
}