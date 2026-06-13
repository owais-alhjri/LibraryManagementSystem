using LMS.Domain.Enums;

namespace LMS.Application.DTOs.User;

public class UpdateRoleRequestDto
{
    public Roles Role { get; set; }
}