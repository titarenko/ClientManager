using System;

namespace BinaryStudio.ClientManager.WebUi.Models
{
    public class TagViewModel
    {
        public string Name { get; set; }

        public string CssClass
        {
            get
            {         
                return Name.Replace(".", "dot").Replace("+", "plus").ToLower();
            }
        }
    }
}