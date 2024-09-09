using ETicaretAPI.Application.Abstractions.Storage;
using ETicaretAPI.Application.Dtos.AwsS3;
using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Commands.ProductImageFile.UploadProductImage
{
    public class UploadProductImageCommandHandler : IRequestHandler<UploadProductImageCommandRequest, UploadProductImageCommandResponse>
    {
        readonly IProductImageFileWriteRepository _productImageFileWriteRepository;
        readonly IProductReadRepository _productReadRepository;
        readonly IFileDocumunReadRepository _fileDocumunReadRepository;
        readonly IFileDocumunWriteRepository _fileDocumunWriteRepository;
        readonly IStorageService _storageService;
        readonly IConfiguration configuration;
        public UploadProductImageCommandHandler(IProductImageFileWriteRepository productImageFileWriteRepository, IProductReadRepository productReadRepository, IStorageService storageService,IConfiguration configuration,IFileDocumunReadRepository fileDocumunReadRepository,IFileDocumunWriteRepository fileDocumunWriteRepository)
        {
            _productImageFileWriteRepository = productImageFileWriteRepository;
            _productReadRepository = productReadRepository;
            _storageService = storageService;
            _fileDocumunReadRepository = fileDocumunReadRepository;
            _fileDocumunWriteRepository = fileDocumunWriteRepository;
            this.configuration = configuration;
        }

        public async Task<UploadProductImageCommandResponse> Handle(UploadProductImageCommandRequest request, CancellationToken cancellationToken)
        {
            //Azure, AWS// AZURE "photo-images", AWS "mini-e-ticaret"
            List<(string fileName, string pathOrContainerName)> results = await _storageService.UploadAsync("photo-images", request.Files);

            Domain.Entities.Product product = await _productReadRepository.GetByIdAsync(request.Id);

            var datas = await _productImageFileWriteRepository.AddRangeAsync(results.Select(r => new Domain.Entities.ProductImageFile
            {
                FileName = r.fileName,
                Path = r.pathOrContainerName,//azur icin olan
                //Path = r.fileName,//aws icin olan
                Storage = _storageService.StorageName,
                Showcase = false,
                Products = new List<Domain.Entities.Product>() { product }
            }).ToList());

            await _productImageFileWriteRepository.SaveAsync();
            return new();
        }
    }
}
