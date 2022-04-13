using AndreAirlinesAPI3._0Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AndreAirlinesAPI3._0User.Service
{
    public interface IUserService
    {
        List<User> Get();
        User Get(string id);
        User GetUserLogin(User userIn);
        User GetLoginUser(string loginUser);
        User GetCpf(string cpf);
        Task<User> Create(User user);
        Task<string> Update(string id, User userIn, User user);
        Task<string> Remove(string id, User userIn, User user);
    }
}
