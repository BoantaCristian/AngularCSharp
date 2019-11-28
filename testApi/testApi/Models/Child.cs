﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace testApi.Models
{
    public class Child
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual Parent Parent { get; set; }
    }
}
