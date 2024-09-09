using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.Abstractions.Storage;
using ETicaretAPI.Application.Dtos.AwsS3;
using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence.Services
{
    public class ImagePathService : IImagePathService
    {
        readonly IFileDocumunReadRepository _fileDocumunReadRepository;
        readonly IFileDocumunWriteRepository _fileDocumunWriteRepository;
        readonly IStorageService _storageService;
        readonly IConfiguration configuration;
        public ImagePathService(IFileDocumunReadRepository fileDocumunReadRepository, IFileDocumunWriteRepository fileDocumunWriteRepository,IConfiguration configuration,IStorageService storageService)
        {
            _fileDocumunReadRepository = fileDocumunReadRepository;
            _fileDocumunWriteRepository = fileDocumunWriteRepository;
            this.configuration = configuration;
            _storageService = storageService;
        }
        public async Task UpdatePathAsync()
        {
            List<FileDocumun>? fileDocumuns = _fileDocumunReadRepository.GetAll().ToList();
            foreach (var fileDocumun in fileDocumuns)
            {
                List<AwsUrlDto>? s3Datas = await _storageService.GetFilesAsync((configuration["Storage:AWS:BucketName"]), fileDocumun.FileName);
                fileDocumun.Path = s3Datas?.Select(x => x.Url).FirstOrDefault();
            }
            await _fileDocumunWriteRepository.SaveAsync();
        }
    }
}
