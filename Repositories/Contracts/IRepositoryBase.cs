using System.Linq.Expressions;

/*
<summary>
    Burada, tanımlayacağımız repolarda kullanacağımız temel CRUD fonksiyonlarının imzalarını tanımladık.
    Bu da bir interface aracılığı ile tanımlandığı için bir interface tanımı yaptık.
</summary>
*/

namespace Repositories.Contracts
{
    public interface IRepositoryBase<T>
    {
        //CRUD
        IQueryable<T> FindAll(bool trackChanges); //IQueryable sorgulanabilir liste. T bir type temsil ediyor. Generic programlama.
        IQueryable<T> FindByCondition(Expression<Func<T,bool>> expression ,bool trackChanges);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
