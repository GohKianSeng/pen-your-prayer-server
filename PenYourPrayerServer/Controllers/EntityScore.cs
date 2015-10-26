using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace PenYourPrayer.Controllers
{
    internal class EntityStore<TEntity> where TEntity : class
    {
        /// <summary>
        /// Context for the store
        /// 
        /// </summary>
        public DbContext Context { get; private set; }

        /// <summary>
        /// Used to query the entities
        /// 
        /// </summary>
        public IQueryable<TEntity> EntitySet
        {
            get
            {
                return (IQueryable<TEntity>)this.DbEntitySet;
            }
        }

        /// <summary>
        /// EntitySet for this store
        /// 
        /// </summary>
        public DbSet<TEntity> DbEntitySet { get; private set; }

        /// <summary>
        /// Constructor that takes a Context
        /// 
        /// </summary>
        /// <param name="context"/>
        public EntityStore(DbContext context)
        {
            this.Context = context;
            this.DbEntitySet = context.Set<TEntity>();
        }

        /// <summary>
        /// FindAsync an entity by ID
        /// 
        /// </summary>
        /// <param name="id"/>
        /// <returns/>
        public virtual Task<TEntity> GetByIdAsync(object id)
        {
            return this.DbEntitySet.FindAsync(new object[1]
      {
        id
      });
        }

        /// <summary>
        /// Insert an entity
        /// 
        /// </summary>
        /// <param name="entity"/>
        public void Create(TEntity entity)
        {
            this.DbEntitySet.Add(entity);
        }

        /// <summary>
        /// Mark an entity for deletion
        /// 
        /// </summary>
        /// <param name="entity"/>
        public void Delete(TEntity entity)
        {
            this.DbEntitySet.Remove(entity);
        }

        /// <summary>
        /// Update an entity
        /// 
        /// </summary>
        /// <param name="entity"/>
        public virtual void Update(TEntity entity)
        {
            if (entity != null)
            {
                //this.Context.Entry<TEntity>(entity).set_State(16);
            }
        }
    }
}