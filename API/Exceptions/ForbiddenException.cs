namespace API.Exceptions
{
    public class ForbiddenException(string message) : ApiException(StatusCodes.Status403Forbidden, message)
    {
    }
}
