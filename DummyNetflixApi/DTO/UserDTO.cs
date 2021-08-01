using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DummyNetflixApi.DTO
{
    public class UserCreateModel
    {
        [Required]
        [StringLength(30)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 10)]
        public string Password { get; set; }

    }

    public class LoginCreateModel
    {
        [Required]
        [StringLength(30)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 10)]
        public string Password { get; set; }

    }
}
