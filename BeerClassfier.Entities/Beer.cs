using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BeerClassfier.Entities
{
    public class Beer
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId _id { get; set; }

        public string Name { get; set; }

        public string Desc { get; set; }

        public string Imgurl { get; set; }

        public string ClassName { get; set; }
    }
}