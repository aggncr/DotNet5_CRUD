﻿using System.Collections.Generic;

namespace DotNet5_CRUD.Models.DataAccess.Entity
{
    public class Author
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public ICollection<Book> Books { get; set; }
    }
}
