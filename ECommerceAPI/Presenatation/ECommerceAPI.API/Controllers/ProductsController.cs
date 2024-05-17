﻿using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Application.RequestParameters;
using ECommerceAPI.Application.Services;
using ECommerceAPI.Application.ViewModels.Products;
using ECommerceAPI.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using F = ECommerceAPI.Domain.Entities;
namespace ECommerceAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IProductReadRepository _productReadRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileService _fileService;
        readonly IFileReadRepository _fileReadRepository;
        readonly IFileWriteRepository _fileWriteRepository;
        readonly IProductImageFileWriteRepository _productImageFileWriteRepository;
        readonly IProductImageFileReadRepository _productImageFileReadRepository;
        readonly IInvoiveFileReadRepository _invoiveFileReadRepository;
        readonly IInvoiveFileWriteRepository _invoiveFileWriteRepository;

        public ProductsController(IProductWriteRepository productWriteRepository,
                                  IProductReadRepository productReadRepository,
                                  IWebHostEnvironment webHostEnvironment,
                                  IFileService fileService,
                                  IFileReadRepository fileReadRepository,
                                  IFileWriteRepository fileWriteRepository,
                                  IProductImageFileWriteRepository productImageFileWriteRepository,
                                  IProductImageFileReadRepository productImageFileReadRepository,
                                  IInvoiveFileReadRepository invoiveFileReadRepository,
                                  IInvoiveFileWriteRepository invoiveFileWriteRepository)
        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
            _webHostEnvironment = webHostEnvironment;
            _fileService = fileService;
            _fileReadRepository = fileReadRepository;
            _fileWriteRepository = fileWriteRepository;
            _productImageFileWriteRepository = productImageFileWriteRepository;
            _productImageFileReadRepository = productImageFileReadRepository;
            _invoiveFileReadRepository = invoiveFileReadRepository;
            _invoiveFileWriteRepository = invoiveFileWriteRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] Pagination pagination)
        {
            var totalCount = _productReadRepository.GetAll(false).Count();
            var products = _productReadRepository.GetAll(false).Skip(pagination.Size * pagination.Page)
                                                               .Take(pagination.Size)
                                                               .Select(p => new
                                                               {
                                                                   p.Id,
                                                                   p.Name,
                                                                   p.Stock,
                                                                   p.Price,
                                                                   p.CreatedDate,
                                                                   p.UpdatedDate
                                                               });

            return Ok(new
            {

                totalCount,
                products

            });


        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            return Ok(await _productReadRepository.GetByIdAsync(id, false));
        }

        [HttpPost]
        public async Task<IActionResult> Post(VM_CreateProduct model)
        {
            await _productWriteRepository.AddAsync(new()
            {
                Name = model.Name,
                Stock = model.Stock,
                Price = model.Price
            });

            await _productWriteRepository.SaveAsync();
            return StatusCode((int)HttpStatusCode.Created);
        }

        [HttpPut]
        public async Task<IActionResult> Put(VM_UpdateProduct model)
        {
            Product product = await _productReadRepository.GetByIdAsync(model.Id);
            product.Stock = model.Stock;
            product.Name = model.Name;
            product.Price = model.Price;
            await _productWriteRepository.SaveAsync();
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _productWriteRepository.RemoveAsync(id);
            await _productWriteRepository.SaveAsync();
            return Ok();
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> Upload()
        {
            var datas = await _fileService.UploadAsync("resource/otherFiles", Request.Form.Files);

            //await _productImageFileWriteRepository.AddRangeAsync(datas.Select(d => new ProductImageFile()
            //{
            //    FileName = d.fileName,
            //    Path = d.path,
            //}).ToList());

            //await _productImageFileWriteRepository.SaveAsync();


            //await _invoiveFileWriteRepository.AddRangeAsync(datas.Select(d => new InvoiceFile()
            //{
            //    FileName = d.fileName,
            //    Path = d.path,
            //    Price = new Random ().Next()
            //}).ToList());
            //await _invoiveFileWriteRepository.SaveAsync();

            await _fileWriteRepository.AddRangeAsync(datas.Select(d => new F::File()
            {
                FileName = d.fileName,
                Path = d.path,
            }).ToList());

            await _fileWriteRepository.SaveAsync();



            return Ok();
        }

    }
}
