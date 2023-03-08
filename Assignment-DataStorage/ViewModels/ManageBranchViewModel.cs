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
	private BranchModel selectedBranch = null!;

	[ObservableProperty]
	private BranchModel branch = null!;

    [RelayCommand]
    private async void SaveBranchToDBAsync()
    {
        if (Branch != null)
        {
            await BranchService.SaveBranchAsync(Branch);
			Branch = new BranchModel();
            populateBranchCollectionAsync();
        }
    }

	[RelayCommand]
	private async void DeleteBranchFromDBAsync()
	{
		if (SelectedBranch != null)
		{
			await BranchService.DeleteBranchAsync(SelectedBranch);
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
