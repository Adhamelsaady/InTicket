using InTicket.Application.Contracts.Presistance;
using InTicket.Domain;
using MediatR;

namespace InTicket.Application.Feauters.Profile.Commands.AddDelegate;

public class AddDelegateRequestHandler : IRequestHandler<AddDelegateRequest, bool>
{
    private readonly IDelegationsRepository _delegationsRepository;
    private readonly IBaseRepository<ApplicationUser> _userRepository;

    public AddDelegateRequestHandler(IDelegationsRepository delegationsRepository,
        IBaseRepository<ApplicationUser> userRepository)
    {
        _delegationsRepository = delegationsRepository;
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(AddDelegateRequest request, CancellationToken cancellationToken)
    {
        // if the user is already delegating someone -> return false
        // if the national id is wrong -> return false
        var delegator = await _userRepository.GetByIdAsync(request.DelegatorId);
        if (delegator == null)
        {
            return false;
        }

        if (await _delegationsRepository.HasDelegationAsync(request.DelegatorId))
        {
            return false;
        }

        var delegateUser = await _userRepository.GetByNationalIdAsync(request.DelegateNationalId);
        if (delegateUser == null)
        {
            return false;
        }

        if (delegateUser.Id == request.DelegatorId)
        {
            return false;
        }

        var resultedDelegate = new Delegation()
        {
            DelegatorId = request.DelegatorId,
            DelegateNationalId = request.DelegateNationalId,
            DelegateId = delegateUser.Id,
            CreatedAt = DateTime.UtcNow,
        };

        await _delegationsRepository.AddAsync(resultedDelegate);
        return true;
    }
}