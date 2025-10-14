namespace AspNetSamples.WebAPI.Model;

public class TokenModel
{
    public string AccessToken { get; set; }
    public Guid RefreshToken { get; set; }

    //OPTIONAL
    //public DateTime ExpiresIn { get; set; }

}