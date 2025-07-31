using ApiProjeKampi.WebApi.Context;
using ApiProjeKampi.WebApi.Dtos.ReservationDtos;
using ApiProjeKampi.WebApi.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SendGrid.Helpers.Mail;
using SendGrid;

namespace ApiProjeKampi.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly ApiContext _context;
        private readonly IMapper _mapper;
        private readonly ISendGridClient _sendGridClient;
        public ReservationsController(ApiContext context, IMapper mapper, ISendGridClient sendGridClient)
        {
            _context = context;
            _mapper = mapper;
            _sendGridClient = sendGridClient;
        }

        [HttpGet]
        public IActionResult ReservationList()
        {
            var values = _context.Reservations.ToList();
            return Ok(_mapper.Map<List<ResultReservationDto>>(values));
        }

        [HttpPost]
        public IActionResult CreateReservation(CreateReservationDto createReservationDto)
        {
            var value = _mapper.Map<Reservation>(createReservationDto);
            _context.Reservations.Add(value);
            _context.SaveChanges();
            return Ok("Rezervasyon Başarıyla Eklendi!");
        }

        [HttpPut]
        public IActionResult UpdateReservation(UpdateReservationDto updateReservationDto)
        {
            var value = _mapper.Map<Reservation>(updateReservationDto);
            _context.Reservations.Update(value);
            _context.SaveChanges();
            return Ok("Rezervasyon Başarıyla Güncellendi!");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteReservation(int id)
        {
            var value = _context.Reservations.Find(id);
            if (value == null)
            {
                return NotFound("Rezervasyon Bulunamadı!");
            }
            _context.Reservations.Remove(value);
            _context.SaveChanges();
            return Ok("Rezervasyon Başarıyla Silindi!");
        }

        [HttpGet("{id}")]
        public IActionResult GetReservationById(int id)
        {
            var value = _context.Reservations.Find(id);
            if (value == null)
            {
                return NotFound("Rezervasyon Bulunamadı!");
            }
            return Ok(_mapper.Map<ResultReservationDto>(value));

        }

        [HttpGet("AcceptReservation")]
        public async Task<IActionResult> AcceptReservation(int id)
        {
            var value = _context.Reservations.Find(id);
            if (value == null)
            {
                return NotFound("Rezervasyon Bulunamadı!");
            }
            else if (value.ReservationStatus == "Onaylandı")
            {
                return BadRequest("Rezervasyon zaten onaylandı!");
            }
            else
            {
                value.ReservationStatus = "Onaylandı";

                _context.Reservations.Update(value);
                _context.SaveChanges();

                var from = new EmailAddress("omerrfarukgundogdu@outlook.com", "Ömer Faruk Gündoğdu");
                var subject = "Rezervasyon İşlem Durumu Güncellemesi";
                var to = new EmailAddress(value.Email, value.NameSurname);
                var plainTextContent = "Rezervasyonunuz onaylandı!";
                var htmlContent = "" +
                    "<!DOCTYPE html>" +
                    "<html lang=\"tr\">" +
                    "<head>" +
                    "<meta charset=\"UTF-8\">" +
                    "<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">" +
                    "<title>Rezervasyon Onayı | Yummy</title>" +
                    "<style> " +
                    "body { font-family: 'Arial', sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: 0 auto; padding: 20px; } " +
                    ".header { background-color: #8B0000; color: white; padding: 20px; text-align: center; border-radius: 8px 8px 0 0; }" +
                    ".content { padding: 20px; background-color: #f9f9f9; border-left: 1px solid #ddd; border-right: 1px solid #ddd; }" +
                    ".footer { background-color: #8B0000; color: white; padding: 10px; text-align: center; border-radius: 0 0 8px 8px; " +
                    "font-size: 12px; } .button { background-color: #8B0000; color: white; padding: 10px 20px; text-decoration: none; " +
                    "border-radius: 5px; display: inline-block; } .details { background: white; padding: 15px; border-radius: 5px; " +
                    "margin: 15px 0; border: 1px solid #eee; } </style>" +
                    "</head>" +
                    "<body>" +
                    "<div class=\"header\">" +
                    "<h1>Rezervasyon Onaylandı!</h1>" +
                    "</div>" +
                    "<div class=\"content\">" +
                    $"<p>Sayın {value.NameSurname},</p>" +
                    $"<p>Yummy'de {value.ReservationDate.Date.ToShortDateString()} tarihli {value.CountOfPeople} kişilik rezervasyonunuz onaylanmıştır.</p>" +
                    "<div class=\"details\">" +
                    "<h3>Rezervasyon Detayları:</h3>" +
                    $"<p><strong>Tarih:</strong> {value.ReservationDate.Date.ToShortDateString()}</p>" +
                    $"<p><strong>Saat:</strong> {value.ReservationTime}</p>" +
                    $"<p><strong>Kişi Sayısı:</strong> {value.CountOfPeople}</p>" +
                    $"<p><strong>Rezervasyon No:</strong> {value.ReservationId}</p></div>" +
                    "<p>Rezervasyonunuzda değişiklik yapmak için lütfen <a href=\"info@yummy.net\">bizimle iletişime geçin</a>.</p>" +
                    "<p>Bizi tercih ettiğiniz için teşekkür ederiz!</p>" +
                    "<a href=\"www.yummy.com\" class=\"button\">Web Sitemizi Ziyaret Edin</a></div>" +
                    "<div class=\"footer\"> <p>Yummy | Samsun / Türkiye | +90 532 236 7568 | info@yummy.net</p>" +
                    $"<p>© 2023 Tüm hakları saklıdır.</p></div>" +
                    $"</body>" +
                    $"</html>";
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await _sendGridClient.SendEmailAsync(msg);

                return Ok("Mail Gönderme İşlem Başarılı!");
            }
        }

        [HttpGet("CancelReservation")]
        public async Task<IActionResult> CancelReservation(int id)
        {
            var value = _context.Reservations.Find(id);
            if (value == null)
            {
                return NotFound("Rezervasyon Bulunamadı!");
            }
            else if (value.ReservationStatus == "İptal Edildi")
            {
                return BadRequest("Rezervasyon zaten iptal edildi!");
            }
            else
            {
                value.ReservationStatus = "İptal Edildi";

                _context.Reservations.Update(value);
                _context.SaveChanges();

                var from = new EmailAddress("omerrfarukgundogdu@outlook.com", "Ömer Faruk Gündoğdu");
                var subject = "Rezervasyon İşlem Durumu Güncellemesi";
                var to = new EmailAddress(value.Email, value.NameSurname);
                var plainTextContent = "Rezervasyonunuz onaylandı!";
                var htmlContent = "" +
                    "<!DOCTYPE html>" +
                    "<html lang=\"tr\">" +
                    "<head>" +
                    "<meta charset=\"UTF-8\">" +
                    "<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">" +
                    "<title>Rezervasyon Talebi | Yummy</title>" +
                    "<style>" +
                    "body { font-family: 'Arial', sans-serif; line-height: 1.6; color: #333; max-width: 600px; margin: 0 auto; padding: 20px; }" +
                    ".header { background-color: #8B0000; color: white; padding: 20px; text-align: center; border-radius: 8px 8px 0 0; }" +
                    ".content { padding: 20px; background-color: #f9f9f9; border-left: 1px solid #ddd; border-right: 1px solid #ddd; }" +
                    ".footer { background-color: #8B0000; color: white; padding: 10px; text-align: center; border-radius: 0 0 8px 8px; font-size: 12px; }" +
                    ".button { background-color: #8B0000; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px; display: inline-block; }" +
                    "</style>" +
                    "</head>" +
                    "<body>" +
                    "<div class=\"header\">" +
                    "<h1>Rezervasyon Talebiniz</h1></div>" +
                    "<div class=\"content\">" +
                    $"<p>Sayın {value.NameSurname},</p>" +
                    $"<p>Maalesef {value.ReservationDate.Date.ToShortDateString()} tarihli rezervasyon talebinizi kabul edemiyoruz. <strong>" +
                    $"Talep ettiğiniz saatte uygun masa bulunmamaktadır.</strong></p>" +
                    $"<p>Size daha iyi hizmet verebilmek için aşağıdaki alternatif saatleri önerebiliriz:</p>" +
                    $"<ul>" +
                    $"<li>[Alternatif Tarih/Saat 1]</li>" +
                    $"<li>[Alternatif Tarih/Saat 2]</li></ul>" +
                    $"<p>Yeni bir rezervasyon oluşturmak için <a href=\"#\">buraya tıklayabilir</a> veya bizi arayabilirsiniz.</p>" +
                    $"<p>Anlayışınız için teşekkür ederiz.</p>" +
                    $"<a href=\"yummy@info.net\" class=\"button\">Bize Ulaşın</a></div>" +
                    $"<div class=\"footer\"<p>Yummy | Samsun / Türkiye | +90 532 236 7568 | info@yummy.net</p>" +
                    $"</div></body>" +
                    $"</html>";
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await _sendGridClient.SendEmailAsync(msg);

                return Ok("Mail Gönderme İşlem Başarılı!");
            }
        }

    }
}
