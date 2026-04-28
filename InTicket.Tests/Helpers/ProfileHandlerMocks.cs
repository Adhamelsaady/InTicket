using AutoMapper;
using InTicket.Application.Contracts.Presistance;
using InTicket.Application.Feauters.Profile.Commands.AddDelegate;
using InTicket.Application.Feauters.Profile.Commands.DeleteDelegation;
using InTicket.Application.Feauters.Profile.Queries.GetMyDelegation;
using InTicket.Application.Feauters.Profile.Queries.GetMyPaymenrs;
using InTicket.Application.Feauters.Profile.Queries.GetPayment;
using InTicket.Domain;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;

namespace InTicket.Tests.Features.Profile;

public class ProfileHandlerMocks
{
    public Mock<IDelegationsRepository> DelegationsRepository { get; }
    public Mock<IBaseRepository<ApplicationUser>> UserRepository { get; }
    public Mock<IBaseRepository<Delegation>> DelegationBaseRepository { get; }
    public Mock<IPaymentRepository> PaymentRepository { get; }
    public Mock<IBaseRepository<Payment>> PaymentBaseRepository { get; }
    public Mock<IMapper> Mapper { get; }
    public Mock<IDbContextTransaction> Transaction { get; }

    public AddDelegateRequestHandler AddDelegateHandler { get; }
    public DeleteDelegationRequestHandler DeleteDelegationHandler { get; }
    public GetMyDelegationsRequestHandler GetMyDelegationsHandler { get; }
    public GetPaymentsRequestHandler GetPaymentsHandler { get; }
    public GetPaymentRequestHandler GetPaymentHandler { get; }

    public ProfileHandlerMocks()
    {
        DelegationsRepository = new Mock<IDelegationsRepository>();
        UserRepository = new Mock<IBaseRepository<ApplicationUser>>();
        DelegationBaseRepository = new Mock<IBaseRepository<Delegation>>();
        PaymentRepository = new Mock<IPaymentRepository>();
        PaymentBaseRepository = new Mock<IBaseRepository<Payment>>();
        Mapper = new Mock<IMapper>();
        Transaction = new Mock<IDbContextTransaction>();

        UserRepository.Setup(x => x.BeginTransactionAsync()).ReturnsAsync(Transaction.Object);
        DelegationBaseRepository.Setup(x => x.BeginTransactionAsync()).ReturnsAsync(Transaction.Object);

        AddDelegateHandler = new AddDelegateRequestHandler(DelegationsRepository.Object, UserRepository.Object);
        DeleteDelegationHandler = new DeleteDelegationRequestHandler(DelegationBaseRepository.Object);
        GetMyDelegationsHandler = new GetMyDelegationsRequestHandler(DelegationsRepository.Object);
        GetPaymentsHandler = new GetPaymentsRequestHandler(PaymentRepository.Object, Mapper.Object);
        GetPaymentHandler = new GetPaymentRequestHandler(PaymentBaseRepository.Object, Mapper.Object);
    }
}
