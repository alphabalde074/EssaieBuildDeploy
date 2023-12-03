using Fujitsu.CvQc.Business;
using Fujitsu.CvQc.Data;
using Fujitsu.CvQc.Data.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fujitsu.CvQc.API.Test
{
    internal class CvQcContextMock
    {
        private DbConnection _connection;
        private DbContextOptions<DataContext> contextOptions;
        public DataContext Context { get; }

        public CvQcContextMock()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();
             
            contextOptions = new DbContextOptionsBuilder<DataContext>().
                UseSqlite(_connection).Options;

            Context = new DataContext(contextOptions);
            Context.Database.EnsureDeleted();
            Context.Database.EnsureCreated();
        
            //Ajout des données
            ProjectConfig projectConfig = new ProjectConfig();
            projectConfig.OutputPath = "outputh";
            projectConfig.InputPath = "input path";
            projectConfig.TemplatePath = "templatePath";
            projectConfig.Rules = "Rules Json";

            Project project1 = new Project();
            project1.Id = new Guid("7c5c7188-e08f-4eef-81a6-0483b3ff88ab");
            project1.Name = "TestCV1";
            project1.Config = projectConfig;

            Project project2 = new Project();
            project2.Id = new Guid("bb88cc3b-67c8-4bd1-bd42-221e6212d9d8");
            project2.Name = "TestCV1";
            project2.Config = projectConfig;

            Project project3 = new Project();
            project3.Id = new Guid("cbf11282-2671-42e2-a670-f71783771656");
            project3.Name = "TestCV3";
            project3.Config = projectConfig;
             
            Project project4 = new Project();
            project4.Id = new Guid("6a58a71b-1d30-4aa8-8658-4a63ee88848f");
            project4.Name = "TestCV4";
            project4.Config = projectConfig;
            

            Context.Projects.Add(project1);
            Context.Projects.Add(project2);
            Context.Projects.Add(project3);
            Context.Projects.Add(project4);

           
            Document document = new Document();
            document.Id = new Guid("d51e843a-c16b-454e-9f5c-eee3762bca0a");
            document.FileName = "documentName3 Test";
            document.ProjectId = new Guid("6a58a71b-1d30-4aa8-8658-4a63ee88848f");
            document.DocumentMap = new DocumentMap();              

            Document document2 = new Document();
            document2.Id = new Guid("dc8325c1-943a-4599-b3b1-795c0808f0b5");
            document2.FileName = "documentName3 Test";
            document2.ProjectId = new Guid("cbf11282-2671-42e2-a670-f71783771656");
            document2.DocumentMap = new DocumentMap();

            Document document3 = new Document();
            document3.Id = new Guid("db5e6370-e89d-4890-a2c1-141adbf1ef52");
            document3.FileName = "documentName3 Test";
            document3.ProjectId = new Guid("cbf11282-2671-42e2-a670-f71783771656");
            document3.DocumentMap = new DocumentMap();


            Context.Documents.Add(document2);
            Context.Documents.Add(document3);
            Context.Documents.Add(document);
            
            Context.SaveChanges();


        }
 
        public void Dispose() =>_connection.Dispose();
    }
}
