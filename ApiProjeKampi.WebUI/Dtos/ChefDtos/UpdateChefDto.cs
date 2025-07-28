namespace ApiProjeKampi.WebUI.Dtos.ChefDtos
{
    public class UpdateChefDto
    {
        public int ChefId { get; set; }
        public string NameSurname { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile ImageFile { get; set; } // Yeni dosya yükleme için
        public string ImageUrl { get; set; }
        public string ExistingImageUrl { get; set; } // Mevcut görsel URL'sini korumak için
    }
}
