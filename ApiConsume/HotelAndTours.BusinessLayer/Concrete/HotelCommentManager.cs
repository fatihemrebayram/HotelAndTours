using HotelAndTours.BusinessLayer.Abstract;
using HotelAndTours.DataAccessLayer.Abstract;
using HotelAndTours.ModelsLayer.Models.UpdateCountsClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelAndTours.EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;

namespace HotelAndTours.BusinessLayer.Concrete;

    public class HotelCommentManager : IHotelCommentService
    {
        private readonly IHotelCommentDAL _commentDal;

        public HotelCommentManager(IHotelCommentDAL commentDal)
        {
            _commentDal = commentDal;
        }

        public void AddBL(HotelComment t)
        {
            _commentDal.InsertDAL(t);
        }

        public void AddRangeBL(List<HotelComment> p)
        {
            throw new NotImplementedException();
        }

        public List<HotelComment> GetListBL()
        {
            return _commentDal.ListDAL();
        }

        public IQueryable<HotelComment> GetListQueryableBL()
        {
            return _commentDal.ListQueryableBL().Include(x=>x.Hotel);
        }

        public List<HotelComment> GetListFilteredBL(string filter)
        {
            throw new NotImplementedException();
        }

        public void RemoveBL(HotelComment t)
        {
           _commentDal.DeleteDAL(t);
        }

        public HotelComment TGetByID(int id)
        {
            return _commentDal.GetByIdDAL(id);
        }

        public void UpdateBL(HotelComment t)
        {
           _commentDal.UpdateDAL(t);
        }
    }
