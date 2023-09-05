using System.Data;
using System.Globalization;
using AutoMapper;
using HotelAndTours.BusinessLayer.Abstract;
using HotelAndTours.DataAccessLayer.Concrete;
using HotelAndTours.DtoLayer.Dtos.RoomDto;
using HotelAndTours.EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelAndTours.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize] // Restrict access to users with the "Admin" role
public class RoomController : ControllerBase
{
	private readonly IMapper _mapper;
	private readonly IRoomService _roomService;

	public RoomController(IRoomService roomService, IMapper mapper)
	{
		_roomService = roomService;
		_mapper = mapper;
	}

	[HttpPost]
	public IActionResult AddRoom(RoomDto p)
	{
		if (!ModelState.IsValid)
			return BadRequest();
		p.AddedDate = DateTime.Now;
		var values = _mapper.Map<Room>(p);
		_roomService.AddBL(values);
		return Ok("Başarıyla oda eklendi");
	}

	[HttpDelete("{id}")]
	public IActionResult DeleteRoom(int id)
	{
		var roomNumber = _roomService.TGetByID(id);
		_roomService.RemoveBL(roomNumber);
		return Ok();
	}
	[AllowAnonymous]
	[HttpGet("GetRoomsByHotelId/{id}")]
	public IActionResult GetAllRoomById(int id)
	{
		var values = _roomService.GetAllById(id);
		return Ok(values);
	}

    [AllowAnonymous]
    [HttpGet("hotels/{hotelId}/rooms/available")]
    public IActionResult GetAvailableRooms(int hotelId)
    {
        using var context = new Context();
        var availableRooms = context.RoomNumbers
            .Where(roomNumber => roomNumber.Room.HotelId == hotelId && roomNumber.Booking == null)
            .Select(roomNumber => new
            {
                roomNumber.RoomNumberId,
                roomNumber.RoomName
            })
            .ToList();

        return Ok(availableRooms);
    }

    [AllowAnonymous]
    [HttpGet("hotels/{hotelId}/rooms/{roomId}/available")]
	public List<RoomNumbers> GetAvailableRoomNumbers(int hotelId,string checkIn, string checkOut, int roomId)
	{
		var _context = new Context();

		if (DateTime.TryParseExact(checkIn, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedCheckIn)
		    && DateTime.TryParseExact(checkOut, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedCheckOut))
		{
			if (roomId == 0)
			{
				var bookedRoomNumbers = _context.Bookings
					.Where(b => parsedCheckIn <= b.CheckOut 
                                && parsedCheckOut >= b.CheckIn 
                                && b.RoomNumber.Room.HotelId == hotelId)
                    
					.Select(b => b.RoomNumberId)
					.ToList();
				var availableRoomNumbers = _context.RoomNumbers
					.Where(rn => !bookedRoomNumbers.Contains(rn.RoomNumberId)&&rn.Room.HotelId==hotelId)
                    .Include(x => x.Room)
                    .ThenInclude(x => x.Hotel)
                    .ToList();
				return availableRoomNumbers;

			}
            else if(roomId > 0)

            {
				var bookedRoomNumbers = _context.Bookings
					.Where(b => parsedCheckIn <= b.CheckOut 
					            && parsedCheckOut >= b.CheckIn 
					            && b.RoomNumber.Room.HotelId == hotelId 
					            && b.RoomNumber.RoomId==roomId)
					.Select(b => b.RoomNumberId)
					.ToList();
				var availableRoomNumbers = _context.RoomNumbers
					.Where(rn => !bookedRoomNumbers.Contains(rn.RoomNumberId)&&rn.RoomId==roomId)
                    .Include(x=>x.Room)
                    .ThenInclude(x=>x.Hotel)
                    .ToList();
				return availableRoomNumbers;

			}



		}

		return new List<RoomNumbers>();
	}



	[HttpGet("{id}")]
	public IActionResult GetRoomById(int id)
	{
		var values = _roomService.TGetByID(id);
        return Ok(values);
	}

    [AllowAnonymous]
    [HttpGet]
    public IActionResult RoomList()
	{
		var values = _roomService.GetListQueryableBL();
		return Ok(values);
	}

	[HttpPut]
	public IActionResult UpdateRoom(RoomDto p)
	{
		if (!ModelState.IsValid)
			return BadRequest();
		p.AddedDate = DateTime.Now;
		var values = _mapper.Map<Room>(p);
		_roomService.UpdateBL(values);
		return Ok("Başarıyla oda güncellendi");
	}
}