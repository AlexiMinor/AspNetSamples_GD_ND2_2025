namespace AspNetSamples.WebAPI.Model;

public class UpdateArticleTextModel
{
    public string NewText { get; set; }
    public double Rate { get; set; }
}

public class UpdateArticleModel
{
    //key is property name, value is new value
    public Dictionary<string, object> Changes { get; set; }
}