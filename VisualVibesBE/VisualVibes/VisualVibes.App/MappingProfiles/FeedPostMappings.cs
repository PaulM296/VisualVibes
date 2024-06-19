﻿using AutoMapper;
using VisualVibes.App.DTOs.FeedDtos;
using VisualVibes.Domain.Models;
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
             .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
             .ForMember(dest => dest.PostImageId, opt => opt.MapFrom(src => src.ImageId))
             .ForMember(dest => dest.UserProfileImageId, opt => opt.MapFrom(src => src.User.UserProfile.ImageId))
             .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments))
             .ForMember(dest => dest.isModerated, opt => opt.MapFrom(src => src.isModerated))
             .ForMember(dest => dest.Reactions, opt => opt.MapFrom(src => src.Reactions));

            CreateMap<FeedPost, FeedPostDto>()
                .ForMember(dest => dest.PostId, opt => opt.MapFrom(src => src.Post.Id))
                .ForMember(dest => dest.ReactionCount, opt => opt.MapFrom(src => src.Post.Reactions.Count))
                .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(src => src.Post.Comments.Count))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Post.User.UserName))
                .ForMember(dest => dest.PostImageId, opt => opt.MapFrom(src => src.Post.ImageId))
                .ForMember(dest => dest.UserProfileImageId, opt => opt.MapFrom(src => src.Post.User.UserProfile.ImageId))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Post.Comments))
                .ForMember(dest => dest.Reactions, opt => opt.MapFrom(src => src.Post.Reactions))
                .ForMember(dest => dest.Caption, opt => opt.MapFrom(src => src.Post.Caption))
                .ForMember(dest => dest.isModerated, opt => opt.MapFrom(src => src.Post.isModerated))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.Post.CreatedAt));
        }
    }
}
