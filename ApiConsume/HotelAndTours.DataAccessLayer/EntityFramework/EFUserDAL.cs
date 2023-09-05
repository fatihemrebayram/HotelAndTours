using DataAccessLayer.Abstract;
using DataAccessLayer.Repositories;
using EntityLayer.Concrete;
using HotelAndTours.DataAccessLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.EntityFramework
{
    public class EFUserDAL : GenericRepositories<AppUser>, IUserDAL
    {
        public EFUserDAL(Context context) : base(context)
        {
        }
    }
}