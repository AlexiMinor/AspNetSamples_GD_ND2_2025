namespace AspNetSamples.Models;

public class CreateArticleModel //: IValidatableObject
{
    //[Required(ErrorMessage = "Title of article is required")]
    public string Title { get; set; }
    
    //[Required]
    public string Description { get; set; }
    //[Required]
    public string Text { get; set; }
    //[Required]
    public Guid SourceId { get; set; }

    public List<SourceModel> Sources { get; set; }
    //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    //{
    //    var errors = new List<ValidationResult>();
    //    // add validation logic here
    //    if (string.IsNullOrWhiteSpace(Title))
    //    {
    //        errors.Add(new ValidationResult("Title is required", new[] { nameof(Title) }));
    //    }
    //    if (string.IsNullOrWhiteSpace(Description))
    //    {
    //        errors.Add(new ValidationResult("Description is required", new[] { nameof(Description) }));
    //    }
    //    if (string.IsNullOrWhiteSpace(Content))
    //    {
    //        errors.Add(new ValidationResult("Content is required", new[] { nameof(Content) }));
    //    }
    //    if (SourceId == Guid.Empty)
    //    {
    //        errors.Add(new ValidationResult("Source is required", new[] { nameof(SourceId) }));
    //    }
    //    return errors;
    //}
}