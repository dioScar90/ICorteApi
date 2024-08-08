using ICorteApi.Domain.Base;

namespace ICorteApi.Domain.Entities;

public class Message : BasePrimaryKeyEntity<int>
{
    public string Content { get; set; }
    public DateTime SentAt { get; set; }
    public bool IsRead { get; set; } = false;

    public int AppointmentId { get; set; }
    public User Appointment { get; set; }

    public int SenderId { get; set; }
    public User Sender { get; set; }
}
