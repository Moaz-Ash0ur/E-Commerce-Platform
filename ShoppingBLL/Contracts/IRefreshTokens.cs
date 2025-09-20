using ShoppingBLL.DTOs;

namespace ShoppingBLL.Contracts
{
    public interface IRefreshTokens
    {
        public bool Refresh(RefreshTokenDto tokenDto);
        public bool IsExpireToken(string token);
        public RefreshTokenDto GetByToken(string token);

    }






}
