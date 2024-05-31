namespace FamilyHubs.Report.Core.Exceptions;

public class ReportingException : Exception
{
    public string Title { get; set; } = "Server Error";
    public string ErrorCode { get; set; } = ExceptionCodes.UnhandledException;
    public int HttpStatusCode { get; set; } = 500;

    public ReportingException(string message) : base(message)
    {
    }
}

