using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_DataStorage.Models;

internal class StatusModel
{
    public int Id { get; set; }

    public string Status { get; set; } = null!;
}
