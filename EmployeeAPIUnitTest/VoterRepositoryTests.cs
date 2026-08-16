using EmployeeAPI.Core.Entities;
using EmployeeAPI.Infrastructure.Data;
using EmployeeAPI.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EmployeeAPIUnitTest;

public class VoterRepositoryTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly VoterRepository _repository;

    public VoterRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _repository = new VoterRepository(_context);
    }

    [Fact]
    public async Task AddAndGetById_ShouldPersistAllMappedFields()
    {
        var voter = new Voter
        {
            SerialNo = 101,
            EpicNo = "EP-1001",
            Name = "Rahul Sharma",
            EnglishName = "Rahul Sharma",
            RelativeName = "Shankar",
            EnglishRelativeName = "Shankar",
            HouseNo = "A-12",
            Age = 35,
            Gender = "Male",
            PartNumber = "12A",
            AreaName = "Main Market",
            AreaNumber = "456"
        };

        var created = await _repository.AddAsync(voter);
        var fetched = await _repository.GetByIdAsync(created.Id);

        fetched.Should().NotBeNull();
        fetched!.SerialNo.Should().Be(101);
        fetched.EpicNo.Should().Be("EP-1001");
        fetched.Name.Should().Be("Rahul Sharma");
        fetched.HouseNo.Should().Be("A-12");
        fetched.AreaName.Should().Be("Main Market");
    }

    [Fact]
    public async Task AddRangeAsync_ShouldPersistMultipleVoters()
    {
        var voters = new List<Voter>
        {
            new()
            {
                SerialNo = 201,
                EpicNo = "EP-2001",
                Name = "Asha Singh",
                HouseNo = "B-14",
                AreaName = "City Center",
                AreaNumber = "789"
            },
            new()
            {
                SerialNo = 202,
                EpicNo = "EP-2002",
                Name = "Kiran Verma",
                HouseNo = "C-9",
                AreaName = "Old Town",
                AreaNumber = "790"
            }
        };

        var created = await _repository.AddRangeAsync(voters);

        created.Should().HaveCount(2);
        (await _context.Voters.CountAsync()).Should().Be(2);
        (await _context.Voters.Select(v => v.Name).ToListAsync()).Should().Contain(new[] { "Asha Singh", "Kiran Verma" });
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
