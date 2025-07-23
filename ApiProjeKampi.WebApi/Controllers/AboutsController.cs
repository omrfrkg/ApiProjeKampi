using ApiProjeKampi.WebApi.Context;
using ApiProjeKampi.WebApi.Dtos.AboutDtos;
using ApiProjeKampi.WebApi.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ApiProjeKampi.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AboutsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ApiContext _context;
        public AboutsController(IMapper mapper, ApiContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public IActionResult AboutList()
        {
            var values = _context.Abouts.ToList();
            return Ok(_mapper.Map<List<ResultAboutDto>>(values));
        }

        [HttpPost]
        public IActionResult CreateAbout(CreateAboutDto createAboutDto)
        {
            var value = _mapper.Map<Entities.About>(createAboutDto);
            _context.Abouts.Add(value);
            _context.SaveChanges();
            return Ok("Hakkında Ekleme İşlemi Başarılı");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAbout(int id)
        {
            var value = _context.Abouts.Find(id);
            if (value == null)
            {
                return NotFound("Hakkında bulunamadı");
            }
            _context.Abouts.Remove(value);
            _context.SaveChanges();
            return Ok("Hakkında Silme İşlemi Başarılı");
        }

        [HttpGet("GetAbout")]
        public IActionResult GetAbout(int id)
        {
            var value = _context.Abouts.Find(id);
            if (value == null)
            {
                return NotFound("Hakkında bulunamadı");
            }
            return Ok(_mapper.Map<GetAboutByIdDto>(value));
        }

        [HttpPut]
        public IActionResult UpdateAbout(UpdateAboutDto updateAboutDto)
        {
            var value = _mapper.Map<About>(updateAboutDto);
            _context.Abouts.Update(value);
            _context.SaveChanges();
            return Ok("Hakkında Güncelleme İşlemi Başarılı");
        }
    }
}
