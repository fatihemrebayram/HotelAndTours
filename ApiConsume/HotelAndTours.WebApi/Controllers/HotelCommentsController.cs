using AutoMapper;
using HotelAndTours.BusinessLayer.Abstract;
using HotelAndTours.DtoLayer.Dtos.HotelComments;
using HotelAndTours.EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace HotelAndTours.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HotelCommentsController : ControllerBase
{
    private readonly IHotelCommentService _HotelCommentService;
    private readonly IMapper _mapper;


    public HotelCommentsController(IHotelCommentService HotelCommentService, IMapper mapper)
    {
        _HotelCommentService = HotelCommentService;
        _mapper = mapper;
    }

    [HttpPost]
    public IActionResult AddHotelComment(HotelCommentsDto p)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        var values = _mapper.Map<HotelComment>(p);
        _HotelCommentService.AddBL(values);
        return Ok("Başarıyla otel yorumu eklendi");
    }

    [HttpGet]
    public IActionResult HotelCommentList()
    {
        var values = _HotelCommentService.GetListQueryableBL();
        return Ok(values);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteHotelComment(int id)
    {
        var roomNumber = _HotelCommentService.TGetByID(id);
        _HotelCommentService.RemoveBL(roomNumber);
        return Ok();
    }

    [HttpGet("{id}")]
    public IActionResult GetHotelCommentById(int id)
    {
        var values = _HotelCommentService.TGetByID(id);
        return Ok(values);
    }

    [HttpPut]
    public IActionResult UpdateHotelComment(HotelCommentsDto p)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        var values = _mapper.Map<HotelComment>(p);
        _HotelCommentService.UpdateBL(values);
        return Ok("Başarıyla otel yorumu güncellendi");
    }

    [HttpGet("CommentApprove/{id}")]
    public IActionResult ApproveHotelComment(int id)
    {
        if (!ModelState.IsValid)
            return BadRequest();
        var p = _HotelCommentService.TGetByID(id);
        if (p.Status)
            p.Status = false;
        else if (!p.Status)
            p.Status = true;

        var values = _mapper.Map<HotelComment>(p);
        _HotelCommentService.UpdateBL(values);
        return Ok("Başarıyla otel yorumu güncellendi");
    }
}