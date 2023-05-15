/******************************************************
* Programmer: Lance Zotigh (lzotigh1@cnm.edu/lzotigh1@gmail.com)
* Program: API Demo
* Purpose: Demo that shows how to create a Minimal API
********************************************************/

//using DemoAPI.Models;
using DemoAPI.Models.DTO;
using FluentValidation;

namespace DemoAPI.Validation
{
    public class CouponUpdateValidation : AbstractValidator<CouponUpdateDTO>
    {
        public CouponUpdateValidation()
        {
            RuleFor(model => model.Id).NotEmpty().GreaterThan(0);

            RuleFor(model => model.Name).NotEmpty();

            RuleFor(model => model.Percent).InclusiveBetween(1, 100);
        }
    }
}
