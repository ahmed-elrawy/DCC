using System.Linq;
using AutoMapper;
using DatingApp.API.Dtos;
using DCC.API.Dtos;
using DCC.API.Helper;
using DCC.API.Model;

namespace DatingApp.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForListDto>()
                .ForMember(dest => dest.PhotoUrl, opt =>
                {
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
                })
                .ForMember(dest => dest.Age, opt =>
                {
                    opt.ResolveUsing(d => d.DateOfBirth?.CalculateAge());
                });
            CreateMap<User, UserForDetailedDto>()
                .ForMember(dest => dest.PhotoUrl, opt =>
                {
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
                })
                .ForMember(dest => dest.Age, opt =>
                {
                    opt.ResolveUsing(d => d.DateOfBirth?.CalculateAge());
                });
            CreateMap<Photo, PhotosForDetailedDto>();
            CreateMap<Photo, PhotoForReturnDto>();
            CreateMap<UserForUpdateDto, User>();
            //  CreateMap<Photo, PhotoForReturnDto>();
            CreateMap<PhotoForCreationDto, Photo>();
            CreateMap<UserForRegisterDto, User>();
            CreateMap<MessageForCreationDto, Message>().ReverseMap();
            CreateMap<Message, MessageToReturnDto>()
            .ForMember(m => m.SenderPhotoUrl, opt => opt
            .MapFrom(u => u.Sender.Photos.FirstOrDefault(p => p.IsMain).Url))
            .ForMember(m => m.RecipientPhotoUrl, opt => opt
            .MapFrom(u => u.Recipient.Photos.FirstOrDefault(p => p.IsMain).Url));

            CreateMap<Message, MessageNotification>()
           .ForMember(m => m.SenderPhotoUrl, opt => opt
           .MapFrom(u => u.Sender.Photos.FirstOrDefault(p => p.IsMain).Url))
           .ForMember(m => m.RecipientPhotoUrl, opt => opt
           .MapFrom(u => u.Recipient.Photos.FirstOrDefault(p => p.IsMain).Url));


            CreateMap<BodyAreaForCreationDto, BodyAreas>();
            CreateMap<DrugForCreationDto, Drug>();
            CreateMap<TreatmentBulletinForCreationDto, TreatmentBulletin>().ReverseMap();
            CreateMap<SymptomForCreationDto, Symptom>();
            CreateMap<TreatmentBulletin, TreatmentBulletinForReturnDto>();

            CreateMap<TreatmentBulletin, TreatmentBulletinForReturnDto>()
                .ForMember(dest => dest.DrugName,
                    opts => opts.MapFrom(
                        src => src.Drug.DrugName
                    )).ReverseMap();

            CreateMap<Drug, DrugForReturnDto>();
            CreateMap<SymptomForCreationDto, Symptom>();
            CreateMap<RequestForCreationDto, Request>();
        }
    }
}