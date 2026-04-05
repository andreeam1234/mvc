using Lab06.Models;
using Lab06.Repositories;

namespace Lab06.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        // Folosim UserRepository-ul deja expus de UnitOfWork în Partea 2 a laboratorului
        return await _unitOfWork.UserRepository.GetAllAsync(cancellationToken);
    }
}