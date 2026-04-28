using InTicket.Application.Feauters.Matchs.Queries;
using InTicket.Application.Feauters.Matchs.Queries.Common;
using Moq;
using DomainMatch = InTicket.Domain.Match;

namespace InTicket.Tests.Features.Matches;

public class GetMatchByIdRequestHandlerTests
{
    private readonly MatchHandlerMocks _matchHandlerMocks;

    public GetMatchByIdRequestHandlerTests()
    {
        _matchHandlerMocks = new MatchHandlerMocks();
    }

    [Fact]
    public async Task Handle_WhenMatchNotFound_ReturnsNull()
    {
        // Arrange
        var request = MatchTestData.ValidGetMatchByIdRequest();

        _matchHandlerMocks.MatchQueryRepository
            .Setup(x => x.GetMatchByIdAsync(request.Id, request.IsRequestedByAdmin))
            .ReturnsAsync((DomainMatch)null!);

        // Act
        var result = await _matchHandlerMocks.GetMatchByIdHandler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Null(result);
        _matchHandlerMocks.Mapper.Verify(x => x.Map<GetMatchResponse>(It.IsAny<object>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenMatchIsInactive_ReturnsNull()
    {
        // Arrange
        var request = MatchTestData.ValidGetMatchByIdRequest();
        var inactiveMatch = MatchTestData.InactiveMatch();

        _matchHandlerMocks.MatchQueryRepository
            .Setup(x => x.GetMatchByIdAsync(request.Id, request.IsRequestedByAdmin))
            .ReturnsAsync(inactiveMatch);

        // Act
        var result = await _matchHandlerMocks.GetMatchByIdHandler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Null(result);
        _matchHandlerMocks.Mapper.Verify(x => x.Map<GetMatchResponse>(It.IsAny<object>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenMatchIsActive_ReturnsMappedResponseWithTeams()
    {
        // Arrange
        var request = MatchTestData.ValidGetMatchByIdRequest();
        var activeMatch = MatchTestData.ActiveMatch();
        var expectedResp = MatchTestData.MatchResponse();

        _matchHandlerMocks.MatchQueryRepository
            .Setup(x => x.GetMatchByIdAsync(request.Id, request.IsRequestedByAdmin))
            .ReturnsAsync(activeMatch);

        _matchHandlerMocks.Mapper
            .Setup(x => x.Map<GetMatchResponse>(activeMatch))
            .Returns(expectedResp);

        _matchHandlerMocks.Mapper
            .Setup(x => x.Map<TeamDto>(activeMatch.HomeTeam))
            .Returns(expectedResp.HomeTeam);

        _matchHandlerMocks.Mapper
            .Setup(x => x.Map<TeamDto>(activeMatch.AwayTeam))
            .Returns(expectedResp.AwayTeam);

        // Act
        var result = await _matchHandlerMocks.GetMatchByIdHandler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedResp.Id, result.Id);
        Assert.Equal(expectedResp.Title, result.Title);

        Assert.NotNull(result.HomeTeam);
        Assert.Equal(MatchTestData.HomeTeamId, result.HomeTeam.Id);
        Assert.Equal("FC Barcelona", result.HomeTeam.Name);

        Assert.NotNull(result.AwayTeam);
        Assert.Equal(MatchTestData.AwayTeamId, result.AwayTeam.Id);
        Assert.Equal("Real Madrid", result.AwayTeam.Name);
    }

    [Fact]
    public async Task Handle_WhenAdminRequest_PassesAdminFlagToRepository()
    {
        // Arrange
        var request = new GetMatchByIdRequest
        {
            Id = MatchTestData.MatchId,
            IsRequestedByAdmin = true
        };
        var activeMatch = MatchTestData.ActiveMatch();

        _matchHandlerMocks.MatchQueryRepository
            .Setup(x => x.GetMatchByIdAsync(request.Id, true))
            .ReturnsAsync(activeMatch);

        _matchHandlerMocks.Mapper
            .Setup(x => x.Map<GetMatchResponse>(activeMatch))
            .Returns(MatchTestData.MatchResponse());

        _matchHandlerMocks.Mapper
            .Setup(x => x.Map<TeamDto>(activeMatch.HomeTeam))
            .Returns(MatchTestData.MatchResponse().HomeTeam);

        _matchHandlerMocks.Mapper
            .Setup(x => x.Map<TeamDto>(activeMatch.AwayTeam))
            .Returns(MatchTestData.MatchResponse().AwayTeam);

        // Act
        await _matchHandlerMocks.GetMatchByIdHandler.Handle(request, CancellationToken.None);

        // Assert
        _matchHandlerMocks.MatchQueryRepository.Verify(
            x => x.GetMatchByIdAsync(MatchTestData.MatchId, true),
            Times.Once);
    }
}