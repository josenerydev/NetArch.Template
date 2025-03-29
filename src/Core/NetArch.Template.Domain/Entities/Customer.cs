using NetArch.Template.Domain.Shared.Enums;

namespace NetArch.Template.Domain.Entities;

public class Customer
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime BirthDate { get; set; }
    public CustomerStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public Customer()
    {
        CreatedAt = DateTime.UtcNow;
        Status = CustomerStatus.Active;
    }
}
