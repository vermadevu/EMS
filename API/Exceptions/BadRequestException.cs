namespace API.Exceptions
{
    public class BadRequestException(string message) : ApiException(StatusCodes.Status400BadRequest, message)
    {
    }
}
