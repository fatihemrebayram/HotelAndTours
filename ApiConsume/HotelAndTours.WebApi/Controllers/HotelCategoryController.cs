using AutoMapper;
using HotelAndTours.BusinessLayer.Abstract;
using HotelAndTours.DtoLayer.Dtos.HotelCategory;
using HotelAndTours.EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace HotelAndTours.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HotelCategoryController : ControllerBase
{
    private readonly IHotelCategoryService _HotelCategoryService;
    private readonly IMapper _mapper;


    public HotelCategoryController(IHotelCategoryService hotelCategoryService, IMapper mapper)
    {
        _HotelCategoryService = hotelCategoryService;
        _mapper = mapper;
    }

    [HttpPost]
    public IActionResult AddHotelCategory(HotelCategoryDto p)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        var values = _mapper.Map<HotelCategory>(p);
        _HotelCategoryService.AddBL(values);
        return Ok("Başarıyla otel kategorisi eklendi");
    }

    [HttpGet]
    public IActionResult HotelCategoryList()
    {
        var values = _HotelCategoryService.GetListQueryableBL();
        return Ok(values);
    }

    [HttpGet("GetHotelCategoriesByIds/{ids}")]
    public IActionResult HotelCategoriesFromIds(string ids)
    {
        var idArray = ids.Split(',').Select(int.Parse).ToArray();

        var hotelCategories = _HotelCategoryService.GetListQueryableBL()
            .Where(x => idArray.Contains(x.HotelCategoryId))
            .ToList();

        return Ok(hotelCategories);
    }



    [HttpDelete("{id}")]
    public IActionResult DeleteHotelCategory(int id)
    {
        var roomNumber = _HotelCategoryService.TGetByID(id);
        _HotelCategoryService.RemoveBL(roomNumber);
        return Ok();
    }

    [HttpGet("{id}")]
    public IActionResult GetHotelCategoryById(int id)
    {
        var values = _HotelCategoryService.TGetByID(id);
        return Ok(values);
    }

    [HttpPut]
    public IActionResult UpdateHotelCategory(HotelCategoryDto p)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        var values = _mapper.Map<HotelCategory>(p);
        _HotelCategoryService.UpdateBL(values);
        return Ok("Başarıyla otel kategorisi güncellendi");
    }
}