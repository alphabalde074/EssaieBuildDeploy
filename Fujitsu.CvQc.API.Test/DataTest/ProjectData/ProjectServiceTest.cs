using Fujitsu.CvQc.Business;
using Fujitsu.CvQc.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fujitsu.CvQc.API.Test.DataTest.ProjectData
{
    public class ProjectServiceTest
    {
        private CvQcContextMock contextMock = new CvQcContextMock();
       
        private  ProjectDataService projectDataService;

        public ProjectServiceTest() 
        {
            this.projectDataService = new ProjectDataService(contextMock.Context);
        }
        public void Dispose() => contextMock.Dispose();

        [Fact]
        public void GetProjects_All_returnAll()
        {
            //Arrange
            ProjectService projectService = new ProjectService(projectDataService);

            //Act
            var data = projectService.GetProjects();

            //Assert
            Assert.IsType<List<ProjectModel>>(data);
            Assert.Equal(contextMock.Context.Projects.Count(), data.Count);
        }

        [Fact]
        public void GetProject_byId_returnProject()
        {
            //Arrange
            ProjectService projectService = new ProjectService(projectDataService);
            String projectId = "7c5c7188-e08f-4eef-81a6-0483b3ff88ab";
            String projectNameExpeted = "TestCV1";
            //Act
            var data =  projectService.GetProject(projectId);

            //Assert
            Assert.IsType<ProjectModel>(data);
            Assert.Equal(projectNameExpeted, data.Name);
        }

        [Fact]
        public void DeleteProject_byId_returnTrue()
        {
            //Arrange
            ProjectService projectService = new ProjectService(projectDataService);
            String projectId = "7c5c7188-e08f-4eef-81a6-0483b3ff88ab";
            //Act
            var data = projectService.DeleteProject(projectId);

            //Assert
            Assert.True(data);
        }

        [Fact]
        public void AddProject_newProject_returnProject()
        {
            //Arrange
            ProjectService projectService = new ProjectService(projectDataService);
            String projectNameExpeted = "NameProject";

            //Act
            var data = projectService.AddProject(dataProjectModel());

            //Assert
            Assert.IsType<ProjectModel>(data);
            Assert.Equal(projectNameExpeted, data.Name);
        }

        [Fact]
        public void UpdateProject_ExistigProject_returnProject()
        {
            //Arrange
            ProjectService projectService = new ProjectService(projectDataService);
            String projectNameExpeted = "NameProject";

            //Act
            var data = projectService.UpdateProject(dataProjectModel());

            //Assert
            Assert.IsType<ProjectModel>(data);
            Assert.Equal(projectNameExpeted, data.Name);
        }

        /**
          * new Project model to use in the test
          */
        public ProjectModel dataProjectModel()
        {
            ProjectModel projectModel = new ProjectModel();
            projectModel.Name = "NameProject";
            projectModel.Id = "98ae4067-f0f6-499c-9ca2-1788f2c7572f";

            return projectModel;
        }
    }
}
