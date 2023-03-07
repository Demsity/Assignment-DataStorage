using Microsoft.VisualBasic;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment_DataStorage.Models.Entities
{
    public class TicketEntity
    {
        public int Id { get; set; }

        public string Description { get; set; } = null!;

        
        public DateTime TicketCreatedAt { get; set; }

        public int CommentId { get; set; }
        public CommentEntity? Comment { get; set; }

        public int CustomerId { get; set; }
        public CustomerEntity Customer { get; set; } = null!;

        public int BranchId { get; set; }
        public BranchEntity Branch { get; set; } = null!;

        public int StatusId { get; set; }
        public StatusEntity Status { get; set; } = null!;
    }
}