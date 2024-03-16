using Ntier.DAL.DTO.Products;
using Ntier.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ntier.DAL.Interfaces
{
    public interface IImageRepository
    {
        public Task AddImagesAsync(ICollection<ProductImage> images);
        public Task DeleteImagesAsync( ImageDTO[] imagesDto);
    }
}
