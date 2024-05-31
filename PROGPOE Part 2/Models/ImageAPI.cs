using System.IO;

namespace PROGPOE_Part_2.Models
{
    public class ImageAPI
    {
        private static HttpClient client = new HttpClient();
        public string baseURL = "https://firebaseimageapi20240531222435.azurewebsites.net/api/Files/";
        public string getRequest = "Url/";
        public string postRequest = "Upload";

        public ImageAPI()
        {

        }

        public async Task<string> makeGetRequest(string folderName, string fileName)
        {
            if (fileName != null)
            {
                string path = baseURL + getRequest + folderName + "%2F" + fileName + ".jpg";

                HttpResponseMessage response = await client.GetAsync(path);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }
            

            return null;
        }
    }
}
