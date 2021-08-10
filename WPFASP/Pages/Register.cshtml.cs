using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;
using DI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WPFASP.Pages
{
    public class RegisterModel : PageModel
    {
        /// <summary>
        /// Email of the user.
        /// </summary>
        private string _email;

        /// <summary>
        /// Data for users.
        /// </summary>
        private readonly IJsonUserData _userData = ApplicationViewModel.Container.GetInstance<IJsonUserData>();

        /// <summary>
        /// Name of the user.
        /// </summary>
        [BindProperty]
        public string Name { get; set; }

        /// <summary>
        /// Email of the user.
        /// </summary>
        [BindProperty]
        public string Email
        {
            get => _email;
            set => _email = value + @"@gmail.com";
        }

        public void OnGet()
        {
        }

        /// <summary>
        /// Register the user.
        /// </summary>
        public void OnPost()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Name))
            {
                Response.ContentType = "text/html";
                Response.StatusCode = 400;

                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                Response.WriteAsync(
                    "<div align=\"center\"> <p>Введены некорректные данные</p><a href=\"/Register\">Назад<a/><div/>", Encoding.GetEncoding(1251));

                return;
            }

            IUser user = ApplicationViewModel.Container.GetInstance<IUser>();


            user.Id = _userData.GetAllUsers().Count;
            user.Email = Email;
            user.UserName = Name;

            try
            {
                _userData.AddUser(user);
            }
            catch (ArgumentException)
            {
                Response.ContentType = "text/html";
                Response.StatusCode = 400;

                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                Response.WriteAsync(
                    "<div align=\"center\"> <p>Пользователь с таким email уже зарегистрирован</p><a href=\"/Register\">Назад<a/><div/>", Encoding.GetEncoding(1251));

                return;
            }

            Response.StatusCode = 200;
            Response.Redirect("/Index");
        }
    }
}
