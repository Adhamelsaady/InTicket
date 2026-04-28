using Moq;
using Xunit;

namespace InTicket.Tests.Features.Profile;

public class GetMyDelegationsRequestHandlerTests
{
    private readonly ProfileHandlerMocks _profileHandlerMocks;

    public GetMyDelegationsRequestHandlerTests()
    {
        _profileHandlerMocks = new ProfileHandlerMocks();
    }

    [Fact]
    public async Task Handle_WhenDelegationDoesNotExist_ReturnsNull()
    {
        // Arrange
        var request = ProfileTestData.ValidGetMyDelegationsRequest();
        _profileHandlerMocks.DelegationsRepository.Setup(x => x.GetDelegationAsync(request.currentUserId))
            .ReturnsAsync((InTicket.Domain.Delegation)null!);

        // Act
        var result = await _profileHandlerMocks.GetMyDelegationsHandler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Handle_WhenDelegationExists_ReturnsResponse()
    {
        // Arrange
        var request = ProfileTestData.ValidGetMyDelegationsRequest();
        var delegation = ProfileTestData.ValidDelegation(Guid.NewGuid());

        _profileHandlerMocks.DelegationsRepository.Setup(x => x.GetDelegationAsync(request.currentUserId))
            .ReturnsAsync(delegation);

        // Act
        var result = await _profileHandlerMocks.GetMyDelegationsHandler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(delegation.DelegateNationalId, result.NationalId);
        Assert.Equal(delegation.CreatedAt, result.CreatedAt);
    }
}
