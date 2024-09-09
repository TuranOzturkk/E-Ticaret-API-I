using ETicaretAPI.Application.Abstractions.Storage;
using ETicaretAPI.Application.Dtos.AwsS3;
using ETicaretAPI.Application.Features.Queries.ProductImageFile.GetProductImages;
using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ETicaretAPI.Application.Features.Queries.ProductImageFile.GetProductImages
{
    public class GetProductImagesQueryHandler : IRequestHandler<GetProductImagesQueryRequest, List<GetProductImagesQueryResponse>>
    {
        readonly IProductReadRepository _productReadRepository;
        readonly IFileDocumunWriteRepository _fileDocumunWriteRepository;
        readonly IFileDocumunReadRepository _fileDocumunReadRepository; 
        readonly IConfiguration configuration;
        readonly IStorageService _storageService;
        readonly IProductImageFileReadRepository _productImageFileReadRepository;

        public GetProductImagesQueryHandler(IProductReadRepository productReadRepository,IConfiguration configuration,IStorageService storageService, IFileDocumunWriteRepository fileDocumunWriteRepository, IFileDocumunReadRepository fileDocumunReadRepository,IProductImageFileReadRepository productImageFileReadRepository)
        {
            _productReadRepository = productReadRepository;
            _fileDocumunReadRepository = fileDocumunReadRepository;
            _fileDocumunWriteRepository = fileDocumunWriteRepository;
            _productImageFileReadRepository = productImageFileReadRepository;
            this.configuration = configuration;
            _storageService = storageService;
        }

        public async Task<List<GetProductImagesQueryResponse>> Handle(GetProductImagesQueryRequest request, CancellationToken cancellationToken)
        {

            Domain.Entities.Product? product = await _productReadRepository.Table.Include(p => p.ProductImageFiles).FirstOrDefaultAsync(p => p.Id == Guid.Parse(request.Id));

            return product?.ProductImageFiles.Select(p => new GetProductImagesQueryResponse
            {
                //Path = $"{configuration["BaseStorageUrl"]}/{p.Path}", /*AZUR ICIN OLAN*/
                Path = $"{configuration["LocalStorageUrl"]}{p.Path}", /*LOCAL (wwwroot icerisinde bulunan image) ICIN OLAN*/
                FileName = p.FileName,
                Id = p.Id
            }).ToList();
        }

        //public async Task<List<GetProductImagesQueryResponse>> Handle(GetProductImagesQueryRequest request, CancellationToken cancellationToken)
        //{

        //    Domain.Entities.Product? product = await _productReadRepository.Table.Include(p => p.ProductImageFiles).FirstOrDefaultAsync(p => p.Id == Guid.Parse(request.Id));


        //    ////Gelen Product Listesinde Her Gorsel Icin AwsS3 e Baglanarak gecici URL Alip Veritabaninda guncellemesini yapiyor(NOT:BURADA SORUN OLABILIR DISPOZ EDILMESI GEREKLI, HEEP`DE GEREKSIZ VERILER TEMIZLENMESI GEREKEBILIR)
        //    //var selectFileName = product.ProductImageFiles;

        //    //foreach (var selectFileNames in selectFileName)
        //    //{
        //    //    Domain.Entities.ProductImageFile? productImageFileList = await _productImageFileReadRepository.GetByIdAsync(selectFileNames.Id.ToString());
        //    //    List<AwsUrlDto> s3Data = new List<AwsUrlDto>();
        //    //    s3Data = await _storageService.GetFilesAsync((configuration["Storage:AWS:BucketName"]), productImageFileList.FileName.ToString());
        //    //    foreach (AwsUrlDto s3Datas in s3Data)
        //    //    {
        //    //        FileDocumun fileDocumun = await _fileDocumunReadRepository.GetByIdAsync(productImageFileList.Id.ToString());
        //    //        fileDocumun.Path = s3Datas.Url;
        //    //        await _fileDocumunWriteRepository.SaveAsync();
        //    //    }
        //    //}
        //    //SON SON SON

        //    //Guncellemesi Yapilmis Veritabanini Burada Tekrar Cekerek Respons olarak return ediyor
        //    //Domain.Entities.Product? productResponse = await _productReadRepository.Table.Include(p => p.ProductImageFiles).FirstOrDefaultAsync(p => p.Id == Guid.Parse(request.Id));

        //    return product?.ProductImageFiles.Select(p => new GetProductImagesQueryResponse
        //    {
        //        Path = $"{configuration["BaseStorageUrl"]}/{p.Path}", /*AZUR ICIN OLAN*/
        //        //Path = p.Path, /*AWS ICIN OLAN*/
        //        FileName = p.FileName,
        //        Id = p.Id
        //    }).ToList();
        //    //SON SON SON
        //}
    }
}
