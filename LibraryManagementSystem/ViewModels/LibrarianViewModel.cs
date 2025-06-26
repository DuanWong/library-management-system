using System.ComponentModel.DataAnnotations;

public class LibrarianViewModel
{
    public string? Id { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }

    [Phone]
    [RegularExpression(@"^(\+1\s?)?(\(?\d{3}\)?[\s\-]?)\d{3}[\s\-]?\d{4}$",
        ErrorMessage = "Enter a valid phone number like (204) 999 5475, +1 (204) 999 5475, or 204-999-5475")]
    public string PhoneNumber { get; set; }
}
