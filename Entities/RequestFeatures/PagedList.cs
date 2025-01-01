namespace Entities.RequestFeatures
{
    public class PagedList<T> : List<T>
    {
        // Instance member. Bu 2 ifadeye ulaşmak için newleme yapmak gerekir. new PagedList().
        public MetaData MetaData { get; set; }
        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            //Referans tipli ifadeler ya tanımlandığı yerde ya da constructorda newlenmelidir.
            MetaData = new MetaData()
            {
                TotalCount = count,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPage = (int)Math.Ceiling((double)count/pageSize)
            };
            AddRange(items);
        }

        // Class member. Sınıf aracılığıyla ulaşılır.
        public static PagedList<T> ToPagedList(IEnumerable<T> source,
            int pageNumber,
            int pageSize)
        {
            var count = source.Count(); // Koleksiyonların ortak özelliği.
            var items = source
                .Skip((pageNumber-1)*pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
