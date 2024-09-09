namespace EasyPOS.Application.Common.Abstractions;

public interface IDateTimeProvider
{
    DateTime Now { get; }
}
