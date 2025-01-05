using System;
using System.Security.Cryptography;
using System.Text;

public class SignedUrlGenerator
{
    
    
    public string GenerateSignedUrl(string baseUrl, string authorizationToken)
    {

        // Create the URL to sign
        string stringToSign = $"{baseUrl}?Authorization={authorizationToken}";
        string signature = SignUrl(stringToSign, authorizationToken);

        return $"{stringToSign}&Signature={signature}";
    }

    private string SignUrl(string data, string secretKey)
    {
        using var hmac = new HMACSHA1(Encoding.UTF8.GetBytes(secretKey));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
        return Convert.ToBase64String(hash);
    }
}