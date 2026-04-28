using InTicket.Domain;
using Moq;
using DomainMatch = InTicket.Domain.Match;

namespace InTicket.Tests.Features.Matches;

public class CreateMatchRequestHandlerTests
{
    private readonly MatchHandlerMocks _matchHandlerMocks;

    public CreateMatchRequestHandlerTests()
    {
        _matchHandlerMocks = new MatchHandlerMocks();
    }

    [Fact]
    public async Task Handle_WhenMatchCreatedWithTickets_CommitsAndReturnsResponse()
    {
        // Arrange
        var request = MatchTestData.ValidCreateMatchRequest();
        var matchEntity = MatchTestData.InactiveMatch();

        _matchHandlerMocks.Mapper
            .Setup(x => x.Map<DomainMatch>(request))
            .Returns(matchEntity);

        _matchHandlerMocks.MatchRepository
            .Setup(x => x.AddAsync(matchEntity))
            .ReturnsAsync(matchEntity);

        _matchHandlerMocks.MatchRepository
            .Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        _matchHandlerMocks.TicketRepository
            .Setup(x => x.AddRangeAsync(It.IsAny<IEnumerable<MatchTicket>>()))
            .Returns(Task.CompletedTask);

        _matchHandlerMocks.TicketRepository
            .Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _matchHandlerMocks.CreateMatchHandler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(matchEntity.Id, result.Id);

        _matchHandlerMocks.TicketRepository.Verify(
            x => x.AddRangeAsync(It.Is<IEnumerable<MatchTicket>>(tickets => tickets.Count() == 60)),
            Times.Once);

        _matchHandlerMocks.Transaction.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenTicketsDistributionIsEmpty_SkipsTicketBatchAndCommits()
    {
        // Arrange
        var request = MatchTestData.ValidCreateMatchRequestNoTickets();
        var matchEntity = MatchTestData.InactiveMatch();

        _matchHandlerMocks.Mapper
            .Setup(x => x.Map<DomainMatch>(request))
            .Returns(matchEntity);

        _matchHandlerMocks.MatchRepository
            .Setup(x => x.AddAsync(matchEntity))
            .ReturnsAsync(matchEntity);

        _matchHandlerMocks.MatchRepository
            .Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        _matchHandlerMocks.TicketRepository
            .Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        // Act
        var result = await _matchHandlerMocks.CreateMatchHandler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(matchEntity.Id, result.Id);

        // No tickets should be batched when distribution is empty
        _matchHandlerMocks.TicketRepository.Verify(
            x => x.AddRangeAsync(It.IsAny<IEnumerable<MatchTicket>>()),
            Times.Never);

        _matchHandlerMocks.Transaction.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenMatchRepositoryThrows_RollsBackAndReturnsNull()
    {
        // Arrange
        var request = MatchTestData.ValidCreateMatchRequest();
        var matchEntity = MatchTestData.InactiveMatch();

        _matchHandlerMocks.Mapper
            .Setup(x => x.Map<DomainMatch>(request))
            .Returns(matchEntity);

        _matchHandlerMocks.MatchRepository
            .Setup(x => x.AddAsync(matchEntity))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _matchHandlerMocks.CreateMatchHandler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Null(result);
        _matchHandlerMocks.Transaction.Verify(x => x.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);
        _matchHandlerMocks.Transaction.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenTicketRepositoryThrows_RollsBackAndReturnsNull()
    {
        // Arrange
        var request = MatchTestData.ValidCreateMatchRequest();
        var matchEntity = MatchTestData.InactiveMatch();

        _matchHandlerMocks.Mapper
            .Setup(x => x.Map<DomainMatch>(request))
            .Returns(matchEntity);

        _matchHandlerMocks.MatchRepository
            .Setup(x => x.AddAsync(matchEntity))
            .ReturnsAsync(matchEntity);

        _matchHandlerMocks.MatchRepository
            .Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);

        _matchHandlerMocks.TicketRepository
            .Setup(x => x.AddRangeAsync(It.IsAny<IEnumerable<MatchTicket>>()))
            .ThrowsAsync(new Exception("Ticket batch failed"));

        // Act
        var result = await _matchHandlerMocks.CreateMatchHandler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Null(result);
        _matchHandlerMocks.Transaction.Verify(x => x.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);
        _matchHandlerMocks.Transaction.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}