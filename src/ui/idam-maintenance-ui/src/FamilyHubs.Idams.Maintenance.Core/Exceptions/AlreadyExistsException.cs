using System.Runtime.Serialization;

namespace FamilyHubs.Idams.Maintenance.Core.Exceptions;

[Serializable]
public class AlreadyExistsException : IdamsException
{
    public AlreadyExistsException(string message):base(message)
    {
        Title = "Already Exists";
        HttpStatusCode = 400;
        ErrorCode = ExceptionCodes.AlreadyExistsException;
    }

    protected AlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
    {

    }
}
