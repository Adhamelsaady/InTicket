using Moq;
using Xunit;

namespace InTicket.Tests.Features.Profile;

public class AddDelegateRequestHandlerTests
{
    private readonly ProfileHandlerMocks _profileHandlerMocks;

    public AddDelegateRequestHandlerTests()
    {
        _profileHandlerMocks = new ProfileHandlerMocks();
    }

    [Fact]
    public async Task Handle_WhenDelegatorNotFound_ReturnsFalse()
    {
        // Arrange
        var request = ProfileTestData.ValidAddDelegateRequest();
        _profileHandlerMocks.UserRepository.Setup(x => x.GetByIdAsync(request.DelegatorId))
            .ReturnsAsync((InTicket.Domain.ApplicationUser)null!);

        // Act
        var result = await _profileHandlerMocks.AddDelegateHandler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result);
        _profileHandlerMocks.Transaction.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenDelegatorAlreadyHasDelegation_ReturnsFalse()
    {
        // Arrange
        var request = ProfileTestData.ValidAddDelegateRequest();
        _profileHandlerMocks.UserRepository.Setup(x => x.GetByIdAsync(request.DelegatorId))
            .ReturnsAsync(ProfileTestData.DelegatorUser());
        _profileHandlerMocks.DelegationsRepository.Setup(x => x.HasDelegationAsync(request.DelegatorId))
            .ReturnsAsync(true);

        // Act
        var result = await _profileHandlerMocks.AddDelegateHandler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result);
        _profileHandlerMocks.Transaction.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenDelegateUserNotFound_ReturnsFalse()
    {
        // Arrange
        var request = ProfileTestData.ValidAddDelegateRequest();
        _profileHandlerMocks.UserRepository.Setup(x => x.GetByIdAsync(request.DelegatorId))
            .ReturnsAsync(ProfileTestData.DelegatorUser());
        _profileHandlerMocks.DelegationsRepository.Setup(x => x.HasDelegationAsync(request.DelegatorId))
            .ReturnsAsync(false);
        _profileHandlerMocks.UserRepository.Setup(x => x.GetByNationalIdAsync(request.DelegateNationalId))
            .ReturnsAsync((InTicket.Domain.ApplicationUser)null!);

        // Act
        var result = await _profileHandlerMocks.AddDelegateHandler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result);
        _profileHandlerMocks.Transaction.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenDelegateIsSameAsDelegator_ReturnsFalse()
    {
        // Arrange
        var request = ProfileTestData.ValidAddDelegateRequest();
        var delegator = ProfileTestData.DelegatorUser();

        _profileHandlerMocks.UserRepository.Setup(x => x.GetByIdAsync(request.DelegatorId))
            .ReturnsAsync(delegator);
        _profileHandlerMocks.DelegationsRepository.Setup(x => x.HasDelegationAsync(request.DelegatorId))
            .ReturnsAsync(false);
        _profileHandlerMocks.UserRepository.Setup(x => x.GetByNationalIdAsync(request.DelegateNationalId))
            .ReturnsAsync(delegator); // Return same user

        // Act
        var result = await _profileHandlerMocks.AddDelegateHandler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result);
        _profileHandlerMocks.Transaction.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenValidRequest_AddsDelegationAndReturnsTrue()
    {
        // Arrange
        var request = ProfileTestData.ValidAddDelegateRequest();
        _profileHandlerMocks.UserRepository.Setup(x => x.GetByIdAsync(request.DelegatorId))
            .ReturnsAsync(ProfileTestData.DelegatorUser());
        _profileHandlerMocks.DelegationsRepository.Setup(x => x.HasDelegationAsync(request.DelegatorId))
            .ReturnsAsync(false);
        _profileHandlerMocks.UserRepository.Setup(x => x.GetByNationalIdAsync(request.DelegateNationalId))
            .ReturnsAsync(ProfileTestData.DelegateUser());

        // Act
        var result = await _profileHandlerMocks.AddDelegateHandler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result);
        _profileHandlerMocks.DelegationsRepository.Verify(x => x.AddAsync(It.Is<InTicket.Domain.Delegation>(d =>
            d.DelegatorId == request.DelegatorId &&
            d.DelegateNationalId == request.DelegateNationalId &&
            d.DelegateId == ProfileTestData.DelegateId)), Times.Once);
        _profileHandlerMocks.Transaction.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenExceptionThrown_RollsBackTransactionAndReturnsFalse()
    {
        // Arrange
        var request = ProfileTestData.ValidAddDelegateRequest();
        _profileHandlerMocks.UserRepository.Setup(x => x.GetByIdAsync(request.DelegatorId))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _profileHandlerMocks.AddDelegateHandler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result);
        _profileHandlerMocks.Transaction.Verify(x => x.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
