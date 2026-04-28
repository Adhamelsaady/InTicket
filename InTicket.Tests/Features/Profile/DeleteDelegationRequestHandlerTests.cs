using Moq;
using Xunit;

namespace InTicket.Tests.Features.Profile;

public class DeleteDelegationRequestHandlerTests
{
    private readonly ProfileHandlerMocks _profileHandlerMocks;

    public DeleteDelegationRequestHandlerTests()
    {
        _profileHandlerMocks = new ProfileHandlerMocks();
    }

    [Fact]
    public async Task Handle_WhenDelegationNotBelongToDelegator_ReturnsFalse()
    {
        // Arrange
        var delegationId = Guid.NewGuid();
        var request = ProfileTestData.ValidDeleteDelegationRequest(delegationId);

        var delegation = ProfileTestData.ValidDelegation(delegationId);
        delegation.DelegatorId = "different-delegator-id"; 

        _profileHandlerMocks.DelegationBaseRepository.Setup(x => x.GetByIdAsync(request.DelegationId))
            .ReturnsAsync(delegation);

        // Act
        var result = await _profileHandlerMocks.DeleteDelegationHandler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result);
        _profileHandlerMocks.DelegationBaseRepository.Verify(x => x.DeleteAsync(It.IsAny<InTicket.Domain.Delegation>()), Times.Never);
        _profileHandlerMocks.Transaction.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenValidRequest_DeletesDelegationAndReturnsTrue()
    {
        // Arrange
        var delegationId = Guid.NewGuid();
        var request = ProfileTestData.ValidDeleteDelegationRequest(delegationId);
        var delegation = ProfileTestData.ValidDelegation(delegationId);

        _profileHandlerMocks.DelegationBaseRepository.Setup(x => x.GetByIdAsync(request.DelegationId))
            .ReturnsAsync(delegation);

        // Act
        var result = await _profileHandlerMocks.DeleteDelegationHandler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result);
        _profileHandlerMocks.DelegationBaseRepository.Verify(x => x.DeleteAsync(delegation), Times.Once);
        _profileHandlerMocks.DelegationBaseRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        _profileHandlerMocks.Transaction.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenExceptionThrown_RollsBackTransactionAndReturnsFalse()
    {
        // Arrange
        var delegationId = Guid.NewGuid();
        var request = ProfileTestData.ValidDeleteDelegationRequest(delegationId);

        _profileHandlerMocks.DelegationBaseRepository.Setup(x => x.GetByIdAsync(request.DelegationId))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _profileHandlerMocks.DeleteDelegationHandler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result);
       
        _profileHandlerMocks.Transaction.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
