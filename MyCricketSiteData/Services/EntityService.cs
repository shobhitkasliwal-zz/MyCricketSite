using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MyCricketSiteData.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCricketSiteData.Services
{
    public interface IEntityService<T> where T : IMongoEntity
    {
        void Create(T entity);

        void Delete(string id);

        T GetById(string id);

        void Update(T entity);
    }

    public abstract class EntityService<T> : IEntityService<T> where T : IMongoEntity
    {
        protected readonly DbContext<T> DBConnectionHandler;

        public virtual void Create(T entity)
        {
            //// Save the entity with safe mode (WriteConcern.Acknowledged)
            var result = this.DBConnectionHandler.DBCollection.Save(
                entity,
                new MongoInsertOptions
                {
                    WriteConcern = WriteConcern.Acknowledged
                });

            if (!result.Ok)
            {
                //// Something went wrong
            }
        }

        public virtual void Delete(string id)
        {
            var result = this.DBConnectionHandler.DBCollection.Remove(
                Query<T>.EQ(e => e.Id,
                new ObjectId(id)),
                RemoveFlags.None,
                WriteConcern.Acknowledged);

            if (!result.Ok)
            {
                //// Something went wrong
            }
        }

        protected EntityService()
        {
            DBConnectionHandler = new DbContext<T>();
        }

        public virtual T GetById(string id)
        {
            var entityQuery = Query<T>.EQ(e => e.Id, new ObjectId(id));
            return this.DBConnectionHandler.DBCollection.FindOne(entityQuery);
        }

        public abstract void Update(T entity);
    }
}
