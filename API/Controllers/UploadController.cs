using API.DTOs;
using API.Interfaces.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class UploadController(ICloudinaryService cloudinaryService) : BaseApiController
    {
        [HttpPost("image")]
        public async Task<ActionResult<ImageUploadDto>> UploadImage(IFormFile file)
        {
            var result = await cloudinaryService.UploadImageAsync(file);

            return Ok(new ImageUploadDto
            {
                PublicId = result.PublicId,
                Url = result.Url
            });
        }
    }
}
