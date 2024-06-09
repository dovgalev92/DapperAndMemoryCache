using Dapper;
using DapperMemoryCache.DbContext;
using DapperMemoryCache.DTO;
using DapperMemoryCache.IRepos;
using DapperMemoryCache.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Data;

namespace DapperMemoryCache.ImplementationRepos
{
    public class ProgrammerRepos : IProgrammerRepos
    {
        private readonly DapperContext _context;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger _logger;
        public ProgrammerRepos(DapperContext context, IMemoryCache memoryCache, ILogger logger)
        {
            _context = context;
            _memoryCache = memoryCache;
            _logger = logger;
        }

        public async Task<Programmer> CreateProgrammer(CreateProgrammerDto create)
        {
            _logger.LogInformation("Выполняется запрос к базе данных");
            var query = "INSERT INTO Programmer(Name, Level, Salary) VALUES(@Name, @Level, @Salary)" +
                "Select CAST (SCOPE_IDENTITY() as int)";

            var parametrsAdd = new DynamicParameters();
            parametrsAdd.Add("Name", create.Name, DbType.String);
            parametrsAdd.Add("Level", create.Level, DbType.String);
            parametrsAdd.Add("Salary", create.Salary, DbType.Decimal);

            using (var connection = _context.CreateConnection())
            {
                var id = await connection.QuerySingleAsync<int>(query, parametrsAdd);

                Programmer programmer = new Programmer
                {
                    Id = id,
                    Name = create.Name,
                    Level = create.Level,
                    Salary = create.Salary
                };
                _logger.LogInformation("Операция выполнена успешно");
                return programmer;
            }
        }

        public Task Delete(int idProgrammer)
        {
            throw new NotImplementedException();
        }

        public Task<Programmer> GetByIdProgrammer(int programmerId)
        {
            throw new NotImplementedException();
        }
    }
}
