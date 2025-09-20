using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using ShoppingDAL.Domains;
using ShoppingDAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBLL.Contracts
{
    public interface IBaseService<T, DTO> where T : class
    {
        IEnumerable<DTO> GetAll();
        IQueryable<DTO> GetAllQueryable();
        DTO GetByID(int Id);
        bool Insert(DTO entity);
        //bool Insert(DTO entity, out int Id);
        //bool ChangeStatus(int ID, string userId, int status = 1);
        bool Update(DTO entity);
    }


    public class BaseService<T, DTO> : IBaseService<T, DTO> where T : class
    {

        private readonly IGenericRepository<T> _repo;
        private readonly IMapper _mapper;

        public BaseService(IGenericRepository<T> repo, IMapper mapper, IUserService userService)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public IEnumerable<DTO> GetAll()
        {
            var dbObject = _repo.GetAll();

            return _mapper.Map<List<T>, List<DTO>>((List<T>)dbObject);
        }

        public IQueryable<DTO> GetAllQueryable()
        {
            var dbObject = _repo.GetAllAsQueryable();
            return _mapper.Map<IQueryable<T>, IQueryable<DTO>>(dbObject);
        }

        public DTO GetByID(int Id)
        {
            var dbObject = _repo.GetById(Id);
            return _mapper.Map<T, DTO>(dbObject);
        }

        public bool Insert(DTO entity)
        {
            var dbObject = _mapper.Map<DTO, T>(entity);
            return _repo.Add(dbObject);
        }

        //public bool Insert(DTO entity, out int Id)
        //{
        //    var dbObject = _mapper.Map<DTO, T>(entity);
        //    return _repo.Add(dbObject, out Id);
        //}

        public bool Update(DTO entity)
        {
            var dbObject = _mapper.Map<DTO, T>(entity);
            return _repo.Update(dbObject) ? true : false;
        }

        //public bool ChangeStatus(int ID, string userId, int status = 1)
        //{
        //    return _repo.ChangeStatus(ID, _userService.GetLoggedInUser(), status);
        //}

         

    }


}
