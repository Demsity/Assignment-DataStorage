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

        /*

        public static async Task UpdateAsync(CustomerModel model)
        {
            var _customer = await _context.Customers.Include(x => x.Address).FirstOrDefaultAsync(x => x.Id == model.Id);
            if (_customer != null)
            {
                if (!string.IsNullOrEmpty(model.StreetName) || !string.IsNullOrEmpty(model.PostalCode) || !string.IsNullOrEmpty(model.City))
                {
                    var _address = await _context.Addresses.FirstOrDefaultAsync(x => x.StreetName == model.StreetName && x.PostalCode == model.PostalCode && x.City == model.City);
                    if (_address != null)
                        _customer.AddressId = _address.Id;
                    else
                    {
                        var address = new Address
                        {
                            StreetName = model.StreetName,
                            PostalCode = model.PostalCode,
                            City = model.City
                        };

                        _context.Add(address);
                        await _context.SaveChangesAsync();
                        _customer.AddressId = address.Id;
                    }

                }

                if (!string.IsNullOrEmpty(model.FirstName))
                    _customer.FirstName = model.FirstName;

                if (!string.IsNullOrEmpty(model.LastName))
                    _customer.LastName = model.LastName;

                if (!string.IsNullOrEmpty(model.Email))
                    _customer.Email = model.Email;

                if (!string.IsNullOrEmpty(model.PhoneNumber))
                    _customer.PhoneNumber = model.PhoneNumber;

                _context.Update(_customer);
                await _context.SaveChangesAsync();

            }
        }
        */
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
