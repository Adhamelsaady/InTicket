using InTicket.Application.Contracts.Presistance;
using MediatR;

namespace InTicket.Application.Feauters.Profile.Queries.GetMyDelegation;

public class GetMyDelegationsRequestHandler : IRequestHandler<GetMyDelegationsRequest , GetMyDelegationsResponse>
{
    private readonly IDelegationsRepository  _delegationsRepository;

    public GetMyDelegationsRequestHandler(IDelegationsRepository delegationsRepository)
    {
        _delegationsRepository = delegationsRepository;
    }

    public async Task<GetMyDelegationsResponse> Handle(GetMyDelegationsRequest request, CancellationToken cancellationToken)
    {
        var result = await _delegationsRepository.GetDelegationAsync(request.currentUserId);
        if (result == null)
            return null;
        var response = new GetMyDelegationsResponse() { NationalId = result.DelegateNationalId };
        return response;
    }
}