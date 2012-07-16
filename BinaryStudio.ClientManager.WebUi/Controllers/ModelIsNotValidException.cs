using System;

namespace BinaryStudio.ClientManager.WebUi.Models
{
    public class ModelIsNotValidException : ApplicationException
    {
        public ModelIsNotValidException() : base("Model is not valid.")
        {
        }
    }
}