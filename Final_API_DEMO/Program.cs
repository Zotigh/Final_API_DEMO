/******************************************************
* Programmer: Lance Zotigh (lzotigh1@cnm.edu/lzotigh1@gmail.com)
* Program: API Demo
* Purpose: Demo that shows how to create a Minimal API
*******************************************************/

using AutoMapper;
using DemoAPI;
using DemoAPI.Data;
using DemoAPI.Models;
using DemoAPI.Models.DTO;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/api/coupon", (ILogger<Program> _logger) =>
{
    APIResponse response = new();
    _logger.Log(LogLevel.Information, "Get all Coupons");
    response.Result = CouponStore.couponList;
    response.IsSuccess = true;
    response.StatusCode = HttpStatusCode.OK;

    return Results.Ok(response);

}).WithName("GetCoupons").Produces<APIResponse>(200);

app.MapGet("/api/coupon/{id:int}", (int id) =>
{
    APIResponse response = new();
    response.Result = CouponStore.couponList.FirstOrDefault(u => u.Id == id);
    response.IsSuccess = true;
    response.StatusCode = HttpStatusCode.OK;

    return Results.Ok(response);

}).WithName("GetCoupon").Produces<APIResponse>(200);

app.MapPost("/api/coupon", async (IMapper _mapper,
    IValidator<CouponCreateDTO> _validation, [FromBody] CouponCreateDTO coupon_C_DTO) =>
{
    APIResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };

    var validationResult = await _validation.ValidateAsync(coupon_C_DTO);

    if (!validationResult.IsValid)
    {
        response.ErrorMessages.Add(validationResult.Errors.FirstOrDefault().ToString());

        return Results.BadRequest(response);
    }

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
    IValidator<CouponUpdateDTO> _validation, [FromBody] CouponUpdateDTO coupon_U_DTO) =>
{
    APIResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };

    var validationResult = await _validation.ValidateAsync(coupon_U_DTO);

    if (!validationResult.IsValid)
    {
        response.ErrorMessages.Add(validationResult.Errors.FirstOrDefault().ToString());

        return Results.BadRequest(response);
    }
    Coupon couponFromStore = CouponStore.couponList.FirstOrDefault(u => u.Id == coupon_U_DTO.Id);
    couponFromStore.IsActive = coupon_U_DTO.IsActive;
    couponFromStore.Name = coupon_U_DTO.Name;
    couponFromStore.Percent = coupon_U_DTO.Percent;
    couponFromStore.LastUpdated = DateTime.Now;

    //converts the coupon from store to the DTO using AutoMapAttribute mapper
    response.Result = _mapper.Map<CouponDTO>(couponFromStore);
    response.IsSuccess = true;
    response.StatusCode = HttpStatusCode.OK;
    return Results.Ok(response);
}).WithName("UpdateCoupon").Accepts<CouponUpdateDTO>("application/json").Produces<APIResponse>(200).Produces(400);

app.MapDelete("/api/coupon/{id:int}", (int id) =>
{
    APIResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };

    Coupon couponFromStore = CouponStore.couponList.FirstOrDefault(u => u.Id == id);

    if (couponFromStore != null)
    {
        CouponStore.couponList.Remove(couponFromStore);
        response.IsSuccess = true;
        response.StatusCode = HttpStatusCode.NoContent;
        return Results.Ok(response);
    }
    else
    {
        response.ErrorMessages.Add("Invalid ID");
        return Results.BadRequest(response);
    }
});

app.UseHttpsRedirection();

app.Run();
