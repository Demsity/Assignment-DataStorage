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

namespace Assignment_DataStorage.Services;

internal class BranchService
{

    private static DataContext _dataContext = new();

    public static async Task SaveBranchAsync(BranchModel model)
    {
        var _branch = new BranchEntity
        {
            Name = model.Name,
        };

        _dataContext.Branches.Add(_branch);
        await _dataContext.SaveChangesAsync();
    }

    public static async Task<IEnumerable<BranchModel>> GetAllBranchesAsync()
    {
        var _branches = new ObservableCollection<BranchModel>();

        foreach (var _branch in await _dataContext.Branches.ToListAsync())
            _branches.Add(new BranchModel
            {
                Id = _branch.Id,
                Name = _branch.Name,
            });

        return _branches;
    }

    public static async Task DeleteBranchAsync(BranchModel model)
    {
        var _branch = await _dataContext.Branches.FirstOrDefaultAsync(x => x.Name == model.Name);
        if (_branch != null)
        {
            _dataContext.Branches.Remove(_branch);
            await _dataContext.SaveChangesAsync();
        }
    }
}
