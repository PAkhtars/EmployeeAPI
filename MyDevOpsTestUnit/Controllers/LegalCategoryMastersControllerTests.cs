using System.Text;
using EmployeeAPI.Core.Entities;
using EmployeeAPI.Interfaces;
using EmployeeAPI.WebApi.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace MyDevOpsTestUnit.Controllers;

public class LegalCategoryMastersControllerTests
{
    [Fact]
    public async Task Create_WithUploadedFile_SavesIconAndReturnsCreated()
    {
        var repository = new FakeLegalCategoryMasterRepository();
        var env = new Mock<IWebHostEnvironment>();
        var webRootPath = Path.Combine(Path.GetTempPath(), "legal-category-tests", Guid.NewGuid().ToString("N"));
        env.SetupGet(x => x.WebRootPath).Returns(webRootPath);
        env.SetupGet(x => x.ContentRootPath).Returns(Path.GetTempPath());

        var controller = new LegalCategoryMastersController(repository, env.Object);
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes("fake-image"));
        var formFile = new FormFile(stream, 0, stream.Length, "categoryIcon", "icon.png")
        {
            Headers = new HeaderDictionary(),
            ContentType = "image/png"
        };

        var request = new CreateLegalCategoryRequest
        {
            CategoryName = "Family Law",
            Alias = "Family",
            Description = "Family related legal matters",
            CategoryIcon = formFile
        };

        var result = await controller.Create(request);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var createdCategory = Assert.IsType<LegalCategoryMaster>(createdResult.Value);
        Assert.Equal("Family Law", createdCategory.CategoryName);
        Assert.NotNull(createdCategory.CategoryIconPath);
        Assert.Contains("icon.png", createdCategory.CategoryIconPath);
    }

    private sealed class FakeLegalCategoryMasterRepository : ILegalCategoryMasterRepository
    {
        private readonly List<LegalCategoryMaster> _items = new();

        public Task<IEnumerable<LegalCategoryMaster>> GetAllAsync() => Task.FromResult<IEnumerable<LegalCategoryMaster>>(_items);

        public Task<LegalCategoryMaster?> GetByIdAsync(int id) => Task.FromResult(_items.FirstOrDefault(x => x.Id == id));

        public Task<LegalCategoryMaster> AddAsync(LegalCategoryMaster category)
        {
            category.Id = _items.Count + 1;
            _items.Add(category);
            return Task.FromResult(category);
        }

        public Task UpdateAsync(LegalCategoryMaster category)
        {
            var index = _items.FindIndex(x => x.Id == category.Id);
            if (index >= 0)
            {
                _items[index] = category;
            }

            return Task.CompletedTask;
        }

        public Task DeleteAsync(int id)
        {
            _items.RemoveAll(x => x.Id == id);
            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(int id) => Task.FromResult(_items.Any(x => x.Id == id));
    }
}
