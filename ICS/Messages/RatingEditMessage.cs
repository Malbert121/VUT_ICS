namespace ICS.Messages;

public record RatingEditMessage
{
    public required Guid RatingId { get; init; }
}