using AutoMapper;
using Fujitsu.CvQc.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fujitsu.CvQc.Business.Mapper
{
    public class ProjectMapperProfile : Profile
    {
        public ProjectMapperProfile()
        {
            CreateMap<Project, ProjectModel>();
            CreateMap<ProjectModel, Project>();
            
        }
    }
}
