using AutoMapper;
using Ntier.BLL.Interfaces;
using Ntier.DAL.Entities;
using Ntier.DAL.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Ntier.DAL.DTO.Products;
using Ntier.BLL.Configuration;
using Microsoft.Extensions.Options;

namespace Ntier.BLL.Services
{

    public class ImageService : IImageService
    {
        private readonly IImageRepository _imageRepository;
        private readonly IMapper _mapper;
        private readonly CloudinarySettings _cloudinarySettings;

        public ImageService(IImageRepository imageRepository , IMapper mapper , IOptions<CloudinarySettings> cloudinarySettings)
        {
            _imageRepository = imageRepository;    
            _mapper = mapper;
            _cloudinarySettings = cloudinarySettings.Value;
        }

        public async Task AddImageAsync( ICollection<ImageDTO> images )
        {
            try
            {
                Account account = new Account(_cloudinarySettings.CloudName, _cloudinarySettings.ApiKey, _cloudinarySettings.ApiSecret);
                Cloudinary cloudinary = new Cloudinary(account);
                cloudinary.Api.Secure = true;
                foreach( var image in images )
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription($@"{image.Url}"),
                        Folder = "Miki_Shop",
                        Overwrite = true,
                    };
                    var uploadResult = await cloudinary.UploadAsync(uploadParams);
                    image.Url = uploadResult.Url.ToString();
                }
                await _imageRepository.AddImagesAsync(_mapper.Map<ICollection<ProductImage>>(images));
            }
            catch ( Exception ex )
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteImagesAsync(ImageDTO[] imagesDto)
        {
            await _imageRepository.DeleteImagesAsync(imagesDto);
        }
    }
}
