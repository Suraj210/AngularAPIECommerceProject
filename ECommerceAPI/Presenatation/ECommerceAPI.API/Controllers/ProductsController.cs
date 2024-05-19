using ECommerceAPI.Application.Abstractions.Storage;
using ECommerceAPI.Application.Repositories;
using ECommerceAPI.Application.RequestParameters;
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
        readonly IFileReadRepository _fileReadRepository;
        readonly IFileWriteRepository _fileWriteRepository;
        readonly IProductImageFileWriteRepository _productImageFileWriteRepository;
        readonly IProductImageFileReadRepository _productImageFileReadRepository;
        readonly IInvoiveFileReadRepository _invoiveFileReadRepository;
        readonly IInvoiveFileWriteRepository _invoiveFileWriteRepository;
        readonly IStorageService _storageService;

        public ProductsController(IProductWriteRepository productWriteRepository,
                                  IProductReadRepository productReadRepository,
                                  IWebHostEnvironment webHostEnvironment,
                                  IFileReadRepository fileReadRepository,
                                  IFileWriteRepository fileWriteRepository,
                                  IProductImageFileWriteRepository productImageFileWriteRepository,
                                  IProductImageFileReadRepository productImageFileReadRepository,
                                  IInvoiveFileReadRepository invoiveFileReadRepository,
                                  IInvoiveFileWriteRepository invoiveFileWriteRepository,
                                  IStorageService storageService)
        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
            _webHostEnvironment = webHostEnvironment;
            _fileReadRepository = fileReadRepository;
            _fileWriteRepository = fileWriteRepository;
            _productImageFileWriteRepository = productImageFileWriteRepository;
            _productImageFileReadRepository = productImageFileReadRepository;
            _invoiveFileReadRepository = invoiveFileReadRepository;
            _invoiveFileWriteRepository = invoiveFileWriteRepository;
            _storageService = storageService;
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
        public async Task<IActionResult> Upload(string id)
        {
            List<(string fileName, string pathOrContainerName)> result = await _storageService.UploadAsync("photo-images", Request.Form.Files);



            Product product = await _productReadRepository.GetByIdAsync(id);


            //foreach (var file in result)
            //{
            //    product.ProductImageFiles.Add(new()
            //    {
            //        FileName = file.fileName,
            //        Path = file.pathOrContainerName,
            //        Storage = _storageService.StorageName,
            //    });
            //}

            await _productImageFileWriteRepository.AddRangeAsync(result.Select(r => new ProductImageFile
            {

                FileName = r.fileName,
                Path = r.pathOrContainerName,
                Storage = _storageService.StorageName,
                Products = new List<Product>() { product}

            }).ToList());

            await _productImageFileWriteRepository.SaveAsync();

            return Ok();
        }

    }
}
