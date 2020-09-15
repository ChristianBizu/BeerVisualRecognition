using System;
using System.Collections.Generic;
using System.Text;

namespace BeerClassifier.Services.Infrastructure
{
    public class BeerService
    {
        public static List<Beer> GetBeers() 
        {
            return new List<Beer>()
            {
                new Beer { Name = "Estrella Galicia", ClassName = "test_estrella", Desc = "Cerveza de color dorado brillante que parte de una selección de maltas y lúpulo especialmente amargos y su proceso de cocción, fermentación y maduración transcurre a lo largo de más de 20 días. Ello hace que esta cerveza tenga un agradable y característico sabor lupulado.", Imgurl = "https://images-na.ssl-images-amazon.com/images/I/61VVrn%2Bbx%2BL._AC_SL1200_.jpg" },
                new Beer { Name = "Heineken", ClassName = "tesd_heineken", Desc = "Una de las cervezas más icónicas del mundo, que comenzó como una pequeña cervecera en Ámsterdam en 1873. Hoy, cuatro generaciones de familia después, más de 25 millones de Heineken® se consumen cada día en 192 países. Ahora también puedes disfrutar de Heineken® 0.0, todo el sabor de esta cerveza Premium con 0.0% alcohol.", Imgurl = "https://images-na.ssl-images-amazon.com/images/I/41Q8lBcVI8L._AC_.jpg" },
                new Beer { Name = "CruzCampo", ClassName = "test_cruzcampo", Desc = "Cerveza Lager Especial, con un contenido alcohólico de 5,6% en volumen y color rubio brillante. Destaca por su suave amargor, el dulce anisado de la malta y por su aroma frutal a manzana. Cruzcampo Especial tiene una personalidad propia y un carácter refrescante único, inspirada en la receta centenaria de 1904.", Imgurl = "https://images-na.ssl-images-amazon.com/images/I/81DcBApqazL._AC_SL1500_.jpg" }
            };
        }
    }

    public class Beer 
    {
        public string Name { get; set; }

        public string Desc { get; set; }

        public string Imgurl { get; set; }

        public string ClassName { get; set; }
    }
}
