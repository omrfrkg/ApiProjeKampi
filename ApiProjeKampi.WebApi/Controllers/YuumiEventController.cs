using ApiProjeKampi.WebApi.Context;
using ApiProjeKampi.WebApi.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ApiProjeKampi.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class YuumiEventController : ControllerBase
    {
        private readonly ApiContext _context;
        public YuumiEventController(ApiContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult YuumiEventList()
        {
            var values = _context.YummyEvent.ToList();
            return Ok(values);
        }

        [HttpPost]
        public IActionResult CreateYuumiEvent(YummyEvent yummyEvent)
        {
            _context.YummyEvent.Add(yummyEvent);
            _context.SaveChanges();
            return Ok("Event ekleme işlemi başarılı!");
        }

        [HttpDelete]
        public IActionResult DeleteYuumiEvent(int id) {
            var values = _context.YummyEvent.Find(id);
            _context.YummyEvent.Remove(values);
            _context.SaveChanges();
            return Ok("Event silme işlemi başarılı!");
        }

        [HttpPut]
        public IActionResult UpdateYummyEvent(YummyEvent yummyEvent)
        {
            _context.YummyEvent.Update(yummyEvent);
            _context.SaveChanges();
            return Ok("Event güncelleme işlemi başarılı!");
        }
    }
}
