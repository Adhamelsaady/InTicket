using InTicket.Application.Feauters.Profile.Queries.GetMyPaymenrs;
using Moq;
using Xunit;

namespace InTicket.Tests.Features.Profile;

public class GetPaymentRequestHandlerTests
{
    private readonly ProfileHandlerMocks _profileHandlerMocks;

    public GetPaymentRequestHandlerTests()
    {
        _profileHandlerMocks = new ProfileHandlerMocks();
    }

    [Fact]
    public async Task Handle_WhenPaymentNotFound_ThrowsException()
    {
        // Arrange
        var request = ProfileTestData.ValidGetPaymentRequest(Guid.NewGuid());
        _profileHandlerMocks.PaymentBaseRepository.Setup(x => x.GetByIdAsync(request.PaymentId))
            .ReturnsAsync((InTicket.Domain.Payment)null!);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            _profileHandlerMocks.GetPaymentHandler.Handle(request, CancellationToken.None));

        Assert.Equal("Payment not found", exception.Message);
    }

    [Fact]
    public async Task Handle_WhenPaymentBelongsToDifferentUser_ThrowsException()
    {
        // Arrange
        var paymentId = Guid.NewGuid();
        var request = ProfileTestData.ValidGetPaymentRequest(paymentId);

        var payment = ProfileTestData.ValidPayment(paymentId);
        payment.UserId = "different-user-id";

        _profileHandlerMocks.PaymentBaseRepository.Setup(x => x.GetByIdAsync(request.PaymentId))
            .ReturnsAsync(payment);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            _profileHandlerMocks.GetPaymentHandler.Handle(request, CancellationToken.None));

        Assert.Equal("Payment not found", exception.Message);
    }

    [Fact]
    public async Task Handle_WhenValidRequest_ReturnsResponse()
    {
        // Arrange
        var paymentId = Guid.NewGuid();
        var request = ProfileTestData.ValidGetPaymentRequest(paymentId);
        var payment = ProfileTestData.ValidPayment(paymentId);
        var response = new GetPaymentResponse { Price = payment.Price, ExpirationDate = payment.ExpirationDate };

        _profileHandlerMocks.PaymentBaseRepository.Setup(x => x.GetByIdAsync(request.PaymentId))
            .ReturnsAsync(payment);

        _profileHandlerMocks.Mapper.Setup(x => x.Map<GetPaymentResponse>(payment))
            .Returns(response);

        // Act
        var result = await _profileHandlerMocks.GetPaymentHandler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(payment.Price, result.Price);
        Assert.Equal(payment.ExpirationDate, result.ExpirationDate);
    }
}
