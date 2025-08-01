using ApiProjeKampi.WebApi.Context;
using ApiProjeKampi.WebApi.Dtos.GalleryDtos;
using ApiProjeKampi.WebApi.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ApiProjeKampi.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GalleriesController : ControllerBase
    {
        private readonly ApiContext _apiContext;
        private readonly IMapper _mapper;
        public GalleriesController(ApiContext apiContext, IMapper mapper)
        {
            _apiContext = apiContext;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GalleryList()
        {
            var values = _apiContext.Galleries.ToList();
            return Ok(_mapper.Map<List<ResultGalleryDto>>(values));
        }

        [HttpPost]
        public IActionResult CreateGallery(CreateGalleryDto createGalleryDto)
        {
            var value = _mapper.Map<Gallery>(createGalleryDto);
            _apiContext.Galleries.Add(value);
            _apiContext.SaveChanges();
            return Ok("Galeri Ekleme İşlemi Başarılı");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteGallery(int id)
        {
            var value = _apiContext.Galleries.Find(id);
            if (value == null)
            {
                return NotFound("Galeri bulunamadı");
            }
            _apiContext.Galleries.Remove(value);
            _apiContext.SaveChanges();
            return Ok("Galeri Silme İşlemi Başarılı");
        }

        [HttpGet("GetGallery")]
        public IActionResult GetGallery(int id)
        {
            var value = _apiContext.Galleries.Find(id);
            if (value == null)
            {
                return NotFound("Galeri bulunamadı");
            }
            return Ok(_mapper.Map<GetGalleryByIdDto>(value));
        }

        [HttpPut("UpdateGallery")]
        public IActionResult UpdateGallery(UpdateGalleryDto updateGalleryDto)
        {
            var value = _mapper.Map<Gallery>(updateGalleryDto);
            if (value == null)
            {
                return NotFound("Galeri bulunamadı");
            }

            _apiContext.Galleries.Update(value);
            _apiContext.SaveChanges();

            return Ok("Galeri Güncelleme İşlemi Başarılı");
        }

    }
}
