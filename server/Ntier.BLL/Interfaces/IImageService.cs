using Ntier.DAL.DTO.Products;

namespace Ntier.BLL.Interfaces
{
    public interface IImageService
    {
        public Task AddImageAsync(ICollection<ImageDTO> images);

        public Task DeleteImagesAsync(ImageDTO[] imagesDto );
    }
}
