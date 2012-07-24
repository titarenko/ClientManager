﻿using System.Collections.Generic;
using BinaryStudio.ClientManager.DomainModel.Entities;

namespace BinaryStudio.ClientManager.WebUi.Models
{
    public class AdminViewModel
    {
        public IEnumerable<CategoryViewModel> Categories { get; set; }
        public IList<Person> Employees { get; set; }
        public IList<Tag> Tags { get; set; }
    }
}