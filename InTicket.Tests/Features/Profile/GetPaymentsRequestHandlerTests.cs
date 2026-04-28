using InTicket.Application.Feauters.Profile.Queries.GetMyPaymenrs;
using Moq;
using Xunit;

namespace InTicket.Tests.Features.Profile;

public class GetPaymentsRequestHandlerTests
{
    private readonly ProfileHandlerMocks _mocks;

    public GetPaymentsRequestHandlerTests()
    {
        _mocks = new ProfileHandlerMocks();
    }

    [Fact]
    public async Task Handle_WhenValidRequest_ReturnsPagedResult()
    {
        // Arrange
        var request = ProfileTestData.ValidGetPaymentsRequest();
        var paymentsList = new List<InTicket.Domain.Payment>
        {
            ProfileTestData.ValidPayment(Guid.NewGuid()),
            ProfileTestData.ValidPayment(Guid.NewGuid())
        };

        var pagedPayments = new InTicket.Application.Feauters.Authentication.Register.PagedResult<InTicket.Domain.Payment>
        {
            TotalCount = 2,
            PageNumber = 1,
            PageSize = 10,
            Items = paymentsList
        };

        var responseList = new List<GetPaymentResponse>
        {
            new() { Price = 150, ExpirationDate = paymentsList[0].ExpirationDate },
            new() { Price = 150, ExpirationDate = paymentsList[1].ExpirationDate }
        };

        _mocks.PaymentRepository.Setup(x => x.GetAllPaymentsOfUserAsync(request.PaymentResourceParameters, request.UserId))
            .ReturnsAsync(pagedPayments);

        _mocks.Mapper.Setup(x => x.Map<List<GetPaymentResponse>>(paymentsList))
            .Returns(responseList);

        // Act
        var result = await _mocks.GetPaymentsHandler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.TotalCount);
        Assert.Equal(1, result.PageNumber);
        Assert.Equal(10, result.PageSize);
        Assert.Equal(2, result.Items.Count);
    }
}
