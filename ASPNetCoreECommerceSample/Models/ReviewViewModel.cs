namespace ASPNetCoreECommerceSample.Models
{
    public class ReviewViewModel
    {
        public int ReviewId { get; set; }
        public int ProductId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public List<ReviewViewModel> Replies { get; set; } = new List<ReviewViewModel>();
        public int? ParentReviewId { get; set; }
    }
}
