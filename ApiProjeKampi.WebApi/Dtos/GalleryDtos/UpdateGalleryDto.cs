namespace ApiProjeKampi.WebApi.Dtos.GalleryDtos
{
    public class UpdateGalleryDto
    {
        public int GalleryId { get; set; }
        public string Title { get; set; }
        public string Tag { get; set; }
        public string ImageUrl { get; set; }
    }
}
