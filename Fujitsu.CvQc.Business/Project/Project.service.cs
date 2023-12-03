using Fujitsu.CvQc.Data;
using Fujitsu.CvQc.Data.Entities;
using System.Diagnostics;

namespace Fujitsu.CvQc.Business;

public class ProjectService : IProjectService
{
    private readonly IProjectDataService projectDataService;

    public ProjectService(IProjectDataService projectDataService)
    {
        this.projectDataService = projectDataService;
    }

    public ServerResponse<IEnumerable<ProjectModel>> GetProjects()
    {
        var response = new ServerResponse<IEnumerable<ProjectModel>>();
        var models =  new List<ProjectModel>();

        var entities = projectDataService.GetProjects();
        if (entities.Count == 0)
        {
            response.AddValidationMessage(false, "No projects found.", "Click on new project to create the first one.", SeverityFlags.Warning);        
        }
        else
        {
            response.AddValidationMessage(false, "Available project(s) found", "Click on any project to continue working on, or click on new project to create a new one.", SeverityFlags.Info);        
        }

        foreach (var entity in entities)
        {
            var model = MapToProjectModel(entity);
            models.Add(model);
        }

        response.Result = models;
        return response;
    }	

    public ProjectModel? GetProject(string id)
    {
        ProjectModel? model = null;

        var guid = new Guid(id);
        var entity = projectDataService.GetProject(guid);
        if (entity != null)
        {
            model = MapToProjectModel(entity);
        }

        return model;
    }

    public ServerResponse<ProjectModel> AddProject(ProjectModel model)
    {    
        var response = new ServerResponse<ProjectModel>();  
        
        var entities = projectDataService.GetProjects();
        if (entities.Any((x) => x.Name.ToLower().Trim() == model.Name.ToLower().Trim())) 
        {
            response.AddValidationMessage("The project name is already used", string.Format("Cannot use '{0}' as a name for the project.  Please choose a diffrent one.", model.Name));        
        }
        else
        {
            var entity = new Project();
            entity.Name = model.Name;
            entity = projectDataService.AddProject(entity);
            model.Id = entity.Id.ToString();
            response.Result = model;
        }
        return response;
    }

    public ProjectModel UpdateProject(ProjectModel model)
    {
        var guid = new Guid(model.Id);
        var entity = projectDataService.GetProject(guid);

        if (entity != null)
        {
            entity.Name = model.Name;
            if (model.Config != null)
            {
                ProjectConfig config = new ProjectConfig
                {
                    ImportMode = model.Config.ImportMode,
                    InputPath = model.Config.InputPath,
                    OutputPath = model.Config.OutputPath,
                    OutputSuffix = model.Config.OutputSuffix,
                    TemplatePath = model.Config.TemplatePath,
                    Rules = model.Config.Rules
                };
                entity.Config = config;
            }
            if (model.Documents != null)
            {
                entity.Documents = MapToDocumentList(model.Documents);
            }
            entity = projectDataService.UpdateProject(entity);
            model.Id = entity.Id.ToString();
        }

        return model;
    }

    public bool DeleteProject(string id)
    {
        var result = false;

        var guid = new Guid(id);
        var entity = projectDataService.GetProject(guid);
        if (entity != null)
        {
            result = projectDataService.DeleteProject(entity);
        }

        return result;
    }

    #region Entities/Models mappers
    public Project MapToProject(ProjectModel model)
    {
        var entity = new Project();
        entity.Id = new Guid(model.Id);
        entity.Name = model.Name;
        if (model.Config != null)
        {
            ProjectConfig config = new ProjectConfig
            {
                ImportMode = model.Config.ImportMode,
                InputPath = model.Config.InputPath,
                OutputPath = model.Config.OutputPath,
                OutputSuffix = model.Config.OutputSuffix,
                TemplatePath = model.Config.TemplatePath,
                Rules = model.Config.Rules
            };
            entity.Config = config;
        }

        if (model.Documents != null)
        {
            ICollection<Document> documents = MapToDocumentList(model.Documents);
            entity.Documents = documents;
        }

        return entity;
    }

    public ProjectModel MapToProjectModel(Project entity)
    {
        var model = new ProjectModel();
        model.Id = entity.Id.ToString();
        model.Name = entity.Name;
        if (entity.Config != null)
        {
            ProjectConfigModel config = new ProjectConfigModel
            {
                ImportMode = entity.Config.ImportMode,
                InputPath = entity.Config.InputPath,
                OutputPath = entity.Config.OutputPath,
                OutputSuffix = entity.Config.OutputSuffix,
                TemplatePath = entity.Config.TemplatePath,
                Rules = entity.Config.Rules
            };
            model.Config = config;
        }

        if (entity.Documents.Any())
        {
            model.Documents = MapToDocumentModelList(entity.Documents);
        }

        return model;
    }

    public DocumentMapping MapToDocumentMapping(DocumentMap mapping)
    {
        DocumentMapping result = new DocumentMapping();
        result.Sections = MapToDocumentMapSectionList(mapping.Sections);
        return result;
    }

    public DocumentMap MapToDocumentMap(DocumentMapping mapping)
    {
        DocumentMap result = new DocumentMap();
        result.Sections = mapping.Sections.Select(MapToDocumentMapSection).ToList();
        return result;
    }

    public IEnumerable<DocumentMapSectionModel> MapToDocumentMapSectionList(IEnumerable<DocumentMapSection> sectionList)
    {
        List<DocumentMapSectionModel> result = new List<DocumentMapSectionModel>();
        foreach (var section in sectionList)
        {
            result.Add(MapToDocumentMapSection(section));
        }

        return result;
    }

    public DocumentMapSectionModel MapToDocumentMapSection(DocumentMapSection section)
    {
        DocumentMapSectionModel result = new DocumentMapSectionModel();
        result.Paragraphs = section.Paragraphs.ToList().ConvertAll(x => x.Text).ToList();
        result.Target = section.Target;
        result.Match = section.Match;
        result.ExportationMode = section.ExportationMode;
        return result;
    }
    
    public DocumentMapSection MapToDocumentMapSection(DocumentMapSectionModel section)
    {
        DocumentMapSection result = new DocumentMapSection();
        result.Paragraphs = section.Paragraphs.Select(text => new SectionParagraph() { Text = text }).ToList();
        result.Target = section.Target;
        result.Match = section.Match;
        result.ExportationMode = section.ExportationMode;
        return result;
    }

    public Document MapToDocument(DocumentModel document)
    {
        Document result = new Document();
        if (document.DocumentMap != null) {
            result.DocumentMap = MapToDocumentMap(document.DocumentMap);
        }        
        result.FileName = document.FileName;
        result.Importation = document.Importation;
        result.ProjectId = new Guid(document.ProjectId);
        return result;
    }

    public ICollection<Document> MapToDocumentList(ICollection<DocumentModel> documents) {
        List<Document> result = new List<Document>();
        foreach (var document in documents)
        {
            result.Add(MapToDocument(document));
        }
        return result;
    }

    public DocumentModel MapToDocumentModel(Document document)
    {
        DocumentModel result = new DocumentModel();
        if (document.DocumentMap != null)
        {
            result.DocumentMap = MapToDocumentMapping(document.DocumentMap);
        }
        result.FileName = document.FileName;
        result.Importation = document.Importation;
        result.ProjectId = document.ProjectId.ToString();
        return result;
    }

    public ICollection<DocumentModel> MapToDocumentModelList(ICollection<Document> documents)
    {
        ICollection<DocumentModel> result = new List<DocumentModel>();
        foreach (var document in documents)
        {
            result.Add(MapToDocumentModel(document));
        }
        return result;
    }
    #endregion
}