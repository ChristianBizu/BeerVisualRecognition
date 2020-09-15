using BeerClassfier.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MongoRepository
{
    public interface IMongoDB
    {
        IList<Beer> GetAllBeers();
        Beer GetBeer(string id);
        // update just a single document / Beer
        bool UpdateBeer(string id, Beer item);

        // demo interface - full document update

    }
}
