using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_DataStorage.Models;

public class CommentModel
{
    public int Id { get; set; }
    public int TicketId { get; set; }
    public string Comment { get; set; } = null!;
    public DateTime CreatedAt { get; set; }

}
