namespace ApiProjeKampi.WebUI.Dtos.YummyEventDtos
{
    public class CreateYummyEventDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public IFormFile ImageFile { get; set; }
        public string ImageUrl { get; set; }
    }
}
