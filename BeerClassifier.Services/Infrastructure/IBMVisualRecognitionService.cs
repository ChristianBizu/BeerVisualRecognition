using BeerClassifier.Services.Core;
using BeerClassifier.Services.Model;
using IBM.Cloud.SDK.Core.Authentication.Iam;
using IBM.Cloud.SDK.Core.Http;
using IBM.Cloud.SDK.Core.Http.Exceptions;
using IBM.Watson.VisualRecognition.v3;
using IBM.Watson.VisualRecognition.v3.Model;
using System;
using System.Collections.Generic;
using System.IO;

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
        }

        public string ClassifyImage(string imageName, string imageURL)
        {
            try
            {
                string service_response = GetClassifier();

                service_response = Classify(imageName, imageURL);

                IBMResponseModel response = IBMResponseModel.ToModel(service_response);

                return service_response;
            }
            catch (ServiceResponseException e)
            {
                return e.Message;
            }
            catch (Exception e)
            {
                return e.Message;
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

        private string GetClassifier()
        {
            var result = visualRecognition.GetClassifier(
                            classifierId: API_MODEL_ID
                        );

            return result.Response;
        }
    }
}
