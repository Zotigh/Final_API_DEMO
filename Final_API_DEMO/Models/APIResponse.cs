/******************************************************
* Programmer: Lance Zotigh (lzotigh1@cnm.edu/lzotigh1@gmail.com)
* Program: API Demo
* Purpose: Demo that shows how to create a Minimal API
********************************************************/
    using System.Net;

namespace DemoAPI.Models
{
    public class APIResponse
    {
        public APIResponse() 
        {
            ErrorMessages = new List<string>();
        }
        public bool IsSuccess { get; set; }

        public Object Result { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public List<string> ErrorMessages { get; set; }
    }
}
