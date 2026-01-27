namespace HotelReservationSystem.Core.Domain.Entities;

public sealed partial class Opinion
{
    private static void ValidateInput(double rating, string comment)
    {
        if (rating < 1 || rating > 5)
            throw new ArgumentException("Rating must be between 1 and 5", nameof(rating));
        
        if (string.IsNullOrWhiteSpace(comment))
            throw new ArgumentException("Comment cannot be empty", nameof(comment));
        
        if (comment.Length > 500)
            throw new ArgumentException("Comment cannot exceed 500 characters", nameof(comment));
    }

    public void UpdateRating(double newRating)
    {
        if (newRating < 1 || newRating > 5)
            throw new ArgumentException("Rating must be between 1 and 5", nameof(newRating));

        Rating = newRating;
    }

    public void UpdateComment(string newComment)
    {
        if (string.IsNullOrWhiteSpace(newComment))
            throw new ArgumentException("Comment cannot be empty", nameof(newComment));
        
        if (newComment.Length > 500)
            throw new ArgumentException("Comment cannot exceed 500 characters", nameof(newComment));

        Comment = newComment;
    }

    public bool IsExcellent => Rating >= 4.5;
    public bool IsGood => Rating >= 3.5 && Rating < 4.5;
    public bool IsPoor => Rating < 2.5;
}