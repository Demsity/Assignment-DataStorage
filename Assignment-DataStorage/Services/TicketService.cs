using Assignment_DataStorage.Contexts;
using Assignment_DataStorage.Models;
using Assignment_DataStorage.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_DataStorage.Services
{
    internal class TicketService
    {
        private static DataContext _dataContext = new();

        public static async Task SaveTicketAsync(TicketModel model, BranchModel branch, StatusModel status)
        {
            var _ticket = new TicketEntity
            {
                Description= model.Description,
                TicketCreatedAt= DateTime.Now,
              
            };

            var _customerEntity = await _dataContext.Customers.FirstOrDefaultAsync(x => x.Email == model.Email);
            if (_customerEntity != null)
                _ticket.CustomerId = _customerEntity.Id;
            else
                _ticket.Customer = new CustomerEntity
                { 
                    FirstName = model.FirstName,
                    LastName= model.LastName,
                    Email= model.Email,
                    PhoneNumber= model.PhoneNumber,
                    CustomerCreatedAt= DateTime.Now
                };

           
            var _branch = await _dataContext.Branches.FirstOrDefaultAsync(x => x.Id== branch.Id);
            if (_branch != null)
            {
                _ticket.BranchId = _branch.Id;
                _ticket.Branch = _branch;
            }
            

            var _status = await _dataContext.Status.FirstOrDefaultAsync(x => x.Id == status.Id);
            if (_status != null)
            {
                _ticket.Status = _status;
                _ticket.StatusId = _status.Id;
            }

            _dataContext.Tickets.Add(_ticket);
            await _dataContext.SaveChangesAsync();
        }

        
        public static async Task<IEnumerable<TicketModel>> GetAllTicketsAsync()
        {
            var _tickets = new ObservableCollection<TicketModel>();

            

            foreach (var _ticket in await _dataContext.Tickets.Include(x => x.Customer).Include(x => x.Comments).Include(x => x.Branch).Include(x => x.Status).ToListAsync())
            {
                _tickets.Add(new TicketModel
                {
                    Id = _ticket.Id,
                    Description = _ticket.Description,
                    FirstName = _ticket.Customer.FirstName,
                    LastName = _ticket.Customer.LastName,
                    Email = _ticket.Customer.Email,
                    PhoneNumber = _ticket.Customer.PhoneNumber,
                    BranchId = _ticket.Branch.Id,
                    Branch = _ticket.Branch.Name,
                    StatusId = _ticket.Status.Id,
                    Status = _ticket.Status.Status,
                    TicketCreatedAt = _ticket.TicketCreatedAt
                });

            }
            foreach (var _ticketModel in _tickets)
            {
                _ticketModel.Comments = new ObservableCollection<CommentModel>();
                foreach (var _comment in await _dataContext.Comments.Where(x => x.TicketId == _ticketModel.Id).ToListAsync())
                    {
                        
                        _ticketModel.Comments.Add(new CommentModel
                        {
                            Id = _comment.Id,
                            TicketId = (int)_ticketModel.Id,
                            Comment = _comment.Comment,
                            CreatedAt = _comment.CommentCreatedAt
                        });
                    }
            }
            return _tickets;
        }


        

        public static async Task<TicketModel> GetTicketAsync(TicketModel model)
        {
            var _ticket = await _dataContext.Tickets.Include(x => x.Customer).Include(x => x.Branch).Include(x => x.Status).FirstOrDefaultAsync(x => x.Id == model.Id);
            if (_ticket != null)
                return new TicketModel
                {
                    Id = _ticket.Id,
                    Description = _ticket.Description,
                    FirstName = _ticket.Customer.FirstName,
                    LastName = _ticket.Customer.LastName,
                    Email = _ticket.Customer.Email,
                    PhoneNumber = _ticket.Customer.PhoneNumber,
                    Branch = _ticket.Branch.Name,
                    Status = _ticket.Status.Status,
                    TicketCreatedAt = _ticket.TicketCreatedAt
                };

            else
                return null!;
        }

        public static async Task<Boolean> CheckIfTicketExsistsAsync(TicketModel model)
        {
            try
            {
                var _ticket = await _dataContext.Tickets.FirstOrDefaultAsync(x => x.Id == model.Id);
                if (_ticket != null)
                    return true;

                else
                    return false;
            } catch
            {
                return false;
            }

        }

        

        public static async Task UpdateTicketAsync(TicketModel model, BranchModel branch, StatusModel status)
        {
            var _ticket = await _dataContext.Tickets.Include(x => x.Customer).FirstOrDefaultAsync(x => x.Id == model.Id);
            if (_ticket != null)
            {
                // Update Customer
                if (!string.IsNullOrEmpty(model.FirstName) || !string.IsNullOrEmpty(model.LastName) || !string.IsNullOrEmpty(model.Email) || !string.IsNullOrEmpty(model.PhoneNumber))
                {
                    var _customer = await _dataContext.Customers.FirstOrDefaultAsync(x =>  x.Email == model.Email);
                    if (_customer != null)
                    {
                        _customer.FirstName= model.FirstName;
                        _customer.LastName= model.LastName;
                        _customer.Email= model.Email;
                        _customer.PhoneNumber= model.PhoneNumber;

                        _dataContext.Customers.Update(_customer);
                        await _dataContext.SaveChangesAsync();
                    }
                }

                // Update Branch and status
                if(_ticket.Branch.Id != branch.Id)
                {
                    var _branch = await _dataContext.Branches.FirstOrDefaultAsync(x => x.Id == branch.Id);
                    if (_branch != null)
                        _ticket.Branch = _branch;
                }

                if (_ticket.Status.Id != status.Id)
                {
                    var _status = await _dataContext.Status.FirstOrDefaultAsync(x => x.Id == status.Id);
                    if (_status != null)
                        _ticket.Status = _status;
                }

                // Update Description
                if (!string.IsNullOrEmpty(model.Description))
                    _ticket.Description = model.Description;



                _dataContext.Tickets.Update(_ticket);
                await _dataContext.SaveChangesAsync();

            }
        }
        
        public static async Task DeleteTicketAsync(TicketModel model)
        {
            var _ticket = await _dataContext.Tickets.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (_ticket != null)
            {
                _dataContext.Tickets.Remove(_ticket);
                await _dataContext.SaveChangesAsync();
            }
        }
        
    }
}
