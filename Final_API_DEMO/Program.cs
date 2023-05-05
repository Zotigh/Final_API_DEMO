/******************************************************
* Programmer: Lance Zotigh (lzotigh1@cnm.edu/lzotigh1@gmail.com)
* Program: API Demo
* Purpose: Demo that shows how to create a Minimal API
*******************************************************/


/*
 * ****You can use the url localhost7101 then the methods file path****
 * Ex: https ://localhost7160/api/coupon/ what ever the id is
 * in this example 3 -> https ://localhost7160/api/coupon/3 -> (remove space)
 * The local host url will be in the Request URL field once the website is ran.
*/

using AutoMapper;
using DemoAPI;
using DemoAPI.Data;
using DemoAPI.Models;
using DemoAPI.Models.DTO;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Net;

//Here is where you would use a logger function if it is not available.
//You can add the service here then use it within the methods below.

var builder = WebApplication.CreateBuilder(args);

//Add services to the container.
//Learn more about configuring Swagger/OpenAPI at https ://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Adds the Automapper dependency. ****Auto Mapper will not work without this****
builder.Services.AddAutoMapper(typeof(MappingConfig));
////Adds the Validator Serviceto the dependency (dependency injection). ****Validators will not work without this****
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/api/coupon", (ILogger<Program> _logger) =>
{
    APIResponse response = new();
    //A logger method that will tell the console what is happening via a logged message.
    _logger.Log(LogLevel.Information, "Get all Coupons");
    response.Result = CouponStore.couponList;
    response.IsSuccess = true;
    response.StatusCode = HttpStatusCode.OK;

    return Results.Ok(response);
  
}).WithName("GetCoupons").Produces<APIResponse>(200);

//This MapGet function returns the coupon with the request specific ID when ran and requested.
//Added the Get name so we can call this endpoint.
app.MapGet("/api/coupon/{id:int}", (int id) =>
{
    APIResponse response = new();
    response.Result = CouponStore.couponList.FirstOrDefault(u => u.Id == id);
    response.IsSuccess = true;
    response.StatusCode = HttpStatusCode.OK;

    //return Results.Ok(CouponStore.couponList.FirstOrDefault(u => u.Id == id));
    return Results.Ok(response);
    //}).WithName("GetCoupon").Produces<Coupon>(200);
}).WithName("GetCoupon").Produces<APIResponse>(200);

// Creates a post requests that creates a coupon and posts it to the server.
//app.MapPost("/api/coupon", (IMapper _mapper, [FromBody] CouponCreateDTO coupon_C_DTO) => {
app.MapPost("/api/coupon", async (IMapper _mapper,
    IValidator<CouponCreateDTO> _validation, [FromBody] CouponCreateDTO coupon_C_DTO) =>
{
    APIResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };

    // This will not work if this is not a async task method.
    var validationResult = await _validation.ValidateAsync(coupon_C_DTO);

    //Tells Server that if the ID is not 0 (which it should be everytime since the DataBase(DB) or server is
    //responsible for adding) or there is no name to return an error message/code. 
    if (!validationResult.IsValid)
    {
        response.ErrorMessages.Add(validationResult.Errors.FirstOrDefault().ToString());

        //return Results.BadRequest("Invalid Id or Coupon Name");

        //This can be customized if you want to return all of the headers.
        //return Results.BadRequest(validationResult.Errors.FirstOrDefault().ToString());
        return Results.BadRequest(response);
    }

    //Safe guard to check if the name of the coupon already exists to prevent duplicates.
    if (CouponStore.couponList.FirstOrDefault(u => u.Name.ToLower() == coupon_C_DTO.Name.ToLower()) != null)
    {
        response.ErrorMessages.Add("Coupon Name Already Exists");
        return Results.BadRequest(response);
    }

    Coupon coupon = _mapper.Map<Coupon>(coupon_C_DTO);

    coupon.Id = CouponStore.couponList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;

    CouponStore.couponList.Add(coupon);
    CouponDTO couponDTO = _mapper.Map<CouponDTO>(coupon);
    
    response.Result = couponDTO;
    response.IsSuccess = true;
    response.StatusCode = HttpStatusCode.Created;
    return Results.Ok();

}).WithName("CreateCoupon").Accepts<CouponCreateDTO>("application/json").Produces<APIResponse>(201).Produces(400);

app.MapPut("/api/coupon", async (IMapper _mapper,
    IValidator<CouponCreateDTO> _validation, [FromBody] CouponCreateDTO coupon_C_DTO) =>
{

});

app.MapDelete("/api/coupon/{id:int}", (int id) =>
{

});

app.UseHttpsRedirection();

app.Run();
