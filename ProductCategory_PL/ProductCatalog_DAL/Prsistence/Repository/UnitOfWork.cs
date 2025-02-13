using ProductCatalog_DAL.IRepository;
using ProductCatalog_DAL.Models;
using ProductCatalog_DAL.Prsistence.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ProductCatalog_DAL.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ProductContext _dbContext;
        //private Dictionary<string, GenericRepository<BaseEntity>> _reopsitory;
        private Hashtable _reopsitory;

     
        //But We can be Methode To be Code Better 
        //Path SortContext For Constructor 
        public UnitOfWork(ProductContext dbContext)
        {
            _dbContext = dbContext;
            //_reopsitory = new Dictionary<string, GenericRepository<BaseEntity>>();
            _reopsitory = new Hashtable();

         

        }
        public IGenericRepository<T> Repository<T>() where T : BaseEntity
        {
            //I can path the object and equal it there but that will make me add for every object and make equal in the Feuture So i will using Dictionary 


            //I need to get the key and Value
            var key = typeof(T).Name;

            if (!_reopsitory.ContainsKey(key))
            {
                //var value = new GenericRepository<T>(_dbContext) as GenericRepository<BaseEntity>;
                var value = new GenericRepository<T>(_dbContext);

                _reopsitory.Add(key, value);
            }

            return _reopsitory[key] as IGenericRepository<T>;

            //but i can use HashTable Foe make key object and value object 

        }

        public async Task<int> CompleteAsync()
         => await _dbContext.SaveChangesAsync();

        public async ValueTask DisposeAsync()
         => await _dbContext.DisposeAsync();

    }
}
