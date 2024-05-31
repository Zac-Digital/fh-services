using System.Runtime.Serialization;

namespace FamilyHubs.Idam.Core.Exceptions;

[Serializable]
public class BadRequestException : IdamsException
{
    public BadRequestException(string message) : base(message)
    {
        Title = "Bad Request";
        HttpStatusCode = 400;
        ErrorCode = ExceptionCodes.BadRequest;
    }

    protected BadRequestException(SerializationInfo info, StreamingContext context) : base(info, context)
    {

    }
}
