using Fujitsu.CvQc.API.Domains;
using Fujitsu.CvQc.Business;
using Fujitsu.CvQc.Data;
using Microsoft.AspNetCore.Mvc;

namespace Fujitsu.CvQc.API.Test.Domains
{
    public class ProjetControllerTests
    {
        private CvQcContextMock contextMock = new CvQcContextMock();

        public void Dispose() => contextMock.Dispose();

        public ProjetControllerTests()
        {
            new ConfigServiceMock().mockServiceProvider();

        }

        [Fact]
        public async Task GetProjects_Empty_returnAll()
        {
            //Arrange
            // ProjectDataService projectDataService = new ProjectDataService(contextMock.Context);
            // ProjectService projectService = new ProjectService(projectDataService);
            ProjectController projectController = new ProjectController();

            //Act
            var data = await projectController.GetProjects();

            //Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<ProjectModel>>>(data);
            var returnResult = Assert.IsType<List<ProjectModel>>(actionResult.Value);
            Assert.Equal(contextMock.Context.Projects.Count(), returnResult.Count);
        }


        [Fact]
        public async Task GetProject_byId_returnProject()
        {
            //Arrange
            ProjectController projectController = new ProjectController();
            String idTest = "1";

            //Act
            var data = await projectController.GetProject(idTest);

            //Assert
            var actionResult = Assert.IsType<ActionResult<ProjectModel>>(data);
            Assert.IsType<ProjectModel>(actionResult.Value);
        }

        [Fact]
        public async Task GetProject_byId_returnNull()
        {
            //Arrange
            ProjectController projectController = new ProjectController();
            String idTest = "";

            //Act
            var data = await projectController.GetProject(idTest);

            //Assert
            var actionResult = Assert.IsType<ActionResult<ProjectModel>>(data);
            Assert.Null(actionResult.Value);
        }

        [Fact]
        public async Task AddProject_projet_returnProjectAdded()
        {
            //Arrange
            ProjectController projectController = new ProjectController();
            ProjectModel projectModel = dataProjectModel();


            //Act
            var data = await projectController.AddProject(projectModel);

            //Assert
            var actionResult = Assert.IsType<ActionResult<ProjectModel>>(data);
            var returnResult = Assert.IsType<ProjectModel>(actionResult.Value);
            Assert.NotNull(returnResult);

        }


        [Fact]
        public async Task UpdateProject_project_returnUpdated()
        {
            //Arrange
            ProjectController projectController = new ProjectController();
            ProjectModel projectModel = dataProjectModel();

            //Act
            var data = await projectController.UpdateProject(projectModel);

            //Assert
            var actionResult = Assert.IsType<ActionResult<ProjectModel>>(data);
            var returnResult = Assert.IsType<ProjectModel>(actionResult.Value);
            Assert.NotNull(returnResult);
        }


        [Fact]
        public async Task DeleteProject_bool_returnTrue()
        {
            //Arrange
            ProjectController projectController = new ProjectController();
            String idTest = "1";
            //Act
            var data = await projectController.DeleteProject(idTest);

            //Assert
            var actionResult = Assert.IsType<ActionResult<bool>>(data);
            Assert.True(actionResult.Value);
        }

        [Fact]
        public void testEqual_shouldbe_equal()
        {
            Assert.Equal(3, 3);

        }

        public ProjectModel dataProjectModel()
        {
            ProjectModel projectModel = new ProjectModel();
            projectModel.Name = "NameProject";
            projectModel.Id = "1";

            return projectModel;
        }
    }
}
