using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using testApi.Models;
using testApi.Models.ViewModels;
using testApi.Repository;

namespace testApi.Automapper
{
    public class AutomapperProfile : Profile
    {
        private readonly ParentRepository parentRepository;

        public AutomapperProfile(ParentRepository parentRepository)
        {
            this.parentRepository = parentRepository;
        }

        public AutomapperProfile()
        {
            //CreateMap<Child, ChildView>().ForMember(dest => dest.Parent, opts => opts.MapFrom(src => parentRepository.GetIdByParent(src))).ReverseMap();
            CreateMap<ChildView, Child>().ForMember(dest => dest.Parent, 
                                                    opts => opts.MapFrom(src => parentRepository.GetParentById(src.Parent)))
                                         .ReverseMap();
            
            CreateMap<Parent, ParentView>().ReverseMap();
        }
    }
}
