using ApiProjeKampi.WebUI.Dtos.MessageDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ApiProjeKampi.WebUI.Controllers
{
    public class MessageController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public MessageController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> MessageList()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7100/api/Messages");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<ResultMessageDto>>(jsonData);
                return View(values);
            }
            return View();
        }

        [HttpGet]
        public PartialViewResult CreateMessage()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(CreateMessageDto createMessageDto)
        {
            var client = _httpClientFactory.CreateClient();
            createMessageDto.SendDate = DateTime.Now;
            var jsonData = JsonConvert.SerializeObject(createMessageDto);
            StringContent content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("https://localhost:7100/api/Messages", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.DeleteAsync($"https://localhost:7100/api/Messages?id={id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}
