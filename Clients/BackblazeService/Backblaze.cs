using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

public class BackblazeB2Service
{
    private readonly string applicationKeyId;
    private readonly string applicationKey;

    public BackblazeB2Service(string applicationKeyId, string applicationKey)
    {
        this.applicationKeyId = applicationKeyId;
        this.applicationKey = applicationKey;
    }

    public async Task<string> GetAuthorizationToken()
    {
        var authString = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{applicationKeyId}:{applicationKey}"));
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Basic {authString}");
            var response = await client.GetAsync("https://api.backblazeb2.com/b2api/v2/b2_authorize_account");
            var jsonResponse = JObject.Parse(await response.Content.ReadAsStringAsync());

            // Extract values
            return jsonResponse["authorizationToken"].ToString();
        }
    }
}