/******************************************************
* Programmer: Lance Zotigh (lzotigh1@cnm.edu/lzotigh1@gmail.com)
* Program: API Demo
* Purpose: Demo that shows how to create a Minimal API
********************************************************/

namespace DemoAPI.Models.DTO
{
    //This DTO just makes it so we cannot see when the coupon was last updated.
    public class CouponDTO
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public int Percent { get; set; }

        public bool IsActive { get; set; }

        //The "?" tells the compiler that this feild is nullable which should satisfy the compiler since it knows.
        public DateTime? Created { get; set; }
    }
}
