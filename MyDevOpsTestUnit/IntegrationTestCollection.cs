
using Xunit;

namespace MyDevOpsTestUnit
{    
    [CollectionDefinition("IntegrationTests")]
    public class IntegrationTestCollection
        : ICollectionFixture<IntegrationTestFixture>
    {
    }
}