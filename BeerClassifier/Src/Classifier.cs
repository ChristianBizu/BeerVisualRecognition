using IBM.Cloud.SDK.Core.Authentication.Iam;
using IBM.Cloud.SDK.Core.Http;
using IBM.Cloud.SDK.Core.Http.Exceptions;
using IBM.Watson.VisualRecognition.v3;
using IBM.Watson.VisualRecognition.v3.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace BeerClassifier
{
    public class BeerClasifier
    {

        private const string API_KEY = "5S2-zoepnLrgIYttY5YaWcahm8yeZgLLwenH4eESZJlO";
        private const string IMG = "00000034.jpg";
        private const string API_URL = "https://gateway.watsonplatform.net/visual-recognition/api";

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            IamAuthenticator authenticator = new IamAuthenticator(
                apikey: API_KEY
                );

            VisualRecognitionService visualRecognition = new VisualRecognitionService("2018-03-19", authenticator);
            visualRecognition.SetServiceUrl(API_URL);
            visualRecognition.DisableSslVerification(true);
            visualRecognition.WithHeader("X-Watson-Learning-Opt-Out", "true");

            string service_response = null;
            try
            {
                service_response = GetClassifier(visualRecognition);

                //Console.WriteLine("DETALLES DEL MODELO\n\n" + service_response);

                service_response = Classify(visualRecognition);

                ClassifyResponseModel response = ClassifyResponseModel.ToModel(service_response);

                //Console.WriteLine("RESULTADO DE CLASIFICAR LA IMAGEN: "+ IMG + "\n\n" + service_response);
            }
            catch (ServiceResponseException e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
            Console.ReadKey();
        }

        private static string Classify(VisualRecognitionService visualRecognition)
        {
            DetailedResponse<ClassifiedImages> result = null;

            using (FileStream fs = File.OpenRead("C:\\Git\\" + IMG))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    fs.CopyTo(ms);
                    result = visualRecognition.Classify(
                        imagesFile: ms,
                        imagesFilename: IMG,
                        owners: new List<string>()
                        {
                                "me"
                        }
                    );
                }
            }
            return result.Response;
        }

        private static string GetClassifier(VisualRecognitionService visualRecognition) 
        {
            var result = visualRecognition.GetClassifier(
                            classifierId: "Cervezas_2141351471"
                        );

            return result.Response;
        }
    }   
}
