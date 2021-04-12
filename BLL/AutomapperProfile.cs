using AutoMapper;
using BLL.Models;
using DAL.Entities;
using System;
using System.Linq;

namespace BLL
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<RegisterUserModel, ApplicationUser>()
                .ForMember(p => p.UserProfile, c => c.MapFrom(profile => new UserProfile 
                { 
                    FirstName = profile.FirstName,
                    LastName = profile.LastName
                }))
                .ForMember(p=>p.UserName, c=>c.MapFrom(u => u.Email))
                .ForMember(p=>p.NickName, c=>c.MapFrom(n=>n.NickName.ToUpper()));

            CreateMap<ApplicationUser, UserModel>()
                .ForMember(p => p.ProfileId, c => c.MapFrom(profile => profile.UserProfile.Id))
                .ForMember(p => p.FirstName, c => c.MapFrom(firstName => firstName.UserProfile.FirstName))
                .ForMember(p => p.LastName, c => c.MapFrom(lastName => lastName.UserProfile.LastName))
                .ForMember(p=>p.About, c=>c.MapFrom(about=>about.UserProfile.About))
                .ForMember(p => p.MessagesIds, c => c.MapFrom(message => message.Messages.Select(m => m.Id)))
                .ReverseMap();

            CreateMap<Topic, TopicModel>()
                .ForMember(p => p.MessagesIds, c => c.MapFrom(messages => messages.Messages.Select(m => m.Id)))
                .ReverseMap();

            CreateMap<ApplicationUser, UserActivityModel>()
                .ForMember(p=>p.NickName, c=>c.MapFrom(nick=>nick.NickName))
                .ForMember(p => p.MessagesCount, c => c.MapFrom(messages => messages.Messages.Count));


            CreateMap<Message, CurrentUserMessageModel>()
                .ForMember(p => p.TopicTitle, c => c.MapFrom(t => t.Topic.Title));

            CreateMap<Message, MessageModel>()
                .ReverseMap();
        }
    }
}
