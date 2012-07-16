using System;

namespace BinaryStudio.ClientManager.WebUi.Controllers
{
    public class ModelIsNotValidException : ApplicationException
    {
        public ModelIsNotValidException() : base("Model is not valid.")
        {
        }
    }
}