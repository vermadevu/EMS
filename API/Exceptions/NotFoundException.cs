namespace API.Exceptions
{
    public class NotFoundException(string message) : ApiException(StatusCodes.Status404NotFound, message)
    {
    }
}
