using BeerClassifier.Services.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BeerClassifier.Services.Model
{
    public class Score_class_model
    {
        [JsonProperty("class")]
        public string Model_class { get; set; }

        [JsonProperty("score")]
        public double Model_score { get; set; }
    }

    public class Image_Classifier
    {
        [JsonProperty("classifier_id")]
        public string Classifier_id { get; set; }

        [JsonProperty("name")]
        public string Classifier_name { get; set; }

        [JsonProperty("classes")]
        public List<Score_class_model> Classes_list { get; set; }
    }

    public class Processed_image
    {
        [JsonProperty("classifiers")]
        public List<Image_Classifier> Classifiers_list { get; set; }

        [JsonProperty("image")]
        public string Image_Name { get; set; }
    }

    public class IBMClassifyResponseModel
    {
        [JsonProperty("images")]
        public List<Processed_image> Image_procesed_list { get; set; }

        [JsonProperty("images_processed")]
        public int Images_processed_count { get; set; }

        [JsonProperty("custom_classes")]
        public int Custom_classes_count { get; set; }

        public ResponseResult ResponseResult => Custom_classes_count > 0 ? ResponseResult.OK : ResponseResult.ERROR;

        public string ResponseDesc { get; set; }

        public static IBMClassifyResponseModel ToModel(string jsonModel)
        {
            return JsonConvert.DeserializeObject<IBMClassifyResponseModel>(jsonModel);
        }
    }

    public class ProcessedImageResponseModel 
    {
        public ResponseResult Result => Classes.Count > 0 ? ResponseResult.OK : ResponseResult.ERROR;
        public List<Classifier_class> Classes { get; set; }

        public static ProcessedImageResponseModel ToModel(GetClassifiersReponse reponse)
        {
            return new ProcessedImageResponseModel() { Classes = reponse.Classes };
        }
    }

    public class ClassifyResponseViewModel 
    {
        public string Class { get; set; }
        public double Accuracy { get; set; }

        public Beer Beer { get; set; }

        public static ClassifyResponseViewModel ToModel(IBMClassifyResponseModel response) 
        {
            if (response.Image_procesed_list.Any()
                && response.Image_procesed_list.First().Classifiers_list.Any()
                && response.Image_procesed_list.First().Classifiers_list.First().Classes_list.Any())
            {
                var match = response.Image_procesed_list.First().Classifiers_list.First().Classes_list.First();

                var beer = BeerService.GetBeers().FirstOrDefault(x=> x.ClassName.Equals(match.Model_class));

                return new ClassifyResponseViewModel { Class = match.Model_class, Accuracy = match.Model_score, Beer = beer };
            }
            return new ClassifyResponseViewModel();
        }
    }

    public enum ResponseResult
    {
        OK,
        ERROR
    }

    public class Classifier_class
    {
        [JsonProperty("class")]
        public string Class_name { get; set; }
    }

    public class GetClassifiersReponse
    {
        [JsonProperty("class")]
        public string Classifier_id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("owner")]
        public string Owner { get; set; }
        [JsonProperty("created")]
        public DateTime Created { get; set; }
        [JsonProperty("updated")]
        public DateTime Updated { get; set; }
        [JsonProperty("classes")]
        public List<Classifier_class> Classes { get; set; }
        [JsonProperty("rscnn_enabled")]
        public bool Rscnn_enabled { get; set; }
        [JsonProperty("core_ml_enabled")]
        public bool Core_ml_enabled { get; set; }
    }



}
