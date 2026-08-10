using GymManagement.BLL.Common;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.AttachMemnt
{
    public class AttachmentService : IAttachmentService
    {
        private readonly long _maxFileSize = 5 *1024 *1024;  //5MB
        private readonly ILogger<AttachmentService> _logger;
        private readonly IHostEnvironment _evn;
        private readonly string[] _extentions = [".jpg", ".jpeg", ".png"];

        public AttachmentService(ILogger<AttachmentService> logger , IHostEnvironment evn)
        {
            _logger = logger;
           _evn = evn;
        }

        public async Task<Result<string>> UploadAsyc(Stream fileStream, string fileName, string folderName, CancellationToken ct = default)
        {

            if (fileStream == null || !fileStream.CanRead || fileStream.Length==0) 
                return Result<string>.NotFound("File Stream Not Found , Can Not Read Or Empty");
            if (fileStream.Length > _maxFileSize)
            {
                _logger.LogError($"File Rejected : File Too Loger {fileStream.Length} Bytes");
                return Result<string>.Failed("File Size Must Be Less Than Or Equal  5MB");
            }

            var extention = Path.GetExtension(fileName);

            if (string.IsNullOrEmpty(extention) || !_extentions.Contains(extention))
            {

                _logger.LogError($"File Rejected : extention {extention} Not Allwed");
                return Result<string>.Failed("extention {extention} Not Allwed");

            }

            var UploadsFolder = Path.Combine(_evn.ContentRootPath, folderName);
            Directory.CreateDirectory(UploadsFolder);
          
            var sortedFile = $"{Guid.NewGuid()}{fileName}";
            var path = Path.Combine(UploadsFolder, sortedFile);
          
            try
            {

                using var fs =  new FileStream(path, FileMode.Create, FileAccess.Write);
                await fileStream.CopyToAsync(fs , ct);
                return Result<string>.OK(sortedFile);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex,$"Failed To Uploade File {fileName}");
                return Result<string>.Failed($"Failed To Uploade File {fileName}");
            }


        }
      
        
        public Result Delete(string folderName, string fileName)
        {
            try
            {
                var fullPath = Path.Combine(_evn.ContentRootPath,folderName, fileName);
                if (!File.Exists(fullPath)) return Result.NotFound($"Attechment {fileName} Not Found");
                File.Delete(fullPath);
                return Result.OK();
               
             

            }
            catch(Exception ex) 
            {
                _logger.LogError(ex, $" Filed To Delete Attachment {fileName}");
                return Result.Failed($" Filed To Delete Attachment {fileName}");
            
            }
        }

        public Result<(Stream Stream, string contantType)> GetFile(string folderName, string fileName)
        {
            if (string.IsNullOrEmpty(folderName) || string.IsNullOrEmpty(fileName))
                return Result<(Stream Stream, string contantType)>.NotFound($"File : {fileName} Not Found");
            var fullPath = Path.Combine(_evn.ContentRootPath,folderName , fileName);
            if (!File.Exists(fullPath))
                return Result<(Stream Stream, string contantType)>.NotFound("File : {fileName} Not Found");


            var stream = new FileStream(fullPath,FileMode.Open,FileAccess.Read);

            var extention = Path.GetExtension(fullPath).ToLower();

            var contntType = extention switch
            {

                ".png" => "image/png",
                ".jpeg" or ".jpg" => "image/jpg",
                _ => "applacation/octec-stream"

            };

            return Result<(Stream Stream, string contantType)>.OK((stream, contntType));
        }
    }
}
