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

            if (model.Comment != null)
            {
                _ticket.Comment = new CommentEntity
                {
                    TicketId = _ticket.Id,
                    Comment = model.Comment,
                    CommentCreatedAt = DateTime.Now
                };
            }

           
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

            foreach (var _ticket in await _dataContext.Tickets.Include(x => x.Customer).Include(x => x.Comment).Include(x => x.Branch).Include(x => x.Status).ToListAsync())
                _tickets.Add(new TicketModel
                {
                    Id = _ticket.Id,
                    Description= _ticket.Description,
                    FirstName = _ticket.Customer.FirstName,
                    LastName = _ticket.Customer.LastName,
                    Email = _ticket.Customer.Email,
                    PhoneNumber = _ticket.Customer.PhoneNumber,
                    BranchId = _ticket.Branch.Id,
                    Branch = _ticket.Branch.Name,
                    StatusId = _ticket.Status.Id,
                    Status = _ticket.Status.Status,
                    Comment = _ticket.Comment?.Comment,
                    CommentCreatedAt = _ticket.Comment?.CommentCreatedAt,
                    TicketCreatedAt = _ticket.TicketCreatedAt
                });

            return _tickets;
        }


        

        public static async Task<TicketModel> GetTicketAsync(TicketModel model)
        {
            var _ticket = await _dataContext.Tickets.Include(x => x.Customer).Include(x => x.Comment).Include(x => x.Branch).Include(x => x.Status).FirstOrDefaultAsync(x => x.Id == model.Id);
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
                    Comment = _ticket.Comment!.Comment,
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

        

        public static async Task UpdateTicketAsync(TicketModel model)
        {
            var _ticket = await _dataContext.Tickets.Include(x => x.Customer).Include(x => x.Comment).FirstOrDefaultAsync(x => x.Id == model.Id);
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
                // Update Comment
                if (!string.IsNullOrEmpty(model.Comment))
                {
                    var _comment = await _dataContext.Comments.FirstOrDefaultAsync(x => x.TicketId == model.Id);
                    if (_comment != null)
                    {
                        if (_ticket.Comment != null)
                        {
                            _ticket.Comment.Comment = model.Comment;
                            _ticket.Comment.CommentCreatedAt = DateTime.Now;

                            _dataContext.Comments.Update(_comment);
                            await _dataContext.SaveChangesAsync();

                        } else
                        {
                            var comment = new CommentEntity
                            {
                                TicketId = (int)model.Id!,
                                Comment = model.Comment,
                                CommentCreatedAt = DateTime.Now,

                            };

                            _dataContext.Comments.Add(comment);
                            await _dataContext.SaveChangesAsync();
                        }
                    } else
                    {
                        var comment = new CommentEntity
                        {
                            TicketId = (int)model.Id!,
                            Comment = model.Comment,
                            CommentCreatedAt = DateTime.Now,

                        };

                        _dataContext.Comments.Add(comment);
                        await _dataContext.SaveChangesAsync();
                    }
                }

                // Update Branch and status
                _ticket.BranchId = model.BranchId;
                _ticket.StatusId = model.StatusId;

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
