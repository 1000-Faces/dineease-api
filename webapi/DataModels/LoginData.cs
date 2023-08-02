#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace webapi.DataModels;

public partial class LoginData
{
    public string Email { get; set; }

    public string Password { get; set; }
}