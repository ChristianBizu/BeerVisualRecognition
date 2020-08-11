using Newtonsoft.Json;
using System.Collections.Generic;

namespace BeerClassifier
{
    public class Model_Class
    {
        [JsonProperty("class")]
        public string Model_class { get; set; }

        [JsonProperty("score")]
        public double Model_score { get; set; }
    }

    public class Clasifier
    {
        [JsonProperty("classifier_id")]
        public string Classifier_id { get; set; }

        [JsonProperty("name")]
        public string Classifier_name { get; set; }

        [JsonProperty("classes")]
        public List<Model_Class> Classes_list { get; set; }
    }

    public class Image
    {
        [JsonProperty("classifiers")]
        public List<Clasifier> Classifiers_list { get; set; }

        [JsonProperty("image")]
        public string Image_Name { get; set; }
    }

    public class ClassifyResponseModel
    {
        [JsonProperty("images")]
        public List<Image> Image_procesed_list { get; set; }

        [JsonProperty("images_processed")]
        public int Images_processed_count { get; set; }

        [JsonProperty("custom_classes")]
        public int Custom_classes_count { get; set; }

        public static ClassifyResponseModel ToModel(string jsonModel)
        {
            return JsonConvert.DeserializeObject<ClassifyResponseModel>(jsonModel);
        }
    }

}