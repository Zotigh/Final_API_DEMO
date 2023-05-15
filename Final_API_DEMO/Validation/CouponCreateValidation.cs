/******************************************************
* Programmer: Lance Zotigh (lzotigh1@cnm.edu/lzotigh1@gmail.com)
* Program: API Demo
* Purpose: Demo that shows how to create a Minimal API
********************************************************/

using FluentValidation;
//using DemoAPI.Models;
using DemoAPI.Models.DTO;

namespace DemoAPI.Validation
{
    public class CouponCreateValidation : AbstractValidator<CouponCreateDTO>
    {
       public CouponCreateValidation()
        {
           RuleFor(model => model.Name).NotEmpty();

           //Defines that the percent must be between a certain threshold 1-100.
            RuleFor(model => model.Percent).InclusiveBetween(1, 100);
        }
    }
}
