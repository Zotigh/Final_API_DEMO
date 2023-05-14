/******************************************************
* Programmer: Lance Zotigh (lzotigh1@cnm.edu/lzotigh1@gmail.com)
* Program: API Demo
* Purpose: Demo that shows how to create a Minimal API
********************************************************/

namespace DemoAPI.Models.DTO
{
    public class CouponDeleteDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Percent { get; set; }
        public bool IsActive { get; set;}
    }
}
