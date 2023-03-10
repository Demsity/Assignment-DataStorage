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

public partial class ViewAllViewModel : ObservableObject
{
	[ObservableProperty]
	private ObservableCollection<TicketModel> tickets = null!;

	[ObservableProperty]
	private ObservableCollection<BranchModel> branches = null!;

	[ObservableProperty]
	private ObservableCollection<StatusModel> statuses = null!;

	[ObservableProperty]
	private TicketModel selectedTicket = null!;

	[ObservableProperty]
	private BranchModel selectedBranch = null!;

	[ObservableProperty]
	private StatusModel selectedStatus = null!;

    partial void OnSelectedTicketChanging(TicketModel value)
    {
		if (value != null)
		{
            if (value.BranchId > 0 && value.StatusId > 0)
            {
                SelectedBranch = Branches.FirstOrDefault(x => x.Id == value.BranchId)!;
                SelectedStatus = Statuses.FirstOrDefault(x => x.Id == value.StatusId)!;
            } else
			{
                SelectedBranch = new();
                SelectedStatus = new();
            }
        } else
		{
			SelectedBranch = new();
			SelectedStatus = new();
		}

    }

    [RelayCommand]
	private async void SaveTicket()
	{
		var _ticket = await TicketService.CheckIfTicketExsistsAsync(SelectedTicket);
		if(_ticket)
		{
			await TicketService.UpdateTicketAsync(SelectedTicket);
			populateTicketsCollectionAsync();
		} else
		{
			await TicketService.SaveTicketAsync(SelectedTicket, SelectedBranch, SelectedStatus);
			SelectedTicket = new();
			populateTicketsCollectionAsync();
		}
	}

	[RelayCommand]
	private async void DeleteTicket()
	{
		await TicketService.DeleteTicketAsync(SelectedTicket);
		populateTicketsCollectionAsync();
	}

	[RelayCommand]
	private void ClearForm()
	{
		SelectedTicket = new();
        SelectedBranch = new();
        SelectedStatus = new();
    }

	public ViewAllViewModel()
	{
		Tickets = new();
		Branches = new();
		Statuses = new();
		SelectedTicket= new();
		SelectedBranch= new();
		SelectedStatus= new();
		populateTicketsCollectionAsync();
		populateBranchesCollectionAsync();
		populateStatusesCollectionAsync();
	}

	private async void populateTicketsCollectionAsync()
	{
		Tickets = (ObservableCollection<TicketModel>)await TicketService.GetAllTicketsAsync();

	}

	private async void populateBranchesCollectionAsync()
	{
        Branches = (ObservableCollection<BranchModel>)await BranchService.GetAllBranchesAsync();
	}

	private async void populateStatusesCollectionAsync()
	{
		Statuses = (ObservableCollection<StatusModel>)await StatusService.GetAllStatusAsync();
	}

}
