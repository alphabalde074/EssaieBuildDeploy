using Fujitsu.CvQc.API.Domains;
using Fujitsu.CvQc.Business;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fujitsu.CvQc.API.Test.Domains
{
    public class JobControllerTest
    {
        private CvQcContextMock contextMock = new CvQcContextMock();
        public void Dispose() => contextMock.Dispose();

        public JobControllerTest() {
            new ConfigServiceMock().mockServiceProvider();
        }

        [Fact]
        public async Task GetJobs_All_returnAllJobs()
        {
            //Arrange
           JobController jobController = new JobController();
            String projectId = "test";
            //Act
            var data =  await jobController.GetJobs(projectId);

            //Assert
            //Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<JobModel>>>(data);
            var resultResp = Assert.IsType<List<JobModel>>(actionResult.Value);
            Assert.NotNull(resultResp);
        }

        [Fact]
        public async Task GetJob_JobById_ReturnJob()
        {
            //Arrange
            JobController jobController = new JobController();
            var idJob = "test";

            //Act
            var data = await jobController.GetJob(idJob);

            //Assert
            var actionResult = Assert.IsType<ActionResult<JobModel>>(data);
            Assert.IsType<JobModel>(actionResult.Value);
        }


        [Fact]  
        public async Task NewJob_addJob_returnString()
        {
            //Arrange
            JobController jobController = new JobController();
            String mode = "mode_Test";
            String projectId = "cbf11282-2671-42e2-a670-f71783771656";
            JobCreationModel jobCreation = new JobCreationModel(mode, projectId);

            //act
            var data = await jobController.NewJob(jobCreation);

            //Assert
            var actionResult = Assert.IsType<ActionResult<String>>(data);
            Assert.Equal(projectId, actionResult.Value);
        }


        [Fact]
        public async Task CompleteJob_Job_returnJobModel()
        {
            //Arrange
            JobController controller = new JobController();
            JobModel jobModel = new JobModel();

            //Act
            var data = await controller.CompleteJob(jobModel);
            //Assert
            var actionResult = Assert.IsType<ActionResult<JobModel>>(data);
            Assert.IsType<JobModel>(actionResult.Value);
        }

        [Fact]
        public async Task ResumeJob_Job_returnJobModel()
        {
            //Arrange
            JobController controller = new JobController();
            JobModel jobModel = new JobModel();

            //Act
            var data = await controller.StopJob(jobModel);
            //Assert
            var actionResult = Assert.IsType<ActionResult<JobModel>>(data);
            Assert.IsType<JobModel>(actionResult.Value);
        }

        [Fact]
        public async Task StopJob_Job_returnJobModel()
        {
            //Arrange
            JobController controller = new JobController();
            JobModel jobModel = new JobModel();

            //Act
            var data = await controller.StopJob(jobModel);
            //Assert
            var actionResult = Assert.IsType<ActionResult<JobModel>>(data);
            Assert.IsType<JobModel>(actionResult.Value);
        }

        [Fact]
        public async Task GetProjectLastJob_Job_returnJobModel()
        {
            //Arrange
            JobController controller = new JobController();
            String idproject = "cbf11282-2671-42e2-a670-f71783771656";

            //Act
            var data = await controller.GetProjectLastJob(idproject);
            //Assert
            var actionResult = Assert.IsType<ActionResult<JobModel>>(data);
            Assert.IsType<JobModel>(actionResult.Value);
        }


        [Fact]
        public async Task DeleteJob_Bool_returnTrue()
        {
            //Arrange
            JobController controller = new JobController();
            String jobid = "id_test";

            //Act
            var data = await controller.DeleteJob(jobid);
            //Assert
            Assert.True(data.Value);
        }
    }
}
