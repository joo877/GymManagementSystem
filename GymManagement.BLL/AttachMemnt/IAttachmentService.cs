using GymManagement.BLL.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.AttachMemnt
{
    public interface IAttachmentService
    {
        Task<Result<string>> UploadAsyc(Stream fileSteam,string fileName ,string folderName, CancellationToken ct = default);

        Result Delete(string folderName ,string fileName );

       Result<(Stream Stream, string contantType)> GetFile(string folderName, string fileName);


    }
}
