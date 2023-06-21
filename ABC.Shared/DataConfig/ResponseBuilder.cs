using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABC.Shared.DataConfig
{
    public class ResponseBuilder
    {
        private static bool IsStackTraceEnabled = true;
        public static ResponseBack<T> BuildWSResponse<T>()
        {
            return new ResponseBack<T> { Status = (int)StatusCodes.SUCCESS_CODE, Message = "API Service Call Success" };
        }

        public static ResponseBack<T> BuildInvalidMethodWSResponse<T>()
        {
            return new ResponseBack<T> { Status = (int)StatusCodes.ERROR_INVALID_METHOD, Message = "Invalid method" };
        }
        public static void SetWSResponse<T>(ResponseBack<T> response, StatusCodes code, Exception exception, T resultObject)
        {
            if (response != null)
            {
                response.Status = (int)code;
                response.Data = resultObject;
                switch (code)
                {
                    case StatusCodes.SUCCESS_CODE:
                        response.Message = "Success.";
                        break;
                    case StatusCodes.FAILURE_CODE:
                        response.Message = string.Format("{0}, {1}",
                            string.Format("Failure: [{0}]", exception.Message),
                            (IsStackTraceEnabled && !string.IsNullOrEmpty(exception.Message) ? string.Format("{0}Stacktrace: [{1}]", Environment.NewLine, exception.StackTrace) : ""));
                        break;
                    case StatusCodes.ERROR_EXCEPTION_CAUGHT_CODE:
                        response.Message = string.Format("{0}, {1}",
                            string.Format("System Error: [{0}]", exception.Message),
                            (IsStackTraceEnabled && !string.IsNullOrEmpty(exception.Message) ? string.Format("{0}Stacktrace: [{1}]", Environment.NewLine, exception.StackTrace) : ""));
                        break;
                    case StatusCodes.INVALID_PASSWORD_EMAIL:
                        response.Message = "Invalid Email or Password.";
                        break;
                    case StatusCodes.EMAIL_ALREADY:
                        response.Message = "Email Already Exist.";
                        break;
                    case StatusCodes.FIELD_REQUIRED:
                        response.Message = "Field Is Required, Validation Error.";
                        break;
                    case StatusCodes.RECORD_NOTFOUND:
                        response.Message = "Record Not Found.";
                        break;
                    case StatusCodes.Success_WithOutEmail:
                        response.Message = "Success But Email Not Send.";
                        break;
                    case StatusCodes.Already_Exists:
                        response.Message = "Already Exists.";
                        break;
                    case StatusCodes.Name_AlreadyExists:
                        response.Message = "Name Already Exists.";
                        break;
                    case StatusCodes.Customer_Profile_Not_Found:
                        response.Message = "Customer Profile Removed or Not Found.";
                        break;
                    case StatusCodes.Exceed_Credit_Limit:
                        response.Message = "Cannot Exceed Credit Limit.";
                        break;
                    case StatusCodes.Availed_Supervisor:
                        response.Message = "This Supervisor Already Availed Credits.";
                        break;
                    case StatusCodes.Availed_Manager:
                        response.Message = "This Manager Already Availed Credits.";
                        break;  
                    case StatusCodes.Less_Credit_Allowed:
                        response.Message = "You Do Not Have Much Limit.";
                        break;
                    default:
                        response.Message = exception.Message;
                        break;
                }
            }
        }
        public static void SignUpWSResponse<T>(ResponseBack<T> response, StatusCodes code, Exception exception, T resultObject)
        {
            if (response != null)
            {
                response.Status = (int)code;
                response.Data = resultObject;
                switch (code)
                {
                    case StatusCodes.SUCCESS_CODE:
                        response.Message = "Account Created Successfully.";
                        break;
                    case StatusCodes.FAILURE_CODE:
                        response.Message = string.Format("{0}, {1}",
                            string.Format(exception.Message),
                            (IsStackTraceEnabled && !string.IsNullOrEmpty(exception.Message) ? string.Format("{0}Stacktrace: [{1}]", Environment.NewLine, exception.StackTrace) : ""));
                        break;
                    case StatusCodes.ERROR_EXCEPTION_CAUGHT_CODE:
                        response.Message = string.Format("{0}, {1}",
                            string.Format(exception.Message),
                            (IsStackTraceEnabled && !string.IsNullOrEmpty(exception.Message) ? string.Format("{0}Stacktrace: [{1}]", Environment.NewLine, exception.StackTrace) : ""));
                        break;
                    default:
                        response.Message = exception.Message;
                        break;
                }
            }
        }
    }
}
