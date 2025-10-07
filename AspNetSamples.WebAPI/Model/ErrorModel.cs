namespace AspNetSamples.WebAPI.Model;

public class ErrorModel
{
    public string Message { get; set; }
    
    //possibly bad idea to expose stack trace in production
    public string? StackTrace { get; set; }
}