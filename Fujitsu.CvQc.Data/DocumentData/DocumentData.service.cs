using Fujitsu.CvQc.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fujitsu.CvQc.Data;
public class DocumentDataService : DataService<Document>, IDocumentDataService
{
    public DocumentDataService(DataContext dataContext) : base(dataContext)
    {

    }

    public List<Document> GetProjectDocuments(Guid projectId)
    {
        WaitAndLockThread();
        DbSet<Document> entities = dataContext.Set<Document>();
        Task<List<Document>> task;
        task = entities
            //.Include(d => d.DocumentMap) // Only if DocumentMap is needed
            .Where(d => d.ProjectId == projectId)
            .ToListAsync();
        Task.WaitAll(task);
        UnlockThread();

        return task.Result;
    }

    public Document? GetDocument(Guid id)
    {
        WaitAndLockThread();
        var entities = dataContext.Set<Document>();
        var task = entities.Include(d => d.DocumentMap).FirstOrDefaultAsync(d => d.Id == id);
        Task.WaitAll(task);
        var result = task.Result;
        UnlockThread();

        return result;
    }

    public Document AddDocument(Document document)
    {
        return Add(document);
    }

    public Document UpdateDocument(Document document)
    {
        return Update(document);
    }

    public bool DeleteDocument(Document document)
    {
        return Delete(document);
    }
}