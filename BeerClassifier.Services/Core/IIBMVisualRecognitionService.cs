using IBM.Watson.VisualRecognition.v3;

namespace BeerClassifier.Services.Core
{
    public interface IIBMVisualRecognitionService
    {
        public void ConfigureService(string apiKey, string apiEndpoint, string apiModelId, string apiOwner);
        public string ClassifyImage(string imageName, string imageURL);
    }
}
