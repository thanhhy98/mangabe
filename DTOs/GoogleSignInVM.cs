
using System.ComponentModel.DataAnnotations;

namespace WebApplication3.DTOs;

public class GoogleSignInVM
{
    public GoogleSignInVM(string idToken)
    {
        IdToken = idToken;
    }

    /// <summary>
    /// This token being passed here is generated from the client side when a request is made  to 
    /// i.e. react, angular, flutter etc. It is being returned as A jwt from google oauth server. 
    /// </summary>
    [Required]
    public string IdToken { get; set; } 
}