using AspNetSamples.Core.Dto;
using MediatR;

namespace AspNetSamples.DataAccess.Queries;

public class GetRoleByNameQuery: IRequest<RoleDto>
{
    public string RoleName { get; set; }
}