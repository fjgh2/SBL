using SBL.Domain.Contracts;

namespace SBL.Domain.Common;

public class CurrentDateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.Now;
}
