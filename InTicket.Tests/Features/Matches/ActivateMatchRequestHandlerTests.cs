using InTicket.Application.Feauters.Matches.Commands.ActivateMatch;
using Moq;

namespace InTicket.Tests.Features.Matches;

public class ActivateMatchRequestHandlerTests
{
    private readonly MatchHandlerMocks _matchHandlerMocks;

    public ActivateMatchRequestHandlerTests()
    {
        _matchHandlerMocks = new MatchHandlerMocks();
    }

    [Fact]
    public async Task Handle_WhenMatchNotFound_ReturnsFalse()
    {
        // Arrange
        var request = new ActivateMatchRequest { Id = MatchTestData.MatchId };

        _matchHandlerMocks.MatchRepository
            .Setup(x => x.GetByIdAsync(request.Id))
            .ReturnsAsync((InTicket.Domain.Match)null!);

        // Act
        var result = await _matchHandlerMocks.ActivateMatchHandler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result);
        _matchHandlerMocks.Transaction.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenMatchIsInactive_ActivatesMatchSetsTimestampsAndReturnsTrue()
    {
        // Arrange
        var request = new ActivateMatchRequest { Id = MatchTestData.MatchId };
        var match = MatchTestData.InactiveMatch();

        _matchHandlerMocks.MatchRepository
            .Setup(x => x.GetByIdAsync(request.Id))
            .ReturnsAsync(match);

        _matchHandlerMocks.MatchRepository
            .Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _matchHandlerMocks.ActivateMatchHandler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result);
        Assert.True(match.IsActive);
        Assert.NotNull(match.FanPriorityBookingStart);
        Assert.NotNull(match.GeneralBookingStart);
        _matchHandlerMocks.Transaction.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenMatchIsAlreadyActive_SkipsActivationAndReturnsTrue()
    {
        // Arrange
        var request = new ActivateMatchRequest { Id = MatchTestData.MatchId };
        var match = MatchTestData.ActiveMatch();
        var originalFanPriorityStart = match.FanPriorityBookingStart;
        var originalGeneralStart = match.GeneralBookingStart;

        _matchHandlerMocks.MatchRepository
            .Setup(x => x.GetByIdAsync(request.Id))
            .ReturnsAsync(match);

        _matchHandlerMocks.MatchRepository
            .Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _matchHandlerMocks.ActivateMatchHandler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result);
        Assert.Equal(originalFanPriorityStart, match.FanPriorityBookingStart);
        Assert.Equal(originalGeneralStart, match.GeneralBookingStart);
        _matchHandlerMocks.Transaction.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenGeneralBookingStartIsSetRelativeToEventDate()
    {
        // Arrange
        var request = new ActivateMatchRequest { Id = MatchTestData.MatchId };
        var match = MatchTestData.InactiveMatch();
        var beforeHandle = DateTime.Now;

        _matchHandlerMocks.MatchRepository
            .Setup(x => x.GetByIdAsync(request.Id))
            .ReturnsAsync(match);

        _matchHandlerMocks.MatchRepository
            .Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        await _matchHandlerMocks.ActivateMatchHandler.Handle(request, CancellationToken.None);

        // Assert
        var expectedGeneralStart = match.EventDate.AddHours(-15);
        Assert.Equal(expectedGeneralStart, match.GeneralBookingStart);
        
        Assert.True(match.FanPriorityBookingStart >= beforeHandle.AddMinutes(1).AddSeconds(-1));
    }

    [Fact]
    public async Task Handle_WhenExceptionThrown_RollsBackAndReturnsFalse()
    {
        // Arrange
        var request = new ActivateMatchRequest { Id = MatchTestData.MatchId };

        _matchHandlerMocks.MatchRepository
            .Setup(x => x.GetByIdAsync(request.Id))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _matchHandlerMocks.ActivateMatchHandler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result);
        _matchHandlerMocks.Transaction.Verify(x => x.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);
        _matchHandlerMocks.Transaction.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}