namespace FaceitSharp.Api.Internal.Models;

public class FaceitUserMe : FaceitUser
{
    [JsonPropertyName("birthdate")]
    public DateOfBirth Birthdate { get; set; } = new();

    [JsonPropertyName("created_by")]
    public string CreatedBy { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("email_verified")]
    public bool EmailVerified { get; set; }

    [JsonPropertyName("faceit_points")]
    public long FaceitPoints { get; set; }

    [JsonPropertyName("firstname")]
    public string FirstName { get; set; } = string.Empty;

    [JsonPropertyName("lastname")]
    public string LastName { get; set; } = string.Empty;

    [JsonPropertyName("mfa")]
    public string[] Mfa { get; set; } = [];

    [JsonPropertyName("phone")]
    public string Phone { get; set; } = string.Empty;

    [JsonPropertyName("roles")]
    public string[] Roles { get; set; } = [];

    [JsonPropertyName("updated_at")]
    public string UpdatedAt { get; set; } = string.Empty;

    [JsonPropertyName("updated_by")]
    public string UpdatedBy { get; set; } = string.Empty;

    public partial class DateOfBirth
    {
        [JsonPropertyName("date")]
        public string Date { get; set; } = string.Empty;

        [JsonPropertyName("month")]
        public string Month { get; set; } = string.Empty;

        [JsonPropertyName("year")]
        public string Year { get; set; } = string.Empty;
    }
}
