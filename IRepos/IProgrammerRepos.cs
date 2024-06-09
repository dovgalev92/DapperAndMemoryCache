using DapperMemoryCache.DTO;
using DapperMemoryCache.Models;

namespace DapperMemoryCache.IRepos
{
    public interface IProgrammerRepos
    {
        Task<Programmer> CreateProgrammer(CreateProgrammerDto create);
        Task<Programmer> GetByIdProgrammer(int programmerId);
        Task Delete(int idProgrammer);
    }
}
