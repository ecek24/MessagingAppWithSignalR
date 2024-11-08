using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagingApp.Services.DTOs
{
    public class UserLoginDTO
    {
            [Required(ErrorMessage = "Password is required")]
            public string Password { get; set; }

            [Required(ErrorMessage = "Email is required")]
            public string Email { get; set; }
        }
    }

