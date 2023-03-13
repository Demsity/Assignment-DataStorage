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

class CommentService
{

    private static DataContext _dataContext = new();

    public static async Task SaveCommentAsync(TicketModel model, CommentModel commentModel)
    {
        if (model != null && commentModel != null)
        {
            var _comment = new CommentEntity
            {
                TicketId = (int)model.Id!,
                Comment = commentModel.Comment,
                CommentCreatedAt = DateTime.Now,

            };

            _dataContext.Comments.Add(_comment);
            await _dataContext.SaveChangesAsync();
        }

    }

    public static async Task DeleteCommentAsync(CommentModel model)
    {
        var _comment = await _dataContext.Comments.FirstOrDefaultAsync(x => x.Id == model.Id);
        if (_comment != null)
        {
            _dataContext.Comments.Remove(_comment);
            await _dataContext.SaveChangesAsync();
        }
    }
}
