using InTicket.Application.Feauters.Matches.Commands.DeleteMatch;
using Moq;
using DomainMatch = InTicket.Domain.Match;

namespace InTicket.Tests.Features.Matches;

public class DeleteMatchRequestHandlerTests
{
    private readonly MatchHandlerMocks _matchHandlerMocks;

    public DeleteMatchRequestHandlerTests()
    {
        _matchHandlerMocks = new MatchHandlerMocks();
    }

    [Fact]
    public async Task Handle_WhenMatchNotFound_ReturnsFalse()
    {
        // Arrange
        var request = new DeleteMatchRequest { Id = MatchTestData.MatchId };

        _matchHandlerMocks.MatchRepository
            .Setup(x => x.GetByIdAsync(request.Id))
            .ReturnsAsync((DomainMatch)null!);

        // Act
        var result = await _matchHandlerMocks.DeleteMatchHandler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result);
        _matchHandlerMocks.MatchRepository.Verify(x => x.DeleteAsync(It.IsAny<DomainMatch>()), Times.Never);
        _matchHandlerMocks.Transaction.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenMatchExists_DeletesAndCommitsAndReturnsTrue()
    {
        // Arrange
        var match   = MatchTestData.ActiveMatch();
        var request = new DeleteMatchRequest { Id = match.Id };

        _matchHandlerMocks.MatchRepository
            .Setup(x => x.GetByIdAsync(request.Id))
            .ReturnsAsync(match);

        _matchHandlerMocks.MatchRepository
            .Setup(x => x.DeleteAsync(match))
            .Returns(Task.CompletedTask);

        _matchHandlerMocks.MatchRepository
            .Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _matchHandlerMocks.DeleteMatchHandler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result);
        _matchHandlerMocks.MatchRepository.Verify(x => x.DeleteAsync(match),  Times.Once);
        _matchHandlerMocks.MatchRepository.Verify(x => x.SaveChangesAsync(),  Times.Once);
        _matchHandlerMocks.Transaction.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenExceptionThrown_RollsBackAndReturnsFalse()
    {
        // Arrange
        var request = new DeleteMatchRequest { Id = MatchTestData.MatchId };

        _matchHandlerMocks.MatchRepository
            .Setup(x => x.GetByIdAsync(request.Id))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _matchHandlerMocks.DeleteMatchHandler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result);
        _matchHandlerMocks.Transaction.Verify(x => x.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);
        _matchHandlerMocks.Transaction.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}