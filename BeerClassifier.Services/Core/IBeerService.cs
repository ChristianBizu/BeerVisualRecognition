using BeerClassfier.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BeerClassifier.Services.Core
{
    public interface IBeerService
    {
        IList<Beer> GetBeers();
        Beer GetBeerByClassName(string className);
    }
}
