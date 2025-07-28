using System.Text;
using System.Threading.Tasks;
using ApiProjeKampi.WebUI.Dtos.ChefDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ApiProjeKampi.WebUI.Controllers
{
    public class ChefController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ChefController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> ChefList()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7100/api/Chefs");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var value = JsonConvert.DeserializeObject<List<ResultChefDto>>(jsonData);
                return View(value); 
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CreateChef()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateChef(CreateChefDto createChefDto)
        {
            if(createChefDto.ImageFile != null && createChefDto.ImageFile.Length > 0)
            {
                // Dosya Türünü Doğrula
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var extension = Path.GetExtension(createChefDto.ImageFile.FileName).ToLower();

                // Eğer Desteklemiyorsa
                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("ImageFile", "Sadece .jpg, .jpeg veya .png dosyaları yüklenebilir.");
                    return View(createChefDto);
                }

                // Dosya Boyutunu Doğrula
                if (createChefDto.ImageFile.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError("ImageFile", "Dosya boyutu 5MB'ı aşamaz.");
                    return View(createChefDto);
                }

                // Dosyayı Kaydet
                var fileName = Guid.NewGuid().ToString() + extension;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/chef_images", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await createChefDto.ImageFile.CopyToAsync(stream);
                }

                createChefDto.ImageUrl = $"/chef_images/{fileName}";

            }
            else
            {
                createChefDto.ImageUrl = "/chef_images/default.jpg"; // Varsayılan görsel
            }

            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(createChefDto);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("https://localhost:7100/api/Chefs", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("ChefList");
            }
            return View();
        }

        public async Task<IActionResult> DeleteChef(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.DeleteAsync($"https://localhost:7100/api/Chefs/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("ChefList");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> UpdateChef(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync($"https://localhost:7100/api/Chefs/GetChef?id={id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var value = JsonConvert.DeserializeObject<UpdateChefDto>(jsonData);

                var updateDto = new UpdateChefDto
                {
                    ChefId = value.ChefId,
                    NameSurname = value.NameSurname,
                    Title = value.Title,
                    Description = value.Description,
                    ImageUrl = value.ImageUrl, // Görsel URL'si, güncelleme için gerekli
                    ExistingImageUrl = value.ImageUrl, // Mevcut görsel URL'sini sakla
                };
                return View(updateDto);
            }
            return RedirectToAction("ChefList");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateChef(UpdateChefDto updateChefDto)
        {
            if (updateChefDto.ImageFile != null && updateChefDto.ImageFile.Length > 0)
            {
                // Dosya türünü doğrula
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var extension = Path.GetExtension(updateChefDto.ImageFile.FileName).ToLower();
                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("ImageFile", "Sadece .jpg, .jpeg veya .png dosyaları yüklenebilir.");
                    return View(updateChefDto);
                }

                // Dosya boyutunu doğrula (örneğin, maks 5MB)
                if (updateChefDto.ImageFile.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError("ImageFile", "Dosya boyutu 5MB'ı aşamaz.");
                    return View(updateChefDto);
                }

                // Yeni dosyayı kaydet
                var fileName = Guid.NewGuid().ToString() + extension;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/chef_images", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await updateChefDto.ImageFile.CopyToAsync(stream);
                }

                // Yeni ImageUrl'yi ayarla
                updateChefDto.ImageUrl = $"/chef_images/{fileName}";

                // Eski görseli sil (isteğe bağlı)
                if (!string.IsNullOrEmpty(updateChefDto.ExistingImageUrl) && updateChefDto.ExistingImageUrl != "/chef_images/default.jpg")
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", updateChefDto.ExistingImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }
            }
            else
            {
                // Yeni dosya yüklenmemişse mevcut görsel URL'sini koru
                updateChefDto.ImageUrl = updateChefDto.ExistingImageUrl;
            }

            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(updateChefDto);
            var stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PutAsync("https://localhost:7100/api/Chefs", stringContent);

            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("ChefList");
            }

            ModelState.AddModelError("", "Etkinlik güncelleme başarısız oldu.");
            return View(updateChefDto);
        }
    }
}
