namespace ICS.Messages;

public record RatingStudentSelectMessage
{
    public required Guid StudentId { get; init; }
}
