using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFlowHR.Domain.Utilities.Interfaces;

namespace WorkFlowHR.Domain.Utilities.Concretes
{
    public class Result:IResult
    {
        public bool IsSuccess { get; }

        public string Messages { get; }


        /// <summary>
        /// Varsayılan işlem sonucu oluşturur.
        /// </summary>
        public Result()
        {
            IsSuccess = false;
            Messages = string.Empty;
        }

        /// <summary>
        /// İşlem sonucunu başarılı olup olmadığını belirterek yeni bir örnek oluşturur.
        /// </summary>
        /// <param name="isSuccess">İşlemin başarılı olup olmadığını belirtir.</param>
        public Result(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }

        /// <summary>
        /// İşlem sonucunu ve mesajları belirterek yeni bir örnek oluşturur.
        /// </summary>
        /// <param name="isSuccess">İşlemin başarılı olup olmadığını belirtir.</param>
        /// <param name="messages">İşlemle ilgili mesajlar.</param>
        public Result(bool isSuccess, string messages) : this(isSuccess)
        {
            Messages = messages;
        }
    }
}
