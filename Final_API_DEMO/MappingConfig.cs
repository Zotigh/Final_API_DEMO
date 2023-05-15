/******************************************************
* Programmer: Lance Zotigh (lzotigh1@cnm.edu/lzotigh1@gmail.com)
* Program: API Demo
* Purpose: Demo that shows how to create a Minimal API
********************************************************/

using AutoMapper;
using DemoAPI.Models;
using DemoAPI.Models.DTO;

namespace DemoAPI
{
    public class MappingConfig : Profile
    {
        public MappingConfig() 
        {
            //This mapps the coupon to CouponCreateDTO. The Reverse map allows the Coupon object
            //to be mapped to another class.


            CreateMap<Coupon, CouponCreateDTO>().ReverseMap();
            CreateMap<Coupon, CouponDTO>().ReverseMap();         
        }
    }
}
