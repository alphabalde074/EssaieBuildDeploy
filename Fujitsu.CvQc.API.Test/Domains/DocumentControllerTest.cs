using Fujitsu.CvQc.API.Domains;
using Fujitsu.CvQc.Business;
using Microsoft.AspNetCore.Mvc;

namespace Fujitsu.CvQc.API.Test.Domains
{
    public class DocumentControllerTest
    {
        private CvQcContextMock contextMock = new CvQcContextMock();
        public void Dispose() => contextMock.Dispose();

        public DocumentControllerTest()
        {
            new ConfigServiceMock().mockServiceProvider();

        }

        [Fact]
        public async Task GetDocuments_ByProjectId_ReturnDocuments()
        {
            //Arrange
            DocumentController documentController = new DocumentController();
            String projectId = "cbf11282-2671-42e2-a670-f71783771656";
            int expectedCount = 2;
            //Act
            var data = await documentController.GetDocuments(projectId);

            //Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<DocumentModel>>>(data);
            var resultResp = Assert.IsType<List<DocumentModel>>(actionResult.Value);
            Assert.Equal(expectedCount, resultResp.Count);

        }

        [Fact]
        public async Task GetDocument_ById_ReturnDocument()
        {
            //Arrange
            DocumentController documentController = new DocumentController();
            String idDoc = "d51e843a-c16b-454e-9f5c-eee3762bca0a";

            //Act
            var data = await documentController.GetDocument(idDoc);
            
            //Assert
            var actionResult = Assert.IsType<ActionResult<DocumentModel>>(data);
            Assert.IsType<DocumentModel>(actionResult.Value);
        }

        [Fact]
        public async Task GetDocument_ById_ReturnNull()
        {
            //Arrange
            DocumentController documentController = new DocumentController();
            String idDoc = "";

            //Act
            var data = await documentController.GetDocument(idDoc);
            
            //Assert
            var actionResult = Assert.IsType<ActionResult<DocumentModel>>(data);
            Assert.Null(actionResult.Value);
        }

    }
}
