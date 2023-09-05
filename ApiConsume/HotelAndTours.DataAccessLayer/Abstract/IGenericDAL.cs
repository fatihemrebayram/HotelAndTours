using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
    public interface IGenericDAL<T>
    {
        void AddRangeDAL(List<T> p);

        void DeleteDAL(T p);

        T GetByIdDAL(int id);

        T GetDAL(Expression<Func<T, bool>> filter);

        void InsertDAL(T p);

        List<T> ListDAL();
        IQueryable<T> ListQueryableBL();


        List<T> ListDAL(Expression<Func<T, bool>> filter);
        IQueryable<T> ListQueryableBL(Expression<Func<T, bool>> filter);

        void UpdateDAL(T p);
    }
}