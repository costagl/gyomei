using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Gyomei.Models;

[Index("Email", Name = "UQ__Users__A9D105343B86CA8F", IsUnique = true)]
public partial class User
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Name { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string PasswordHash { get; set; } = null!;

    public int RoleId { get; set; }

    [ForeignKey("RoleId")]
    [InverseProperty("Users")]
    public virtual Role Role { get; set; } = null!;

    [InverseProperty("AssigneeUser")]
    public virtual ICollection<Ticket> TicketAssigneeUsers { get; set; } = new List<Ticket>();

    [InverseProperty("AuthorUser")]
    public virtual ICollection<TicketComment> TicketComments { get; set; } = new List<TicketComment>();

    [InverseProperty("CreatorUser")]
    public virtual ICollection<Ticket> TicketCreatorUsers { get; set; } = new List<Ticket>();
}
