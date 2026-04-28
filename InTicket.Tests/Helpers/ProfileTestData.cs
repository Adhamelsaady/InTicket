using InTicket.Application.Feauters.Profile.Commands.AddDelegate;
using InTicket.Application.Feauters.Profile.Commands.DeleteDelegation;
using InTicket.Application.Feauters.Profile.Queries.GetMyDelegation;
using InTicket.Application.Feauters.Profile.Queries.GetMyPaymenrs;
using InTicket.Application.Feauters.Profile.Queries.GetPayment;
using InTicket.Domain;

namespace InTicket.Tests.Features.Profile;

public static class ProfileTestData
{
    public const string DelegatorId = "delegator-123";
    public const string DelegateId = "delegate-456";
    public const string DelegateNationalId = "12345678901234";
    public const string PaymentId = "payment-789";

    public static AddDelegateRequest ValidAddDelegateRequest() => new()
    {
        DelegatorId = DelegatorId,
        DelegateNationalId = DelegateNationalId
    };

    public static DeleteDelegationRequest ValidDeleteDelegationRequest(Guid delegationId) => new()
    {
        DelegatorId = DelegatorId,
        DelegationId = delegationId
    };

    public static GetMyDelegationsRequest ValidGetMyDelegationsRequest() => new()
    {
        currentUserId = DelegatorId
    };

    public static GetPaymentsRequest ValidGetPaymentsRequest() => new()
    {
        UserId = DelegatorId,
        PaymentResourceParameters = new InTicket.Application.ResourceParameters.PaymentResourceParameters()
    };

    public static GetPaymentRequest ValidGetPaymentRequest(Guid paymentId) => new()
    {
        UserId = DelegatorId,
        PaymentId = paymentId
    };

    public static ApplicationUser DelegatorUser() => new()
    {
        Id = DelegatorId,
        NationalId = "98765432109876"
    };

    public static ApplicationUser DelegateUser() => new()
    {
        Id = DelegateId,
        NationalId = DelegateNationalId
    };

    public static Delegation ValidDelegation(Guid id) => new()
    {
        Id = id,
        DelegatorId = DelegatorId,
        DelegateId = DelegateId,
        DelegateNationalId = DelegateNationalId,
        CreatedAt = DateTime.UtcNow
    };

    public static Payment ValidPayment(Guid id) => new()
    {
        PaymentId = id,
        UserId = DelegatorId,
        Price = 150,
        ExpirationDate = DateTime.UtcNow.AddMinutes(15)
    };
}
