using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.Shared.DataConfig
{
    public enum StatusCodes
    {
        FAILURE_CODE = 0,
        SUCCESS_CODE = 1,
        ERROR_EXCEPTION_CAUGHT_CODE = 2,
        ERROR_INVALID_AUTH = 3,
        ERROR_INVALID_METHOD = 4,
        INVALID_PASSWORD_EMAIL = 5,
        EMAIL_ALREADY = 6,
        FIELD_REQUIRED = 7,
        RECORD_NOTFOUND = 8,
        Success_WithOutEmail = 9,
        Already_Exists = 10,
        Name_AlreadyExists = 11,
        Customer_Profile_Not_Found = 12,
        Exceed_Credit_Limit = 13,
        Availed_Supervisor = 14,
        Availed_Manager = 15,
        Less_Credit_Allowed = 16,
  
    }
    public class ResponseBack<T>
    {
        private int status_value = 0;
        private string message_value = "";
        private T data_value = default(T);

        public int Status
        {
            get { return status_value; }
            set { status_value = value; }
        }
        public string Message
        {
            get { return message_value; }
            set { message_value = value; }
        }
        public T Data
        {
            get { return data_value; }
            set { data_value = value; }
        }
    }
}
