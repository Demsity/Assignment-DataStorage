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
using System.Windows;

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
	private ObservableCollection<CommentModel> comments = null!;

	[ObservableProperty]
	private CommentModel newComment = null!;

	[ObservableProperty]
	private TicketModel selectedTicket = null!;

	[ObservableProperty]
	private BranchModel selectedBranch = null!;

	[ObservableProperty]
	private StatusModel selectedStatus = null!;

	[ObservableProperty]
	private CommentModel selectedComment = null!;

    partial void OnSelectedTicketChanging(TicketModel value)
    {
		if (value != null)
		{
            if (value.BranchId > 0 && value.StatusId > 0)
            {
                SelectedBranch = Branches.FirstOrDefault(x => x.Id == value.BranchId)!;
                SelectedStatus = Statuses.FirstOrDefault(x => x.Id == value.StatusId)!;
            }


        } else
		{
			SelectedBranch = new();
			SelectedStatus = new();
			Comments = new();
		}

    }

    partial void OnSelectedTicketChanged(TicketModel value)
    {
		populateCommentCollection();
    }

    partial void OnSelectedBranchChanging(BranchModel value)
    {
		if(SelectedTicket != null)
		{
            if (SelectedTicket.BranchId > 0)
            {
                var _branch = Branches.FirstOrDefault(x => x.Id == SelectedTicket.BranchId);
                if (SelectedTicket.BranchId != _branch.Id)
                {
                    SelectedTicket.BranchId = _branch.Id;
                    SelectedTicket.Branch = _branch.Name;
                }
            }
        }
    }

    [RelayCommand]
	private async void SaveTicket()
	{
		var _ticket = await TicketService.CheckIfTicketExsistsAsync(SelectedTicket);
		if(_ticket)
		{
			await TicketService.UpdateTicketAsync(SelectedTicket, SelectedBranch, SelectedStatus);
			MessageBox.Show("Ticket has been updated");
			populateTicketsCollectionAsync();
		} else
		{
			await TicketService.SaveTicketAsync(SelectedTicket, SelectedBranch, SelectedStatus);
            MessageBox.Show("Ticket has been saved");
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
		SelectedBranch = Branches.First();
		SelectedStatus = Statuses.First();
    }

	[RelayCommand]
	private async void AddCommentToDB()
	{
		if (SelectedTicket != null && NewComment.Comment != null)
		{
            await CommentService.SaveCommentAsync(SelectedTicket, NewComment);
			NewComment = new();
			populateTicketsCollectionAsync();
        }else
		{
			MessageBox.Show("Please Select Ticket and Add a new Comment");
		}

	}

	[RelayCommand]
	private async void DeleteComment()
	{
		await CommentService.DeleteCommentAsync(SelectedComment);
		populateTicketsCollectionAsync();
	}

	public ViewAllViewModel()
	{
		Tickets = new();
		Branches = new();
		Statuses = new();
		SelectedTicket= new();
		SelectedBranch= new();
		SelectedStatus= new();
		NewComment= new();
		SelectedComment= new();
		populateTicketsCollectionAsync();
		populateBranchesCollectionAsync();
		populateStatusesCollectionAsync();
	}

	private async void populateTicketsCollectionAsync()
	{
		TicketModel _tempTicket = SelectedTicket;
		Tickets = (ObservableCollection<TicketModel>)await TicketService.GetAllTicketsAsync();
		if(_tempTicket.Id != null)
		{
            SelectedTicket = (TicketModel)Tickets.FirstOrDefault(x => x.Id == _tempTicket.Id)!;
        } else
		{
			SelectedTicket = (TicketModel)Tickets.First();
        }
		

	}

	private async void populateBranchesCollectionAsync()
	{
        Branches = (ObservableCollection<BranchModel>)await BranchService.GetAllBranchesAsync();
	}

	private async void populateStatusesCollectionAsync()
	{
		Statuses = (ObservableCollection<StatusModel>)await StatusService.GetAllStatusAsync();
	}

	private void populateCommentCollection()
	{
        if (SelectedTicket != null && SelectedTicket.Comments != null && Comments != null)
        {
            foreach (var comment in SelectedTicket.Comments!)
            {
                Comments.Add(comment);
            }
        }
    }

}
