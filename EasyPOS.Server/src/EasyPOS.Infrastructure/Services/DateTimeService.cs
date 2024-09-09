using EasyPOS.Application.Common.Abstractions;

namespace EasyPOS.Infrastructure.Services;

internal class DateTimeService : IDateTimeProvider
{
    public DateTime Now => DateTime.Now;
}
