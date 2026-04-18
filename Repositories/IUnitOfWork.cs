using Lab06.Models;

namespace Lab06.Repositories;

public interface IUnitOfWork
{
    IArticleRepository ArticleRepository { get; }
        ICategoryRepository CategoryRepository { get; }
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
