using Microsoft.VisualBasic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assignment_DataStorage.Models.Entities
{
    public class CommentEntity
    {
        public int Id { get; set; }
        public string Comment { get; set; } = null!;

        
        public DateTime CommentCreatedAt { get; set; }

        public int TicketId { get; set; }
        public TicketEntity Ticket { get; set; } = null!;
    }
}