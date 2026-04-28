using InTicket.Application.Feauters.Matchs.Queries.Common;
using InTicket.Application.Feauters.Matchs.Queries.GetAllMatches;
using InTicket.Application.ResourceParameters;
using Moq;
using DomainMatch = InTicket.Domain.Match;

namespace InTicket.Tests.Features.Matches;

public class GetAllMatchesRequestHandlerTests
{
    private readonly MatchHandlerMocks _matchHandlerMocks;

    public GetAllMatchesRequestHandlerTests()
    {
        _matchHandlerMocks = new MatchHandlerMocks();
    }

    [Fact]
    public async Task Handle_WhenMatchesExist_ReturnsMappedPagedResult()
    {
        // Arrange
        var request = MatchTestData.ValidGetAllMatchesRequest();
        var matches = new List<DomainMatch>
        {
            MatchTestData.ActiveMatch(Guid.NewGuid()),
            MatchTestData.ActiveMatch(Guid.NewGuid())
        };
        var pagedDomain = MatchTestData.PagedMatchResult(matches);
        var mappedResponses = matches.Select(m => MatchTestData.MatchResponse(m.Id)).ToList();

        _matchHandlerMocks.MatchQueryRepository
            .Setup(x => x.GetAllMatchesAsync(request.MatchResourceParameters, request.IsRequestedByAdmin))
            .ReturnsAsync(pagedDomain);

        _matchHandlerMocks.Mapper
            .Setup(x => x.Map<List<GetMatchResponse>>(pagedDomain.Items))
            .Returns(mappedResponses);

        // Act
        var result = await _matchHandlerMocks.GetAllMatchesHandler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(pagedDomain.TotalCount, result.TotalCount);
        Assert.Equal(pagedDomain.PageNumber, result.PageNumber);
        Assert.Equal(pagedDomain.PageSize, result.PageSize);
        Assert.Equal(mappedResponses.Count, result.Items.Count);
    }

    [Fact]
    public async Task Handle_WhenNoMatchesExist_ReturnsEmptyPagedResult()
    {
        // Arrange
        var request = MatchTestData.ValidGetAllMatchesRequest();
        var pagedEmpty = MatchTestData.PagedMatchResult(new List<DomainMatch>());

        _matchHandlerMocks.MatchQueryRepository
            .Setup(x => x.GetAllMatchesAsync(request.MatchResourceParameters, request.IsRequestedByAdmin))
            .ReturnsAsync(pagedEmpty);

        _matchHandlerMocks.Mapper
            .Setup(x => x.Map<List<GetMatchResponse>>(pagedEmpty.Items))
            .Returns(new List<GetMatchResponse>());

        // Act
        var result = await _matchHandlerMocks.GetAllMatchesHandler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.Items);
        Assert.Equal(0, result.TotalCount);
    }

    [Fact]
    public async Task Handle_WhenAdminRequest_PassesAdminFlagToRepository()
    {
        // Arrange
        var request = MatchTestData.ValidGetAllMatchesRequest(isAdmin: true);
        var pagedResult = MatchTestData.PagedMatchResult(new List<DomainMatch>());

        _matchHandlerMocks.MatchQueryRepository
            .Setup(x => x.GetAllMatchesAsync(request.MatchResourceParameters, true))
            .ReturnsAsync(pagedResult);

        _matchHandlerMocks.Mapper
            .Setup(x => x.Map<List<GetMatchResponse>>(pagedResult.Items))
            .Returns(new List<GetMatchResponse>());

        // Act
        await _matchHandlerMocks.GetAllMatchesHandler.Handle(request, CancellationToken.None);

        // Assert: admin flag must be forwarded verbatim
        _matchHandlerMocks.MatchQueryRepository.Verify(
            x => x.GetAllMatchesAsync(request.MatchResourceParameters, true),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenFiltersApplied_ForwardsResourceParametersToRepository()
    {
        // Arrange
        var parameters = new MatchResourceParameters
        {
            League = "La Liga",
            HomeTeamId = MatchTestData.HomeTeamId
        };
        var request = new GetAllMatchesRequest
        {
            MatchResourceParameters = parameters,
            IsRequestedByAdmin = false
        };
        var pagedResult = MatchTestData.PagedMatchResult(new List<DomainMatch>());

        _matchHandlerMocks.MatchQueryRepository
            .Setup(x => x.GetAllMatchesAsync(parameters, false))
            .ReturnsAsync(pagedResult);

        _matchHandlerMocks.Mapper
            .Setup(x => x.Map<List<GetMatchResponse>>(pagedResult.Items))
            .Returns(new List<GetMatchResponse>());

        // Act
        await _matchHandlerMocks.GetAllMatchesHandler.Handle(request, CancellationToken.None);

        // Assert
        _matchHandlerMocks.MatchQueryRepository.Verify(
            x => x.GetAllMatchesAsync(
                It.Is<MatchResourceParameters>(p =>
                    p.League == "La Liga" &&
                    p.HomeTeamId == MatchTestData.HomeTeamId),
                false),
            Times.Once);
    }
}