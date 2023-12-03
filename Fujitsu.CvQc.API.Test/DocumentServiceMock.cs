using AutoMapper;
using Fujitsu.CvQc.Business;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fujitsu.CvQc.API.Test
{
    internal class DocumentServiceMock : IDocumentService
    {
        private readonly IMapper _mapper;
        private CvQcContextMock _context = new CvQcContextMock();

        public DocumentServiceMock(IMapper mapper)
        {
            _mapper = mapper;
        }
        CvQcContextMock CvQcContextMock = new CvQcContextMock();
        public DocumentModel? GetDocument(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            else return new DocumentModel();
        }

        public List<DocumentModel> GetDocuments(string projectId)
        {
            List<DocumentModel> documents = new List<DocumentModel>();  
            
            foreach (var document in _context.Context.Documents)
            {
                if(document.ProjectId == new Guid(projectId))
                {
                    DocumentModel documentModel= _mapper.Map<DocumentModel>(document);
                    documents.Add(documentModel);   
                }
            }
            return documents;
        }
    }
}
