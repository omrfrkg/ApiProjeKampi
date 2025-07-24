using ApiProjeKampi.WebApi.Context;
using ApiProjeKampi.WebApi.Dtos.TestimonialDtos;
using ApiProjeKampi.WebApi.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ApiProjeKampi.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestimonialsController : ControllerBase
    {
        private readonly ApiContext _context;
        private readonly IMapper _mapper;

        public TestimonialsController(ApiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult TestimonialList()
        {
            var values = _context.Testimonials.ToList();
            return Ok(_mapper.Map<List<ResultTestimonialDto>>(values));
        }

        [HttpPost]
        public IActionResult CreateTestimonial(Testimonial testimonial)
        {
            var value = _mapper.Map<Testimonial>(testimonial);
            _context.Testimonials.Add(value);
            _context.SaveChanges();
            return Ok("Testimonial Ekleme İşlemi Başarılı");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTestimonial(int id)
        {
            var value = _context.Testimonials.Find(id);
            _context.Testimonials.Remove(value);
            _context.SaveChanges();
            return Ok("Testimonial Silme İşlemi Başarılı");
        }

        [HttpGet("GetTestimonial")]
        public IActionResult GetTestimonial(int id)
        {
            var value = _context.Testimonials.Find(id);
            return Ok(_mapper.Map<GetByIdTestimonialDto>(value));
        }

        [HttpPut]
        public IActionResult UpdateTestimonial(UpdateTestimonialDto updateTestimonialDto)
        {
            var value = _mapper.Map<Testimonial>(updateTestimonialDto);
            _context.Testimonials.Update(value);
            _context.SaveChanges();
            return Ok("Testimonial Güncelleme İşlemi Başarılı");
        }
    }
}
