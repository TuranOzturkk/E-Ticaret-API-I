using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Exceptions
{
    public class UserCreadFailedException : Exception
    {
        public UserCreadFailedException(): base("Kullanıcı Oluşturulurken Beklenmeyen Bir Hata Oluştu!")
        {
        }

        public UserCreadFailedException(string? message) : base(message)
        {
        }

        public UserCreadFailedException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
