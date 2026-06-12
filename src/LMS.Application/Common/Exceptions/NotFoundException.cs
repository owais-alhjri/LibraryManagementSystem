namespace LMS.Application.Common.Exceptions
{
    public class NotFoundException : AppException
    {
        public NotFoundException(string entity, object id) : base($"{entity} not found with id: {id}") { }

    }
}
