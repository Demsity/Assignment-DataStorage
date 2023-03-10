﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_DataStorage.Models;

public class TicketModel
{
    public int? Id { get; set; }

    public string Description { get; set; } = null!;

    public DateTime? TicketCreatedAt { get; set; }


    // Customer 
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public DateTime? CustomerCreatedAt { get; set; }

    // Comment
    public string? Comment { get; set; }
    public DateTime? CommentCreatedAt { get; set; }

    // Branch 
    public int BranchId { get; set; }
    public string Branch { get; set; } = null!;

    // Status
    public int StatusId { get; set; }
    public string Status { get; set; } = null!;

}
