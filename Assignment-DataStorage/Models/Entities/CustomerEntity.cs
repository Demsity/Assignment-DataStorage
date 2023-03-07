﻿using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_DataStorage.Models.Entities
{
    [Index(nameof(Email), IsUnique = true)]
    public class CustomerEntity
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; } = null!;

        [StringLength(50)]
        public string LastName { get; set; } = null!;

        [StringLength(320)]
        public string Email { get; set; } = null!;

        [Column(TypeName = "char(11)")]
        public string PhoneNumber { get; set; } = null!;

        
        public DateTime CustomerCreatedAt { get; set; }

        public ICollection<TicketEntity> Tickets = new HashSet<TicketEntity>();

    }
}
