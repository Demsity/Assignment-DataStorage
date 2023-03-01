using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Assignment_DataStorage.Models.Entities
{
    public class StatusEntity
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = null!;

        public ICollection<TicketEntity> Tickets { get; set; } = new HashSet<TicketEntity>();
    }
}