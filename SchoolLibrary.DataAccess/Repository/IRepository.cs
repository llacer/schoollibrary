using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLibrary.DataAccess.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IList<TEntity> Initialize();
        TEntity Find(int id);
        IList<TEntity> Get();
        void Update(TEntity entity);
        void Delete(int id);
        void Insert(TEntity entity);
        void SaveChanges();
    }
}
