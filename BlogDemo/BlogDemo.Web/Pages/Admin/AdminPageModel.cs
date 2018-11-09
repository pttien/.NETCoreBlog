using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace BlogDemo.Web.Pages.Admin
{
    public class AdminPageModel : PageModel
    {
        public bool IsAdmin { get; set; }

        [TempData]
        public string Message { get; set; }
        public bool ShowMessage => !string.IsNullOrEmpty(Message);

        [TempData]
        public string Error { get; set; }
        public bool ShowError => !string.IsNullOrEmpty(Error);

        public void Clear()
        {
            Message = "";
            Error = "";
        }
    }
}