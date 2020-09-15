using BeerClassfier.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MongoRepository
{
    public class MongoDB : IMongoDB
    {
        public IMongoDatabase Table { get; set; }
        public MongoDB() 
        {
            var client = new MongoClient("mongodb+srv://dataowner:n0CQJ13hlQYAOzgi@cluster0.slacy.mongodb.net/<dbname>?retryWrites=true&w=majority");
            Table = client.GetDatabase("Beer");
        }
        public IMongoCollection<Beer> BeersCollection
        {
            get
            {
                return Table.GetCollection<Beer>("Beers");
            }
        }

        public Beer GetBeerById()
        {
            throw new NotImplementedException();
        }

        public IList<Beer> GetAllBeers()
        {
            try
            {
                return BeersCollection.Find(_ => true).ToList();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }

        private ObjectId GetInternalId(string id)
        {
            ObjectId internalId;
            if (!ObjectId.TryParse(id, out internalId))
                internalId = ObjectId.Empty;

            return internalId;
        }


        public Beer GetBeer(string id)
        {
            try
            {
                ObjectId internalId = GetInternalId(id);
                return BeersCollection
                                .Find(beer => beer._id == internalId)
                                .FirstOrDefault();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }


        public bool UpdateBeer(string id, Beer item)
        {
            try
            {

                ObjectId internalId = GetInternalId(id);

                ReplaceOneResult actionResult
                    = BeersCollection.ReplaceOne(n => n._id.Equals(internalId)
                                            , item
                                            , new ReplaceOptions { IsUpsert = true });
                return actionResult.IsAcknowledged
                    && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                // log or manage the exception
                throw ex;
            }
        }


        
    }
}
