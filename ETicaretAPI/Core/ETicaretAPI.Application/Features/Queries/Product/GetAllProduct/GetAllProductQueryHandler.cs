using ETicaretAPI.Application.Abstractions.Services;
using ETicaretAPI.Application.Abstractions.Storage;
using ETicaretAPI.Application.Dtos.AwsS3;
using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Application.RequestParameters;
using ETicaretAPI.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Queries.Product.GetAllProduct
{
    public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQueryRequest, GetAllProductQueryResponse>
    {
        readonly IProductReadRepository _productReadRepository;
        readonly IFileDocumunReadRepository _fileDocumunReadRepository;
        readonly IFileDocumunWriteRepository _fileDocumunWriteRepository;
        readonly IStorageService _storageService;
        readonly ILogger<GetAllProductQueryHandler> _logger;
        readonly IConfiguration configuration;
        readonly IImagePathService _imagePathService;
        public GetAllProductQueryHandler(IProductReadRepository productReadRepository,
            ILogger<GetAllProductQueryHandler> logger,
            IFileDocumunReadRepository fileDocumunReadRepository,
            IFileDocumunWriteRepository fileDocumunWriteRepository,
            IStorageService storageService,
            IConfiguration configuration,IImagePathService imagePathService)
        {
            _productReadRepository = productReadRepository;
            _fileDocumunReadRepository = fileDocumunReadRepository;
            _fileDocumunWriteRepository = fileDocumunWriteRepository;
            _storageService = storageService;
            this.configuration = configuration;
            _imagePathService = imagePathService;
            _logger = logger;
        }
        public async Task<GetAllProductQueryResponse> Handle(GetAllProductQueryRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get all products");
            var totalProductCount = _productReadRepository.GetAll(false).Count();

            //AWSS3 sisteminde her image url icin gecici bir url olusturmak icin olusturulmus servis butun image verilerine bir url olusturuluyor icerisinde
            //await _imagePathService.UpdatePathAsync();

            var products = _productReadRepository.GetAll(false).Skip(request.Page * request.Size).Take(request.Size)
                .Include(p => p.ProductImageFiles)
                .Select(p => new
            {
                p.Id,
                p.Name,
                p.Stock,
                p.Price,
                p.CreatedDate,
                p.UpdatedDate,
                p.ProductImageFiles
            }).ToList();

            return new()
            {
                Products = products,
                TotalProductCount = totalProductCount
            };
        }
    }
}
