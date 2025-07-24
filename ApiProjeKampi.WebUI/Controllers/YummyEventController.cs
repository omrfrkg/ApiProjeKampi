using System.Text;
using ApiProjeKampi.WebUI.Dtos.YummyEventDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ApiProjeKampi.WebUI.Controllers
{
    public class YummyEventController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public YummyEventController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> YummyEventList()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7100/api/YummyEvents");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultYummyEventDto>>(jsonData);
                return View(values);
            }
            return View();
        }


        public async Task<IActionResult> YummyEventStatusChangeToFalse(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync($"https://localhost:7100/api/YummyEvents/YummyEventStatusChangeToFalse/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("YummyEventList");
            }
            return View();
        }


        public async Task<IActionResult> YummyEventStatusChangeToTrue(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync($"https://localhost:7100/api/YummyEvents/YummyEventStatusChangeToTrue/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("YummyEventList");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CreateYummyEvent()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateYummyEvent(CreateYummyEventDto createYummyEventDto)
        {
            if (createYummyEventDto.ImageFile != null && createYummyEventDto.ImageFile.Length > 0)
            {
                // Dosya türünü doğrula
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var extension = Path.GetExtension(createYummyEventDto.ImageFile.FileName).ToLower();
                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("ImageFile", "Sadece .jpg, .jpeg veya .png dosyaları yüklenebilir.");
                    return View(createYummyEventDto);
                }

                // Dosya boyutunu doğrula (örneğin, maks 5MB)
                if (createYummyEventDto.ImageFile.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError("ImageFile", "Dosya boyutu 5MB'ı aşamaz.");
                    return View(createYummyEventDto);
                }

                // Dosyayı kaydet
                var fileName = Guid.NewGuid().ToString() + extension;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await createYummyEventDto.ImageFile.CopyToAsync(stream);
                }

                createYummyEventDto.ImageUrl = $"/images/{fileName}";
            }
            else
            {
                createYummyEventDto.ImageUrl = "/images/default.jpg"; // Varsayılan görsel
            }

            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(createYummyEventDto);
            var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("https://localhost:7100/api/YummyEvents", stringContent);

            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("YummyEventList");
            }

            ModelState.AddModelError("", "Etkinlik oluşturma başarısız oldu.");
            return View(createYummyEventDto);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateYummyEvent(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync($"https://localhost:7100/api/YummyEvents/GetYummyEvent?id={id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var value = JsonConvert.DeserializeObject<GetYummyEventByIdDto>(jsonData);
                var updateDto = new UpdateYummyEventDto
                {
                    YummyEventId = value.YummyEventId,
                    Title = value.Title,
                    Description = value.Description,
                    Price = value.Price,
                    ExistingImageUrl = value.ImageUrl, // Mevcut görsel URL'sini sakla
                    ImageUrl = value.ImageUrl,
                    Status = value.Status
                };
                return View(updateDto);
            }
            return RedirectToAction("YummyEventList");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateYummyEvent(UpdateYummyEventDto updateYummyEventDto)
        {
            if (updateYummyEventDto.ImageFile != null && updateYummyEventDto.ImageFile.Length > 0)
            {
                // Dosya türünü doğrula
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var extension = Path.GetExtension(updateYummyEventDto.ImageFile.FileName).ToLower();
                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("ImageFile", "Sadece .jpg, .jpeg veya .png dosyaları yüklenebilir.");
                    return View(updateYummyEventDto);
                }

                // Dosya boyutunu doğrula (örneğin, maks 5MB)
                if (updateYummyEventDto.ImageFile.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError("ImageFile", "Dosya boyutu 5MB'ı aşamaz.");
                    return View(updateYummyEventDto);
                }

                // Yeni dosyayı kaydet
                var fileName = Guid.NewGuid().ToString() + extension;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await updateYummyEventDto.ImageFile.CopyToAsync(stream);
                }

                // Yeni ImageUrl'yi ayarla
                updateYummyEventDto.ImageUrl = $"/images/{fileName}";

                // Eski görseli sil (isteğe bağlı)
                if (!string.IsNullOrEmpty(updateYummyEventDto.ExistingImageUrl) && updateYummyEventDto.ExistingImageUrl != "/images/default.jpg")
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", updateYummyEventDto.ExistingImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }
            }
            else
            {
                // Yeni dosya yüklenmemişse mevcut görsel URL'sini koru
                updateYummyEventDto.ImageUrl = updateYummyEventDto.ExistingImageUrl;
            }

            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(updateYummyEventDto);
            var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PutAsync("https://localhost:7100/api/YummyEvents", stringContent);

            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("YummyEventList");
            }

            ModelState.AddModelError("", "Etkinlik güncelleme başarısız oldu.");
            return View(updateYummyEventDto);
        }

        public async Task<IActionResult> DeleteYummyEvent(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.DeleteAsync($"https://localhost:7100/api/YummyEvents?id={id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("YummyEventList");
            }
            return View();
        }
    }
}
