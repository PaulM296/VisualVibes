﻿using AutoMapper;
using VisualVibes.App.DTOs.FeedDtos;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.MappingProfiles
{
    public class FeedPostMappings : Profile
    {
        public FeedPostMappings() 
        {
            CreateMap<Post, FeedPostDto>()
             .ForMember(dest => dest.PostId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ReactionCount, opt => opt.MapFrom(src => src.Reactions.Count))
            .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(src => src.Comments.Count))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));
        }
    }
}