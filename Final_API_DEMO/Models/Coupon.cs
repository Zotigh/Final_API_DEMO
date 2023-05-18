/******************************************************
* Programmer: Lance Zotigh (lzotigh1@cnm.edu/lzotigh1@gmail.com)
* Program: API Demo
* Purpose: Demo that shows how to create a Minimal API
*******************************************************/


namespace DemoAPI.Models
{
    public class Coupon
    {
        public int Id { get; set; }
        
        public string? Name { get; set; }

        public int Percent { get; set; }

        public bool IsActive { get; set; }

         public DateTime? Created { get; set; }

        public DateTime? LastUpdated { get; set; }
    }
}
