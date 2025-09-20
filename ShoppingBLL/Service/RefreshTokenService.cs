using AutoMapper;
using ShoppingBLL.Contracts;
using ShoppingBLL.DTOs;
using ShoppingDAL.Domains;
using ShoppingDAL.Repositories;

namespace ShoppingBLL.Services
{
    public class RefreshTokenService : IRefreshTokens
    {

        private IGenericRepository<RefreshToken> _repo;
        private IMapper _mapper;
        private readonly IUserService _userService;

        public RefreshTokenService(IGenericRepository<RefreshToken> repo, IMapper mapper,
            IUserService userService)
        {
            _repo = repo;
            _mapper = mapper;
            _userService = userService;
        }


        public bool Refresh(RefreshTokenDto tokenDto)
        {
            var allTokens = _repo.GetList(r => r.UserId == tokenDto.UserId && r.CurrentState == 0);

            foreach (var dbToken in allTokens)
            {
                ChangeStatus(dbToken.Id, tokenDto.UserId);
            }

            var newToken = _mapper.Map<RefreshToken>(tokenDto);
            newToken.CreatedBy = _userService.GetLoggedInUser();
            newToken.CreatedDate = DateTime.Now;
            _repo.Add(newToken);
            _repo.Save();
            return true;
        }

        public bool IsExpireToken(string token)
        {
            var storedToken = GetByToken(token);

            if (storedToken == null || storedToken.CurrentState == 1 || storedToken.ExpiresAt < DateTime.UtcNow)
            {
                return true;
            }
            return false;
        }

        public RefreshTokenDto GetByToken(string token)
        {
            var myToken = _repo.GetFirst(r => r.Token == token);
            return _mapper.Map<RefreshTokenDto>(myToken);
        }

        private bool ChangeStatus(int ID, Guid userId, int Status = 1)
        {

            var entity = _repo.GetById(ID);

            if (entity != null)
            {
                entity.CurrentState = Status;
                entity.UpdatedDate = DateTime.Now;
                entity.UpdatedBy = userId.ToString();
                return true;
            }
            return false;
        }
    }


}
