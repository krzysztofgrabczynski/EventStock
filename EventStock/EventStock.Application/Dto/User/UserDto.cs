﻿using AutoMapper;
using EventStock.Application.Mapping;

namespace EventStock.Application.Dto.User
{
    public class UserDto : IMapFrom<EventStock.Domain.Models.User>
    {
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<EventStock.Domain.Models.User, UserDto>();
        }
    }
}
