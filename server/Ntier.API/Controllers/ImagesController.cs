using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ntier.BLL.Interfaces;
using Ntier.DAL.DTO.Products;
using Ntier.DAL.SeedWorks;

namespace Ntier.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageService _imageService;
        public ImagesController( IImageService imageService ) { 
            _imageService = imageService;
        }

        [HttpPost]
        [Authorize(Permission.Admin.All)]

        public async Task<IActionResult> AddImagesAsync( ICollection<ImageDTO> images )
        {
            try
            {
                await _imageService.AddImageAsync(images);
                return Ok(new {
                    message = "Create image succecfully",
                });
            }
            catch ( Exception ex )
            {
                return BadRequest( ex );
            }
        }

        [HttpDelete("delete")]
        [Authorize(Permission.Admin.All)]

        public async Task<IActionResult> DeleteImagesAsync( ImageDTO[] imagesDto )
        {   
            try
            {
                await _imageService.DeleteImagesAsync( imagesDto );
                return Ok(new
                {
                    message = "Delete image succecfully",
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
