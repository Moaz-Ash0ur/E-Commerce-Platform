namespace ShoppingBLL.DTOs
{
    public class RefreshTokenDto
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public Guid UserId { get; set; }
        public DateTime ExpiresAt { get; set; }
        public int CurrentState { get; set; }
    }


}
