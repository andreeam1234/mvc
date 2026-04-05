using Lab06.Models;
using Lab06.Repositories;

namespace Lab06.Services;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoryService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<Category>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.CategoryRepository.GetAllAsync(cancellationToken);
    }
    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        var all = await _unitOfWork.CategoryRepository.GetAllAsync(cancellationToken);
        return all.Count;
    }
}
