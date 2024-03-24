using Application.Common.Interfaces;
using BambaBackEnd.Application.Common.Mailing;

namespace Infrastructure.Common.Mailing;

public interface IMailService : ITransientService
{
    Task SendAsync(MailRequest request, CancellationToken ct);
}