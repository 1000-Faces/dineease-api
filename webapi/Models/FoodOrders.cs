﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace webapi.Models;

[PrimaryKey("FoodId", "OrderId")]
public partial class FoodOrders
{
    [Key]
    public Guid FoodId { get; set; }

    [Key]
    public Guid OrderId { get; set; }
}