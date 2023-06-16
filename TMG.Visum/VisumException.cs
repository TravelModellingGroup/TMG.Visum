namespace TMG.Visum;

/// <summary>
/// An exception that was generated from Visum
/// </summary>
public sealed class VisumException : Exception
{
    public VisumException() : base("Unknown excpetion")
    {
            
    }

    public VisumException(string message) : base(message)
    {

    }

    public VisumException(Exception innerException) : base(innerException.Message, innerException)
    {

    }
}
