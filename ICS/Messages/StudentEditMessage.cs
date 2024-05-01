
namespace ICS.Messages
{
    public record StudentEditMessage
    {
         public required Guid StudentId { get; init; }
    }
}
