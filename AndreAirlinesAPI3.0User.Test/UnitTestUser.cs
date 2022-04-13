using AndreAirlinesAPI3._0Models;
using AndreAirlinesAPI3._0User.Service;
using AndreAirlinesAPI3._0User.Utils;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace AndreAirlinesAPI3._0User.Test
{
    public class UnitTestUser
    {
        //private List<User> _allUsers;

        private UserService InitializeDataBase()
        {
            var settings = new AndreAirlinesDatabaseUserSettings();
            UserService userService = new(settings);
            return userService;
            /*
            _allUsers = new List<User>();
            _allUsers.Add(new User() { Id = "1", Cpf = "426.441.220-07", Name = "Teste1", Telephone = "99999", DateBirth = DateTime.Now.Date, Email = "a@a.com", Address = new Address { District = "Sé", City = "São Paulo", Country = "Brasil", ZipCode = "01001000", Street = "Praça da Sé", State = "São Paulo", Number = 1000, Complement = "", Continent = "South America", ErrorCode = "null" }, LoginUser = "m1m1", ErrorCode = "", Login = "m1m1", Password = "m1m1", Role = "adm", Function = new Function { Id = 1, Description = "adm", Access = new List<Access> { new Access { Id = 1, Description = "all" } } } });
            _allUsers.Add(new User() { Id = "6252c3f0ea6a1df3a8a2abcc", Cpf = "237.946.190-21", Name = "Teste2", Telephone = "88888", DateBirth = DateTime.Now.Date, Email = "b@b.com", Address = new Address { District = "Vila Guilherme", City = "São Paulo", Country = "Brasil", ZipCode = "02075000", Street = "Rua José Gomide", State = "São Paulo", Number = 2000, Complement = "andar superior", Continent = "South America", ErrorCode = "null" }, LoginUser = "m1m1", ErrorCode = "", Login = "m1m1", Password = "m1m1", Role = "adm", Function = new Function { Id = 1, Description = "adm", Access = new List<Access> { new Access { Id = 1, Description = "all" } } } });
            _allUsers.Add(new User() { Id = "3", Cpf = "028.864.570-78", Name = "Teste3", Telephone = "77777", DateBirth = DateTime.Now.Date, Email = "c@c.com", Address = new Address { District = "Vila Maria Alta", City = "São Paulo", Country = "Brasil", ZipCode = "02125000", Street = "Rua Mere Amedea", State = "São Paulo", Number = 3000, Complement = "", Continent = "South America", ErrorCode = "null" }, LoginUser = "m1m1", ErrorCode = "", Login = "m1m1", Password = "m1m1", Role = "adm", Function = new Function { Id = 1, Description = "adm", Access = new List<Access> { new Access { Id = 1, Description = "all" } } } });
            */
        }

        [Fact]
        public void GetAll()
        {
            var userService = InitializeDataBase();
            var users = userService.Get();

            var status = users.Count > 0;
            Assert.Equal(true, status);
        }

        [Fact]
        public void GetOne()
        {
            var userService = InitializeDataBase();
            var user = userService.Get("6252c3f0ea6a1df3a8a2abcc");
            if (user == null) user = new User();
            Assert.Equal("6252c3f0ea6a1df3a8a2abcc", user.Id);
        }

        [Fact]
        public void GetUserLogin()
        {
            User user = new User { Login = "m1m1", Password = "m1m1" };

            var userService = InitializeDataBase();
            var userLogin = userService.GetUserLogin(user);
            Assert.Equal("m1m1", userLogin.Login);
        }

        [Fact]
        public void GetLoginUser()
        {
            var userService = InitializeDataBase();
            var user = userService.GetLoginUser("m2m2");
            if (user == null) user = new User();
            Assert.Equal("m2m2", user.Login);
        }

        [Fact]
        public void GetUserCpf()
        {
            var userService = InitializeDataBase();
            var user = userService.GetCpf("106.241.640-63");
            if (user == null) user = new User();
            Assert.Equal("106.241.640-63", user.Cpf);
        }

        [Fact]
        public async void Create()
        {
            User newUser = new User() { Cpf = "028.864.570-78", Name = "Teste3", Telephone = "77777", DateBirth = DateTime.Now.Date, Email = "c@c.com", Address = new Address { District = "Vila Maria Alta", City = "São Paulo", Country = "Brasil", ZipCode = "02125000", Street = "Rua Mere Amedea", State = "São Paulo", Number = 3000, Complement = "", Continent = "South America", ErrorCode = "null" }, LoginUser = "m1m1", ErrorCode = "", Login = "m3m3", Password = "m3m3", Role = "adm", Function = new Function { Id = 1, Description = "adm", Access = new List<Access> { new Access { Id = 1, Description = "all" } } } };
            
            var userService = InitializeDataBase();
            var returnMsg = await userService.Create(newUser);
            Assert.Equal("Teste3", returnMsg.Name);
        }

        [Fact]
        public async void Update()
        {
            User newUser = new User() { Id = "6255d60e533cf26a4a28ace7", Cpf = "028.864.570-78", Name = "Teste3", Telephone = "0000000000", DateBirth = DateTime.Now.Date, Email = "c@c.com", Address = new Address { District = "Vila Maria Alta", City = "São Paulo", Country = "Brasil", ZipCode = "02125000", Street = "Rua Mere Amedea", State = "São Paulo", Number = 3000, Complement = "", Continent = "South America", ErrorCode = "null" }, LoginUser = "m1m1", ErrorCode = "", Login = "m3m3", Password = "m3m3", Role = "adm", Function = new Function { Id = 1, Description = "adm", Access = new List<Access> { new Access { Id = 1, Description = "all" } } } };

            var userService = InitializeDataBase();
            var returnMsg = await userService.Update("6255d60e533cf26a4a28ace7", newUser, newUser);
            Assert.Equal("ok", returnMsg);
        }

        [Fact]
        public async void Remove()
        {
            var userService = InitializeDataBase();
            var returnMsg = await userService.Remove("6255d60e533cf26a4a28ace7", new User() { Id = "6255d60e533cf26a4a28ace7" }, null);
            Assert.Equal("ok", returnMsg);
        }
    }
}
