using AutoMapper;
using InTicket.Application.Contracts.Presistance;
using InTicket.Application.Feauters.Matches.Commands.ActivateMatch;
using InTicket.Application.Feauters.Matches.Commands.CreateMatch;
using InTicket.Application.Feauters.Matches.Commands.DeleteMatch;
using InTicket.Application.Feauters.Matchs.Queries;
using InTicket.Application.Feauters.Matchs.Queries.GetAllMatches;
using InTicket.Domain;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using DomainMatch = InTicket.Domain.Match;

namespace InTicket.Tests.Features.Matches;

public class MatchHandlerMocks
{
    public Mock<IBaseRepository<DomainMatch>> MatchRepository { get; }
    public Mock<IBaseRepository<MatchTicket>> TicketRepository { get; }
    public Mock<IMatchRepository> MatchQueryRepository { get; }
    public Mock<IMapper> Mapper { get; }
    public Mock<IDbContextTransaction> Transaction { get; }

    public CreateMatchRequestHandler CreateMatchHandler { get; }
    public ActivateMatchRequestHandler ActivateMatchHandler { get; }
    public DeleteMatchRequestHandler DeleteMatchHandler { get; }
    public GetAllMatchesRequestHandler GetAllMatchesHandler { get; }
    public GetMatchByIdRequestHandler GetMatchByIdHandler { get; }

    public MatchHandlerMocks()
    {
        MatchRepository = new Mock<IBaseRepository<DomainMatch>>();
        TicketRepository = new Mock<IBaseRepository<MatchTicket>>();
        MatchQueryRepository = new Mock<IMatchRepository>();
        Mapper = new Mock<IMapper>();
        Transaction = new Mock<IDbContextTransaction>();

        Transaction.Setup(x => x.CommitAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        Transaction.Setup(x => x.RollbackAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        Transaction.Setup(x => x.DisposeAsync()).Returns(ValueTask.CompletedTask);

        MatchRepository.Setup(x => x.BeginTransactionAsync()).ReturnsAsync(Transaction.Object);
        TicketRepository.Setup(x => x.BeginTransactionAsync()).ReturnsAsync(Transaction.Object);

        CreateMatchHandler = new CreateMatchRequestHandler(
            MatchRepository.Object,
            TicketRepository.Object,
            Mapper.Object);

        ActivateMatchHandler = new ActivateMatchRequestHandler(MatchRepository.Object);
        DeleteMatchHandler   = new DeleteMatchRequestHandler(MatchRepository.Object);

        GetAllMatchesHandler = new GetAllMatchesRequestHandler(
            MatchQueryRepository.Object,
            Mapper.Object);

        GetMatchByIdHandler = new GetMatchByIdRequestHandler(
            MatchQueryRepository.Object,
            Mapper.Object);
    }
}