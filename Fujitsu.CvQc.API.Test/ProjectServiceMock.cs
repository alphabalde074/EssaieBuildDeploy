using Fujitsu.CvQc.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fujitsu.CvQc.API.Test
{
    internal class ProjectServiceMock : IProjectService
    {
        private CvQcContextMock _context = new CvQcContextMock();
        public ProjectModel AddProject(ProjectModel model)
        {
            return new ProjectModel();
        }

        public bool DeleteProject(string id)
        {
            return true;
        }

        public ProjectModel? GetProject(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }         
            else
            {
                return new ProjectModel();
            }            
        }

        public List<ProjectModel> GetProjects()
        {
            var projects = new List<ProjectModel>();
            foreach(var project in _context.Context.Projects)
            {
                projects.Add(new ProjectModel());

            }
            return projects;
        }

        public ProjectModel UpdateProject(ProjectModel model)
        {
           return new ProjectModel();
        }
        ServerResponse<ProjectModel> IProjectService.AddProject(ProjectModel model)
        {
            throw new NotImplementedException();
        }

        ServerResponse<IEnumerable<ProjectModel>> IProjectService.GetProjects()
        {
            throw new NotImplementedException();
        }
    }
}
