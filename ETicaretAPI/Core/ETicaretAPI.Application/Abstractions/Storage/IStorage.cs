using ETicaretAPI.Application.Dtos.AwsS3;
using ETicaretAPI.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Abstractions.Storage
{
    public interface IStorage
    {
        Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string pathOrContainerName, IFormFileCollection files);
        Task DeleteAsync(string pathOrContainerName, string fileName);
        List<string> GetFiles(string pathOrContainerName);
        Task<List<AwsUrlDto>> GetFilesAsync(string pathOrContainerName, string? prefix);
        bool HasFile(string pathOrContainerName,string fileName);
    }
}
