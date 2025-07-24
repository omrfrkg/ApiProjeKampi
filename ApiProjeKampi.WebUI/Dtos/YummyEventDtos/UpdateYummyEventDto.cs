namespace ApiProjeKampi.WebUI.Dtos.YummyEventDtos
{
    public class UpdateYummyEventDto
    {
        public int YummyEventId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile ImageFile { get; set; } // Yeni dosya yükleme için
        public string ImageUrl { get; set; }
        public string ExistingImageUrl { get; set; } // Mevcut görsel URL'sini korumak için
        public bool Status { get; set; }
        public decimal Price { get; set; }
    }
}
