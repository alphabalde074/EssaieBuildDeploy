using AutoMapper;
using Fujitsu.CvQc.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fujitsu.CvQc.Business.Mapper
{
    public class DocumentMapperProfile : Profile
    {
        public DocumentMapperProfile() { 

            CreateMap<DocumentMap,DocumentMapping>();
            CreateMap<DocumentMapping,DocumentMap>();

            CreateMap<Document, DocumentModel>().ForMember(dest=>dest.DocumentMap,opt=>opt.MapFrom(src=>src.DocumentMap));
            CreateMap<DocumentModel, Document>();
        }
    }
}
