using ECommerceAPI.Application.Features.Commands.Product.CreateProduct;
using ECommerceAPI.Application.Features.Commands.Product.DeleteProduct;
using ECommerceAPI.Application.Features.Commands.Product.UpdateProduct;
using ECommerceAPI.Application.Features.Commands.ProductImageFile.ChangeShowcaseImage;
using ECommerceAPI.Application.Features.Commands.ProductImageFile.DeleteProductImage;
using ECommerceAPI.Application.Features.Commands.ProductImageFile.UploadProductImage;
using ECommerceAPI.Application.Features.Queries.Product.GetAllProduct;
using ECommerceAPI.Application.Features.Queries.Product.GetByIdProduct;
using ECommerceAPI.Application.Features.Queries.ProductImageFile.GetProductImages;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
namespace ECommerceAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllProductQueryRequest getAllProductQueryRequest)
        {

            GetAllProductQueryResponse response = await _mediator.Send(getAllProductQueryRequest);
            return Ok(response);

        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get([FromRoute] GetByIdProductQueryRequest getByIdProductQueryRequest)
        {

            GetByIdProductQueryResponse response = await _mediator.Send(getByIdProductQueryRequest);
            return Ok(response);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Admin")]

        public async Task<IActionResult> Post(CreateProductCommandRequest createProductCommandRequest)
        {

            CreateProductCommandResponse response = await _mediator.Send(createProductCommandRequest);
            if (response != null)
                return StatusCode((int)HttpStatusCode.Created);
            else
                return StatusCode((int)HttpStatusCode.BadRequest);
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = "Admin")]

        public async Task<IActionResult> Put([FromBody] UpdateProductCommandRequest updateProductCommandRequest)
        {
            UpdateProductCommandResponse response = await _mediator.Send(updateProductCommandRequest);
            return Ok();
        }

        [HttpDelete("{Id}")]
        [Authorize(AuthenticationSchemes = "Admin")]

        public async Task<IActionResult> Delete([FromRoute] DeleteProductCommandRequest deleteProductCommandRequest)
        {
            DeleteProductCommandResponse response = await _mediator.Send(deleteProductCommandRequest);
            return Ok();
        }


        [HttpPost("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]

        public async Task<IActionResult> Upload([FromQuery] UploadProductImageCommandRequest uploadProductImageCommandRequest)
        {


            uploadProductImageCommandRequest.Files = Request.Form.Files;

            UploadProductImageCommandResponse response = await _mediator.Send(uploadProductImageCommandRequest);

            return Ok();
        }


        [HttpGet("[action]/{id}")]
        [Authorize(AuthenticationSchemes = "Admin")]

        public async Task<IActionResult> GetProductImages([FromRoute] GetProductImagesQueryRequest getProductImagesQueryRequest)
        {

            List<GetProductImagesQueryResponse> response = await _mediator.Send(getProductImagesQueryRequest);

            return Ok(response);
        }


        [HttpDelete("[action]/{Id}")]
        [Authorize(AuthenticationSchemes = "Admin")]

        public async Task<IActionResult> DeleteProductImage([FromRoute] string id, [FromQuery] string imageId)
        {
            DeleteProductImageCommandRequest deleteProductImageCommandRequest = new()
            {
                Id = id,
                ImageId = imageId
            };
            DeleteProductImageCommandResponse response = await _mediator.Send(deleteProductImageCommandRequest);

            return Ok();
        }


        [HttpGet("[action]")]
        [Authorize(AuthenticationSchemes = "Admin")]
        public async Task<IActionResult> ChangeShowcaseImage([FromQuery]ChangeShowcaseImageCommandRequest changeShowcaseImageCommandRequest)
        {
            ChangeShowcaseImageCommandResponse response = await _mediator.Send(changeShowcaseImageCommandRequest);
            return Ok(response);
        }
    }
}
