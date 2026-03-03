using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Gyomei.Models;

public partial class Ticket
{
    [Key]
    public int Id { get; set; }

    [StringLength(150)]
    [Unicode(false)]
    public string Title { get; set; } = null!;

    [Column(TypeName = "text")]
    public string Description { get; set; } = null!;

    public int CreatorUserId { get; set; }

    public int? AssigneeUserId { get; set; }

    public int StatusId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("AssigneeUserId")]
    [InverseProperty("TicketAssigneeUsers")]
    public virtual User? AssigneeUser { get; set; }

    [ForeignKey("CreatorUserId")]
    [InverseProperty("TicketCreatorUsers")]
    public virtual User CreatorUser { get; set; } = null!;

    [ForeignKey("StatusId")]
    [InverseProperty("Tickets")]
    public virtual TicketStatus Status { get; set; } = null!;

    [InverseProperty("Ticket")]
    public virtual ICollection<TicketComment> TicketComments { get; set; } = new List<TicketComment>();
}
