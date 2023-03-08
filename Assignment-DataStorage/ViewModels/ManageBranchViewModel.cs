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

internal partial class ManageBranchViewModel : ObservableObject
{
	[ObservableProperty]
	private ObservableCollection<BranchModel> branches = new();

	[ObservableProperty]
	private BranchModel branch = null!;

    [RelayCommand]
    private async void SaveBranchToDB()
    {
        if (Branch != null)
        {
            await BranchService.SaveBranchAsync(Branch);
			Branch = new BranchModel();
            populateBranchCollectionAsync();
        }
    }
    public ManageBranchViewModel()
	{
		populateBranchCollectionAsync();
		Branch= new BranchModel();
	}

	private async void populateBranchCollectionAsync()
	{
		Branches = (ObservableCollection<BranchModel>)await BranchService.GetAllBranchesAsync();
	}


}
