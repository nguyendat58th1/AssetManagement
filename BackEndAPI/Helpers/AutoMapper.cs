using AutoMapper;
using BackEndAPI.Entities;
using BackEndAPI.Models;

namespace BackEndAPI.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {

            CreateMap<CreateUserModel, User>(); 
            CreateMap<User, UserDTO>();
            CreateMap<ReturnRequest, ReturnRequestDTO>()
                .ForMember(dto => dto.AssetCode, b => b.MapFrom(rr => rr.AssetCodeCopy))
                .ForMember(dto => dto.AssetName, b => b.MapFrom(rr => rr.AssetNameCopy))
                .ForMember(dto => dto.AssignedDate, b => b.MapFrom(rr => rr.AssignedDateCopy))
                .ForMember(dto => dto.RequestedByUser, b => b.MapFrom(rr => rr.RequestedByUser.UserName))
                .ForMember(dto => dto.AcceptedByUser, b => b.MapFrom(
                                                                rr => string.IsNullOrEmpty(rr.AcceptedByUser.UserName)
                                                                    ? ""
                                                                    : rr.AcceptedByUser.UserName
                                                                )
                        );



            CreateMap<CreateCategoryModel, AssetCategory>();
            CreateMap<CreateAssetModel, Asset>();
            CreateMap<AssignmentModel, Assignment>();
            CreateMap<Assignment, AssignmentDTO>();
            CreateMap<EditAssetModel, Asset>();
            CreateMap<Asset, AssetDTO>();

        }
    }
}