using EmployeeAPI.Core.Entities;
using EmployeeAPI.Infrastructure.Data;
using EmployeeAPI.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EmployeeAPIUnitTest;

public class ActSectionDtlsRepositoryTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly ActSectionDtlsRepository _repository;

    public ActSectionDtlsRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _repository = new ActSectionDtlsRepository(_context);
    }

    [Fact]
    public async Task AddAndGetById_ShouldPersistEntity()
    {
        var entity = new ActSectionDtls
        {
            ActId = 10,
            SectionNo = 5,
            ChapterName = "General Provisions",
            BareAct = "The Act",
            Meaning = "Meaning text",
            Objective = "Objective text",
            Illustration = "Illustration text",
            Exception = "Exception text",
            CaseStudyId = 20
        };

        var created = await _repository.AddAsync(entity);
        var fetched = await _repository.GetByIdAsync(created.SectionId);

        fetched.Should().NotBeNull();
        fetched!.ActId.Should().Be(10);
        fetched.SectionNo.Should().Be(5);
        fetched.ChapterName.Should().Be("General Provisions");
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
