using InTicket.Application.Feauters.Matchs.Queries.Common;
using MediatR;

namespace InTicket.Application.Feauters.Matchs.Queries;
public class GetMatchByIdRequest : IRequest <GetMatchResponse>
{
    public Guid Id { get; set; }
    public bool IsRequestedByAdmin { get; set; }
}