namespace ASPNetCoreECommerceSample.Entities
{
    public class Review
    {
        public int ReviewId { get; set; }
        public string UserName { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public string Email { get; set; }

        // Foreign key for the product
        public int ProductId { get; set; }
        public Product Product { get; set; }

        // Foreign key for the parent review (self-referencing)
        public int? ParentReviewId { get; set; }
        public Review ParentReview { get; set; }

        // Navigation property for child reviews (self-referencing)
        public ICollection<Review> Replies { get; set; } = new List<Review>();
    }

}
