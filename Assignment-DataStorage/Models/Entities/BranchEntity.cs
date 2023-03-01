using System.Collections.Generic;

namespace Assignment_DataStorage.Models.Entities
{
    public class BranchEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public ICollection<TicketEntity> Tickets { get; set; } = new HashSet<TicketEntity>();
    }
}