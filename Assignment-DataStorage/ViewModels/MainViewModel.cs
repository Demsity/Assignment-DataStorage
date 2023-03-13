using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_DataStorage.ViewModels;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableObject currentViewModel;

    [RelayCommand]
    private void GoToViewAll()
    {
        CurrentViewModel= new ViewAllViewModel();
    }

    [RelayCommand]
    private void GoToManageBranch()
    {
        CurrentViewModel = new ManageBranchViewModel();
    }

    [RelayCommand]
    private void GoToManageStatus()
    {
        CurrentViewModel = new ManageStatusViewModel();
    }

    public MainViewModel()
    {
        currentViewModel = new ViewAllViewModel();
    }
}
