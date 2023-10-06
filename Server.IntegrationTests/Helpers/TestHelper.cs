using System.Text.Json;

namespace Server.Helpers
{
    public class TestHelper
    {
        // Parse JSON response into model
        public T GetResponseContent<T>(HttpResponseMessage response) {
            string responseString = response.Content.ReadAsStringAsync().Result;

            // Convert from camel case to pascal case
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            T? returnedObject = System.Text.Json.JsonSerializer.Deserialize<T>(responseString, options);
            if(returnedObject == null) {
                throw new Exception("Returned object is null");
            }
            return returnedObject;
        }
    }
}