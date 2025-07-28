using ApiProjeKampi.WebApi.Context;
using ApiProjeKampi.WebApi.Dtos.ChefDtos;
using ApiProjeKampi.WebApi.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ApiProjeKampi.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChefsController : ControllerBase
    {
        private readonly ApiContext _context;
        private readonly IMapper _mapper;

        public ChefsController(ApiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult ChefList()
        {
            var values = _context.Chefs.ToList();
            return Ok(_mapper.Map<List<ResultChefDto>>(values));
        }

        [HttpPost]
        public IActionResult CreateChef(CreateChefDto createChefDto) {
            var values = _mapper.Map<Chef>(createChefDto);
            _context.Chefs.Add(values);
            _context.SaveChanges();
            return Ok("Şef Sisteme Başarıyla Eklendi.");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteChef(int id) {
            var value = _context.Chefs.Find(id);
            _context.Chefs.Remove(value);
            _context.SaveChanges();
            return Ok("Şef Sistemden Başarıyla Silindi.");
        }

        [HttpGet("GetChef")]
        public IActionResult GetChef(int id) {
            var value = _context.Chefs.Find(id);
            return Ok(_mapper.Map<GetChefByIdDto>(value));
        }

        [HttpPut]
        public IActionResult UpdateChef(UpdateChefDto updateChefDto)
        {
            var value = _mapper.Map<Chef>(updateChefDto);
            _context.Chefs.Update(value);
            _context.SaveChanges();
            return Ok("Şef Bilgileri Sistemden Başarıyla Güncellendi.");
        }
    }
}
