using Application.Dto;
using Application.Mappers.Interfaces;
using Domain.Entiites.Users;

namespace Application.Mappers.Implementations;

public class UserMapper : IEntityDtoMapper<User, UserDto> {

    public UserDto MapToDto(User entity) {
        return new UserDto.Complete(entity.Name, entity.Email, entity.BirthDay);
    }

    public User MapToEntity(UserDto dto) {
        return dto switch {
            UserDto.Basic user => new() { Name = user.Name },
            UserDto.WithEmail user => new() { Name = user.Name, Email = user.Email },
            UserDto.WithBirthDay user => new() { Name = user.Name, BirthDay = user.BirthDay },
            UserDto.Complete user => new() {
                Name = user.Name,
                Email = user.Email,
                BirthDay = user.BirthDay,
            },
            _ => throw new Exception("Invalid UserDto object"),
        };
    }
}

