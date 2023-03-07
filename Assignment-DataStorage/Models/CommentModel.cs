using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_DataStorage.Models;

internal class CommentModel
{
    public int TicketId { get; set; }
    public string Comment { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

}
