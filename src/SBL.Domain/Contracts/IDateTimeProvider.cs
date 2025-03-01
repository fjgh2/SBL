namespace SBL.Domain.Contracts;

public interface IDateTimeProvider
{
    DateTime Now { get; }
}
