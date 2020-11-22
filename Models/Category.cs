﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogMvc.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<Post> Posts { get; set; }
    }
}
