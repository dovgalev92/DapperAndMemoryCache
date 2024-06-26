﻿using Dapper;
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
        private readonly ILogger<ProgrammerRepos> _logger;
        public ProgrammerRepos(DapperContext context, IMemoryCache memoryCache, ILogger<ProgrammerRepos> logger)
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
        public async Task Delete(int idProgrammer)
        {
            var query = "Delete From Programmer where Id = @idProgrammer";
            _memoryCache.TryGetValue(idProgrammer, out Programmer? programmer);
            if(programmer != null)
            {
                _memoryCache.Remove(idProgrammer);
                var stat = _memoryCache.GetCurrentStatistics();
                if(stat != null)
                {
                    _logger.LogInformation($"Количество элементов в кэше {stat.CurrentEntryCount}");
                    _logger.LogInformation($"Размер элементов в кэше {stat.CurrentEstimatedSize}");
                }
                _logger.LogInformation($"{idProgrammer} удален из кэша");
            }
            using (var connect = _context.CreateConnection())
            {
                var deleteProgrammer = await connect.ExecuteAsync(query, new { idProgrammer });
                _logger.LogInformation($"Данные удалены с бд");
            } 
        }
        public async Task<Programmer> GetByIdProgrammer(int programmerId)
        {
            _logger.LogInformation($"Выполняется поиск программиста с ID {programmerId} в кэшe");
            _memoryCache.TryGetValue(programmerId, out Programmer? programmer);
            if (programmer != null)
            {
                Console.WriteLine($"{programmer.Name} получен из кэша");
                return programmer;
            }
            else
            {
                _logger.LogInformation($"Выполняется запрос в бд для поиска программиста по ID {programmerId}");
                var query = "Select * FROM Programmer where Id = @programmerId";
                using (var connect = _context.CreateConnection())
                {
                    var getIdprogrammer = await connect.QuerySingleOrDefaultAsync<Programmer>(query, new { programmerId });
                    _memoryCache.Set(programmerId, getIdprogrammer, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(5)).SetSize(3));
                    var stat = _memoryCache.GetCurrentStatistics();
                    if(stat != null)
                    {
                        _logger.LogInformation($"Количество элементов в кеше {stat.CurrentEntryCount}");
                        _logger.LogInformation($"Размер элементов в кеше {stat.CurrentEstimatedSize}");
                    }
                    var result = getIdprogrammer is null ? throw new ArgumentNullException() : getIdprogrammer;
                    return result;
                }
            }
        }
    }
}
