using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Assignment_DataStorage.Models.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    public class BranchEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public ICollection<TicketEntity> Tickets { get; set; } = new HashSet<TicketEntity>();
    }
}