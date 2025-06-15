// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Identity.Client.UI;
using StaffManagement.Models;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;


namespace StaffManagement.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterConfirmationModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender<ApplicationUser> _sender;
        public string? Email { get; set; }

        public bool DisplayConfirmAccountLink { get; set; }

        public string? EmailConfirmationUrl { get; set; }

        public RegisterConfirmationModel(UserManager<ApplicationUser> userManager, IEmailSender<ApplicationUser> sender)
        {
            _userManager = userManager;
            _sender = sender;
        }

        public  async Task<IActionResult> OnGetAsync(string email, string? returnUrl = null)
        {
            if (email == null)
            {
                return RedirectToPage("/Index");
            }

            returnUrl ??= Url.Content("~/");

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"Unable to load user with email '{email}'.");
            }

            Email = email;

            // Nếu email sender là "noop", hiển thị liên kết xác nhận tài khoản trong trang
            bool displayConfirmAccountLink = true;

            if (displayConfirmAccountLink)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var encodedCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                var emailConfirmationUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity", userId = userId, code = encodedCode, returnUrl = returnUrl },
                    protocol: Request.Scheme);

                // Hiển thị liên kết email trong trang
                // Có thể thêm logic hiển thị liên kết cho người dùng tại đây
            }

            return Page();
        }
    }
}


