using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using MediatR;

namespace InTicket.Application.Feauters.Profile.Commands.AddDelegate;

public class AddDelegateRequest : IRequest <bool>
{
    [Required]
    public string DelegatorId { get; set; }
    [Required]
    public string DelegateNationalId { get; set; }
}