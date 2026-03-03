using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Gyomei.Models;

public partial class TicketComment
{
    [Key]
    public int Id { get; set; }

    public int TicketId { get; set; }

    public int AuthorUserId { get; set; }

    [Column(TypeName = "text")]
    public string Message { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("AuthorUserId")]
    [InverseProperty("TicketComments")]
    public virtual User AuthorUser { get; set; } = null!;

    [ForeignKey("TicketId")]
    [InverseProperty("TicketComments")]
    public virtual Ticket Ticket { get; set; } = null!;
}
