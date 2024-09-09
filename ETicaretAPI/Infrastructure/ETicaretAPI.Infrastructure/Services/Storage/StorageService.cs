using ETicaretAPI.Application.Abstractions.Storage;
using ETicaretAPI.Application.Dtos.AwsS3;
using ETicaretAPI.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure.Services.Storage
{
    public class StorageService : IStorageService
    {
        readonly IStorage _storage;
        public StorageService(IStorage storage) 
        {
            _storage = storage; 
        }

        public string StorageName { get => _storage.GetType().Name; }

        public async Task DeleteAsync(string pathOrContainerName, string fileName)
        =>await _storage.DeleteAsync(pathOrContainerName, fileName);

        public List<string> GetFiles(string pathOrContainerName)
        => _storage.GetFiles(pathOrContainerName);

        public Task<List<AwsUrlDto>> GetFilesAsync(string pathOrContainerName, string? prefix)
        => _storage.GetFilesAsync(pathOrContainerName, prefix);

        public bool HasFile(string pathOrContainerName, string fileName)
        => _storage.HasFile(pathOrContainerName, fileName);

        public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string pathOrContainerName, IFormFileCollection files)
        =>await _storage.UploadAsync(pathOrContainerName, files);
    }
}
