using System.Runtime.Serialization;

namespace FamilyHubs.Idam.Core.Exceptions;

[Serializable]
public class NotFoundException : IdamsException
{
    public NotFoundException(string message) : base(message)
    {
        Title = "Not Found";
        HttpStatusCode = 404;
        ErrorCode = ExceptionCodes.NotFoundException;
    }

    protected NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {

    }
}