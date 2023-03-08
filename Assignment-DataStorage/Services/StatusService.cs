using Assignment_DataStorage.Contexts;
using Assignment_DataStorage.Models.Entities;
using Assignment_DataStorage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Net.NetworkInformation;

namespace Assignment_DataStorage.Services;

internal class StatusService
{

    private static DataContext _dataContext = new();

    public static async Task SaveStatusAsync(StatusModel model)
    {
        var _status = new StatusEntity
        {
            Status = model.Status,
        };

        _dataContext.Status.Add(_status);
        await _dataContext.SaveChangesAsync();
    }

    public static async Task<IEnumerable<StatusModel>> GetAllStatusAsync()
    {
        var _statuses = new ObservableCollection<StatusModel>();

        foreach (var _status in await _dataContext.Status.ToListAsync())
            _statuses.Add(new StatusModel
            {
                Id = _status.Id,
                Status = _status.Status,
            });

        return _statuses;
    }

    public static async Task DeleteStatusAsync(StatusModel model)
    {
        var _status = await _dataContext.Status.FirstOrDefaultAsync(x => x.Status == model.Status);
        if (_status != null)
        {
            _dataContext.Status.Remove(_status);
            await _dataContext.SaveChangesAsync();
        }
    }
}
