namespace ICS.Messages;
using System;

public record RatingEditMessage
{
    public required Guid RatingId { get; init; }
}