using Assignment_DataStorage.Models;
using Assignment_DataStorage.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_DataStorage.ViewModels;

internal partial class ManageStatusViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<StatusModel> statuses = new();

    [ObservableProperty]
    private StatusModel selectedStatus = null!;

    [ObservableProperty]
    private StatusModel status = null!;

    [RelayCommand]
    private async void SaveStatusToDBAsync()
    {
        if (Status != null)
        {
            await StatusService.SaveStatusAsync(Status);
            Status = new StatusModel();
            populateStatusCollectionAsync();
        }
    }

    [RelayCommand]
    private async void DeleteStatusFromDBAsync()
    {
        if (SelectedStatus != null)
        {
            await StatusService.DeleteStatusAsync(SelectedStatus);
            populateStatusCollectionAsync();
        }
    }
    public ManageStatusViewModel()
    {
        populateStatusCollectionAsync();
        Status = new StatusModel();
    }

    private async void populateStatusCollectionAsync()
    {
        Statuses = (ObservableCollection<StatusModel>)await StatusService.GetAllStatusAsync();
    }
}
