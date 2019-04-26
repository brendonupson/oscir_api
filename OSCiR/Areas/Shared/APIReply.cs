using System;
namespace OSCiR.Areas.Shared
{
    /// <summary>
    /// Generic reply object for all api requests.
    /// </summary>
    public class APIReply
    {
        public static string ERROR = "error";
        public static string OK = "ok";

        public string Status = OK; //ok or error
        public string StatusMessage = "";
        public int ApiVersion = 1;
        public object Data; //single object or array

        public APIReply()
        {
        }
    }
}
