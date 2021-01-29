﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAPI.DTO
{
    public class RefreshRequest
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Token { get; set; }
    }
}
