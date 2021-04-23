using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Helpers;
using BookStore.Models;
using BookStore.Models.Dto.ResultDto;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly ApplicationContext _context;


        public ImageController(IWebHostEnvironment appEnvironment,
            ApplicationContext context)
        {
            _appEnvironment = appEnvironment;
            _context = context;
        }

        [HttpPost("UploadImage/(id)")]
        public ResultDto UploadImage([FromRoute] string id, [FromForm(Name = "file")] IFormFile uploadedImage) { 
            string fileName = Guid.NewGuid().ToString() + ".jpg";
            string path = _appEnvironment.WebRootPath + @"\Images";

            if (!Directory.Exists(path)){
                Directory.CreateDirectory(path);
            }

            path = path + @"\" + fileName;
            if (uploadedImage == null) 
                return new ResultDto 
                { 
                    IsSuccessful = false,
                    Message = "Error" 
                };
            if (uploadedImage.Length == 0)
                return new ResultDto
                {
                    IsSuccessful = false,
                    Message = "Empty"
                };
            try
            {
                using (Bitmap bmp = new Bitmap(uploadedImage.OpenReadStream()))
                {
                    var saveImage = ImageWorker.CreateImage(bmp, 200, 125);
                    if (saveImage != null)
                    {
                        saveImage.Save(path, ImageFormat.Jpeg);
                        var user = _context.UserAdditionalInfo.Find(id);

                        if (user.Image != null && user.Image != "default.jpg")
                        {
                            System.IO.File.Delete(_appEnvironment.WebRootPath + @"\Image\" + user.Image);
                        }

                        _context.UserAdditionalInfo.Find(id).Image = fileName;
                        _context.SaveChanges();
                    }
                }
                return new ResultDto
                {
                    IsSuccessful = true,
                    Message = "Ok"
                };
            }
            catch (Exception ex)
            {
                return new ResultDto
                {
                    IsSuccessful = false,
                    Message = ex.Message
                };
            }
           
        }
    }
}