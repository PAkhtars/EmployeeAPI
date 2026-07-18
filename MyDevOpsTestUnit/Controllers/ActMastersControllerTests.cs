using System.Text;
using EmployeeAPI.Core.DTOs;
using EmployeeAPI.Core.Entities;
using EmployeeAPI.Interfaces;
using EmployeeAPI.WebApi.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace MyDevOpsTestUnit.Controllers;

public class ActMastersControllerTests
{
    [Fact]
    public async Task PostActMaster_WithUploadedImage_SavesImageAndReturnsCreated()
    {
        var repository = new FakeActMasterRepository();
        var env = new Mock<IWebHostEnvironment>();
        var webRootPath = Path.Combine(Path.GetTempPath(), "act-master-tests", Guid.NewGuid().ToString("N"));
        env.SetupGet(x => x.WebRootPath).Returns(webRootPath);
        env.SetupGet(x => x.ContentRootPath).Returns(Path.GetTempPath());

        var controller = new ActMastersController(repository, env.Object);
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes("fake-image"));
        var formFile = new FormFile(stream, 0, stream.Length, "actImage", "act.png")
        {
            Headers = new HeaderDictionary(),
            ContentType = "image/png"
        };

        var request = new MasterActCreateDto
        {
            ActName = "Consumer Protection",
            ActDetails = "Important consumer legislation",
            ActImage = formFile
        };

        var result = await controller.PostActMaster(request);

        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var createdActMaster = Assert.IsType<ActMaster>(createdResult.Value);
        Assert.Equal("Consumer Protection", createdActMaster.ActName);
        Assert.NotNull(createdActMaster.ImagePath);
        Assert.Contains("act.png", createdActMaster.ImagePath);
    }

    private sealed class FakeActMasterRepository : IActMasterRepository
    {
        private readonly List<ActMaster> _items = new();

        public Task<IEnumerable<ActMaster>> GetAllAsync() => Task.FromResult<IEnumerable<ActMaster>>(_items);

        public Task<ActMaster?> GetByIdAsync(int id) => Task.FromResult(_items.FirstOrDefault(x => x.ActId == id));

        public Task<ActMaster> AddAsync(ActMaster actMaster)
        {
            actMaster.ActId = _items.Count + 1;
            _items.Add(actMaster);
            return Task.FromResult(actMaster);
        }

        public Task UpdateAsync(ActMaster actMaster)
        {
            var index = _items.FindIndex(x => x.ActId == actMaster.ActId);
            if (index >= 0)
            {
                _items[index] = actMaster;
            }

            return Task.CompletedTask;
        }

        public Task DeleteAsync(int id)
        {
            _items.RemoveAll(x => x.ActId == id);
            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(int id) => Task.FromResult(_items.Any(x => x.ActId == id));
    }
}
