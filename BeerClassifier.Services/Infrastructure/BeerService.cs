using BeerClassfier.Entities;
using BeerClassifier.Services.Core;
using MongoRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeerClassifier.Services.Infrastructure
{
    public class BeerService : IBeerService
    {
        private readonly IMongoDB _database;

        public BeerService(IMongoDB database) 
        {
            _database = database;
        }

        public IList<Beer> GetBeers() 
        {
            return _database.GetAllBeers();
        }

        public Beer GetBeerByClassName(string className) 
        {
            return GetBeers().FirstOrDefault(x => x.ClassName == className);
        }
    }

    public class BeerViewModel
    {
        public string Name { get; set; }

        public string Desc { get; set; }

        public string Imgurl { get; set; }

        public string ClassName { get; set; }

        public static BeerViewModel FromModel(Beer model) 
        {
            return new BeerViewModel() { Name = model.Name, Imgurl = model.Imgurl, ClassName = model.ClassName, Desc = model.Desc };
        }
    }
}
