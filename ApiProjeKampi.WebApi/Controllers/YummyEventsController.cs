using ApiProjeKampi.WebApi.Context;
using ApiProjeKampi.WebApi.Dtos.YummyEventDtos;
using ApiProjeKampi.WebApi.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ApiProjeKampi.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class YummyEventsController : ControllerBase
    {
        private readonly ApiContext _context;
        private readonly IMapper _mapper;
        public YummyEventsController(ApiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult YuumyEventList()
        {
            var values = _context.YummyEvents.ToList();
            return Ok(_mapper.Map<List<ResultYummyEventDto>>(values));
        }

        [HttpPost]
        public IActionResult CreateYuumyEvent(CreateYummyEventDto createYummyEventDto)
        {
            var value = _mapper.Map<YummyEvent>(createYummyEventDto);
            _context.YummyEvents.Add(value);
            _context.SaveChanges();
            return Ok("Event ekleme işlemi başarılı!");
        }

        [HttpDelete]
        public IActionResult DeleteYuumyEvent(int id)
        {
            var values = _context.YummyEvents.Find(id);
            _context.YummyEvents.Remove(values);
            _context.SaveChanges();
            return Ok("Event silme işlemi başarılı!");
        }

        [HttpPut]
        public IActionResult UpdateYummyEvent(UpdateYummyEventDto updateYummyEventDto)
        {
            var value = _mapper.Map<YummyEvent>(updateYummyEventDto);
            _context.YummyEvents.Update(value);
            _context.SaveChanges();
            return Ok("Event güncelleme işlemi başarılı!");
        }

        [HttpGet("GetYummyEvent")]
        public IActionResult GetYummyEvent(int id)
        {
            var value = _context.YummyEvents.Find(id);
            return Ok(_mapper.Map<GetYummyEventByIdDto>(value));
        }

        [HttpGet("YummyEventStatusChangeToFalse/{id}")]
        public IActionResult YummyEventStatusChangeToFalse(int id)
        {
            var value = _context.YummyEvents.Find(id);
            value.Status = false;
            _context.YummyEvents.Update(value);
            _context.SaveChanges();
            return Ok("Etkinlik Durumu Pasif Olarak Değitirildi!");
        }

        [HttpGet("YummyEventStatusChangeToTrue/{id}")]
        public IActionResult YummyEventStatusChangeToTrue(int id)
        {
            var value = _context.YummyEvents.Find(id);
            value.Status = true;
            _context.YummyEvents.Update(value);
            _context.SaveChanges();
            return Ok("Etkinlik Durumu Aktif Olarak Değitirildi!");
        }
    }
}
