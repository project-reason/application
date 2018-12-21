using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Main.Site.Pages.Person
{
    public class RegisterModel : PageModel
    {

        [BindProperty]
        public InputModel InputM { get; set; }

        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            var test = InputM.TipoPessoa;

            return Page();
        }

        public class InputModel
        {
            public string TipoPessoa { get; set; }

            public string Nome { get; set; }

            public string RazaoSocial { get; set; }
        }
    }
}