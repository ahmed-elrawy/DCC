using DCC.API.Helper;
using DCC.API.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DCC.API.Repository
{
    public interface IDccRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAll();
        Task<IEnumerable<User>> GetUsers();
        Task<PageList<User>> GetUsersPaging(UserParams userPatams);

        Task<User> GetUser(string id);
        Task<Photo> GetPhoto(int id);
        Task<Photo> GetMainPhotoForUser(string userId);
        Task<Like> GetLike(string userId, string recipientId);
        Task<IEnumerable<string>> GetSpec();
        Task<Message> GetMessage(int id);
        Task<PageList<Message>> GetMessageForUser(MessageParams messageParams);
        Task<IEnumerable<Message>> GetMessageThread(string userId, string recipientId);


    }
}
