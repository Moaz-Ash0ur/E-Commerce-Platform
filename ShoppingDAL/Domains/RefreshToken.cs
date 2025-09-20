namespace ShoppingDAL.Domains
{
    public class RefreshToken 
  {
        public int Id { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public Guid UserId { get; set; } 
        public string? UpdatedBy { get; set; }       
        public int CurrentState { get; set; }      
        public DateTime CreatedDate { get; set; }       
        public string? CreatedBy { get; set; }       
        public DateTime? UpdatedDate { get; set; }

 }
        
    
}



