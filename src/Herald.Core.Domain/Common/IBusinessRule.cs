namespace Herald.Core.Domain.Common;

public interface IBusinessRule
{
    public bool IsBroken();
    
    public string Message { get; }
}