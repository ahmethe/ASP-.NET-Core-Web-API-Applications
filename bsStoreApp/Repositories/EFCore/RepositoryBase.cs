using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using System.Linq.Expressions;

/*
<summary>
    IRepositoryBase interfacesi implemente edilerek fonksiyonların gövdeleri yazıldı.
    Bu sınıf abstract tanımlanarak kendisinden kalıtım alan alt sınıflara yani 
    concrete repositorylere fonksiyonların mantıklarını aktaracak ve ayrıca repositoryler
    kendilerine ihtiyaç duydukları fonksiyonları ayrıca tanımlayabilecek. Bu sınıf repository işlemleri
    için base görevi görecek. Bu işlemler yapılırken concrete repositoryler repository contexte 
    ihtiyaç duyduğu için bu alan protected tanımlandı.
</summary>
*/

namespace Repositories.EFCore
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T>
        where T : class
    {
        protected readonly RepositoryContext _context;

        public RepositoryBase(RepositoryContext context)
        {
            _context = context;
        }

        public void Create(T entity) => _context.Set<T>().Add(entity);

        public void Delete(T entity) => _context.Set<T>().Remove(entity);

        public IQueryable<T> FindAll(bool trackChanges) => 
            !trackChanges ? 
            _context.Set<T>().AsNoTracking() :
            _context.Set<T>();

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression,
            bool trackChanges) =>
            !trackChanges ?
            _context.Set<T>().Where(expression).AsNoTracking() :
            _context.Set<T>().Where(expression);

        public void Update(T entity) => _context.Set<T>().Update(entity);
    }
}
