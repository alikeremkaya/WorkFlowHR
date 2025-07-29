

namespace WorkFlowHR.Domain.Utilities.Interfaces;

public interface IResult
{
    public bool IsSuccess { get; }

    public string Messages { get; }
}
