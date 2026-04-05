using Lab06.Models;

namespace Lab06.Repositories;

public interface IUnitOfWork
{
    IArticleRepository ArticleRepository { get; }
    // Modifică linia veche cu asta:
    ICategoryRepository CategoryRepository { get; }
    IRepository<User> UserRepository { get; }
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
