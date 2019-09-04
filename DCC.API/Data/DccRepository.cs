using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DCC.API.Helper;
using DCC.API.Model;
using DCC.API.Repository;
using Microsoft.EntityFrameworkCore;
using MoreLinq;

namespace DCC.API.Data
{
    public class DccRepository : IDccRepository
    {
        #region  private Members
        private readonly DataContext _context;
        #endregion

        #region ctor
        public DccRepository(DataContext context)
        {
            _context = context;
        }
        #endregion

        #region  Method Implemntaions
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }


        public async Task<User> GetUser(string id)
        {
            var user = await _context.Users.Include(p => p.Photos)
            .FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = await _context.Users.Include(p => p.Photos).ToListAsync();
            return users;
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Photo> GetPhoto(int id)
        {
            var photo = await _context.Photos.FirstOrDefaultAsync(p => p.Id == id);
            return photo;
        }

        public async Task<Photo> GetMainPhotoForUser(string userId)
        {
            return await _context.Photos
            .Where(p => p.UserId == userId)
            .FirstOrDefaultAsync(p => p.IsMain);
        }
        public async Task<PageList<User>> GetUsersPaging(UserParams userParams)
        {
            var users = _context.Users.Include(p => p.Photos)
            .OrderByDescending(u => u.LastActive).AsQueryable();

            users = users.Where(u => u.Id != userParams.UserId);
            users = users.Where(u => u.TypeOfUser == userParams.typeOfUser);
            if (!string.IsNullOrEmpty(userParams.Specialization))
            {
                users = users.Where(u => u.Specialization == userParams.Specialization);
            }
            foreach (var x in users)
            {
                x.LikerCount = await this.GetLikerCount(x.Id);
            }
            if (userParams.Likers)
            {
                var userLikers = await GetUserLikes(userParams.UserId, userParams.Likers);
                users = users.Where(u => userLikers.Contains(u.Id));
            }
            if (userParams.Likees)
            {
                var userLikees = await GetUserLikes(userParams.UserId, userParams.Likers);
                users = users.Where(u => userLikees.Contains(u.Id));
            }
            if (!string.IsNullOrEmpty(userParams.OrderBy))
            {
                switch (userParams.OrderBy)
                {
                    case "created":
                        users = users.OrderByDescending(u => u.Created);
                        break;
                    case "rating":
                        users = users.OrderByDescending(u => u.LikerCount);
                        break;
                    case "lowrating":
                        users = users.OrderBy(u => u.LikerCount);
                        break;

                    default:
                        users = users.OrderByDescending(u => u.LastActive);
                        break;
                }
            }
            return await PageList<User>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
        }
        private async Task<IEnumerable<string>> GetUserLikes(string id, bool likers)
        {
            var user = await _context.Users
            .Include(x => x.Likers)
            .Include(x => x.Likees)
            .FirstOrDefaultAsync(u => u.Id == id);
            if (likers)
            {
                return user.Likers.Where(u => u.LikeeId == id).Select(i => i.LikerId);
            }
            else
            {
                return user.Likees.Where(u => u.LikerId == id).Select(i => i.LikeeId);
            }
        }
        public async Task<Like> GetLike(string userId, string recipientId)
        {
            return await _context.Likes
            .FirstOrDefaultAsync(u => u.LikerId == userId && u.LikeeId == recipientId);
        }
        public async Task<int> GetLikerCount(string userId)
        {
            var likerCount = await _context.Users
            .Include(u => u.Likers)
            .Include(u => u.Likees)
            .FirstOrDefaultAsync(u => u.Id == userId);
            var count = likerCount.Likers.Where(u => u.LikeeId == userId).Select(i => i.LikerId).Count();
            // var user = _context.users.Where(u => u.Id == userId);

            return count;
        }


        public async Task<IEnumerable<string>> GetSpec()
        {
            IEnumerable<string> spec = await _context.Users.Select(s => s.Specialization).Where(t => t != null).Distinct().ToListAsync();
            return spec;
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<PageList<Message>> GetMessageForUser(MessageParams messageParams)
        {
            var messages = _context.Messages.Include(u => u.Sender)
            .ThenInclude(p => p.Photos)
            .Include(u => u.Recipient)
            .ThenInclude(p => p.Photos)
            .AsQueryable();
            switch (messageParams.MessageContainer)
            {
                case "Inbox":
                    messages = messages.GroupBy(x => x.SenderId).Select(f =>
                    f.OrderByDescending(y => y.MessageSent).First()).Where(u => u.RecipientId == messageParams.UserId && u.RecipientDeleted == false);
                    break;
                case "Outbox":
                    messages = messages.Where(u => u.SenderId == messageParams.UserId == u.SenderDeleted == false);
                    break;
                default:
                    //                messages = messages.GroupBy(u => u.SenderId).Select(y => y.First()).Where(
                    //    u => u.RecipientId == messageParams.UserId
                    //&& u.RecipientDeleted == false && u.isRead == false
                    //);

                    messages = messages.GroupBy(x => x.SenderId).Select(f =>
                    f.OrderByDescending(y => y.MessageSent).First()).Where(
                        u => u.RecipientId == messageParams.UserId
                     && u.RecipientDeleted == false && u.isRead == false);

                     break;
            }
            messages = messages.OrderByDescending(d => d.MessageSent);
            return await PageList<Message>.CreateAsync(messages,
             messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<IEnumerable<Message>> GetMessageThread(string userId, string recipientId)
        {
            var messages = await _context.Messages.Include(u => u.Sender).ThenInclude(p => p.Photos)
            .Include(u => u.Recipient).ThenInclude(p => p.Photos)
            .Where(m => m.RecipientId == userId && m.RecipientDeleted == false && m.SenderId == recipientId
            || m.RecipientId == recipientId && m.SenderId == userId && m.SenderDeleted == false)
            .OrderByDescending(m => m.MessageSent)
            .ToListAsync();
            return messages;
        }
        #endregion
    }
}