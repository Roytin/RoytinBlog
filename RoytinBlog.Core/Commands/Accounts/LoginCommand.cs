﻿using iBoxDB.LocalServer;
using RoytinBlog.Core.Documents;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace RoytinBlog.Core.Commands.Accounts
{
    public class LoginCommand
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }

    public class LoginCommandResult : CommandResult
    {
        public LoginCommandResult()
            : base()
        { }

        public LoginCommandResult(string trrorMessage)
            : base(trrorMessage)
        { }

        public Author Author { get; set; }
    }

    public class LoginCommandInvoker : ICommandInvoker<LoginCommand, LoginCommandResult>
    {
        private readonly DB.AutoBox _db;

        public LoginCommandInvoker(DB.AutoBox db)
        {
            _db = db;
        }

        public LoginCommandResult Execute(LoginCommand loginCommand)
        {
            var hashedPassword = Hasher.GetMd5Hash(loginCommand.Password);

            var author = from u in _db.Select<Author>("from " + DBTableNames.Authors)
                         where u.Email == loginCommand.Email && u.HashedPassword == hashedPassword
                         select u;

            if (author.Count() > 0)
                return new LoginCommandResult() { Author = author.FirstOrDefault() };

            return new LoginCommandResult(trrorMessage: "用户名或密码不正确") { };
        }
    }
}