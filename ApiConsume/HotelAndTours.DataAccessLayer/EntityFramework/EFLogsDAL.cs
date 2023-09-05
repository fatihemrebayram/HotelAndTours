using DataAccessLayer.Abstract;
using DataAccessLayer.Repositories;
using EntityLayer.Concrete;
using HotelAndTours.DataAccessLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.EntityFramewok
{
    public class EFLogsDAL : GenericRepositories<Logs>, ILogDAL
    {
        public EFLogsDAL(Context context) : base(context)
        {
        }
    }
}