using BeerClassifier.Services.Core;
using BeerClassifier.Services.Model;
using IBM.Cloud.SDK.Core.Authentication.Iam;
using IBM.Cloud.SDK.Core.Http;
using IBM.Cloud.SDK.Core.Http.Exceptions;
using IBM.Watson.VisualRecognition.v3;
using IBM.Watson.VisualRecognition.v3.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BeerClassifier.Services.Infrastructure
{
    public class IBMVisualRecognitionService : IIBMVisualRecognitionService
    {
        private IamAuthenticator authenticator;
        private VisualRecognitionService visualRecognition;

        private string API_KEY;
        private string API_ENPOINT;
        private string API_MODEL_ID;
        private string API_OWNER;

        public IList<Classifier_class> Classes = new List<Classifier_class>();

        public IBMVisualRecognitionService() 
        {
            
        }

        public void ConfigureService(string apiKey, string apiEndpoint, string apiModelId, string apiOwner) 
        {
            API_KEY = apiKey;
            API_ENPOINT = apiEndpoint;
            API_MODEL_ID = apiModelId;
            API_OWNER = apiOwner;

            authenticator = new IamAuthenticator(
                       apikey: API_KEY
                       );

            visualRecognition = new VisualRecognitionService("2018-03-19", authenticator);
            visualRecognition.SetServiceUrl(API_ENPOINT);
            visualRecognition.DisableSslVerification(true);
            visualRecognition.WithHeader("X-Watson-Learning-Opt-Out", "true");
            
            GetClassifier();
        }

        public IBMClassifyResponseModel ClassifyImage(string imageName, string imageURL)
        {
            try
            {
                var classify_reponse = Classify(imageName, imageURL);

                IBMClassifyResponseModel response = IBMClassifyResponseModel.ToModel(classify_reponse);

                return response;
            }
            catch (ServiceResponseException e)
            {
                return new IBMClassifyResponseModel() { ResponseDesc = e.Message, Custom_classes_count = 0 };
            }
            catch (Exception e)
            {
                return new IBMClassifyResponseModel() { ResponseDesc = e.Message, Custom_classes_count = 0 };
            }
        }

        private string Classify(string imageName, string imageURL)
        {
            DetailedResponse<ClassifiedImages> result = null;

            using (FileStream fs = File.OpenRead(imageURL))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    fs.CopyTo(ms);
                    result = visualRecognition.Classify(
                        imagesFile: ms,
                        imagesFilename: imageName,
                        owners: new List<string>()
                        {
                                API_OWNER
                        }
                    );
                }
            }
            return result.Response;
        }

        private ProcessedImageResponseModel GetClassifier()
        {
            var result = visualRecognition.GetClassifier(
                            classifierId: API_MODEL_ID
                        );

            GetClassifiersReponse myDeserializedClass = JsonConvert.DeserializeObject<GetClassifiersReponse>(result.Response);

            ProcessedImageResponseModel processed = ProcessedImageResponseModel.ToModel(myDeserializedClass);

            Classes = processed.Classes;

            return processed;
        }
    }
}
