using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZeemProductManagementTest.DTOs;
using ZeemProductManagementTest.Models;
using ZeemProductManagementTest.Repository;
using ZeemProductManagementTest.Services.Pagination;

namespace ZeemProductManagementTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepo _repository;
        private readonly IMapper _mapper;

        public ProductsController(IProductRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("GetAllProducts")]
        public async Task<ActionResult<PaginationModel<GetProductDTO>>> GetAllProducts(int pageSize = 10, int pageNumber = 1)
        {
            try
            {
                var products = await _repository.GetAllAsync(pageSize, pageNumber);
                var result = _mapper.Map<PaginationModel<GetProductDTO>>(products);
                if (products.TotalNumberOfPages == 0) return NotFound(result);
                return Ok(result);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("GetProductById")]
        public async Task<ActionResult<GetProductDTO>> GetProductById(Guid id)
        {
            try
            {
                var product = await _repository.GetByIdAsync(id);
                if (product == null) return NotFound("Product not found");
                var result = _mapper.Map<GetProductDTO>(product);
                return Ok(result);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("CreateProduct")]
        public async Task<ActionResult<GetProductDTO>> CreateProduct(CreateProductDTO product)
        {
            try
            {
                var newProduct = _mapper.Map<Product>(product);
                var result = await _repository.AddAsync(newProduct);
                var createdProduct = _mapper.Map<GetProductDTO>(result);
                return StatusCode(StatusCodes.Status201Created, createdProduct);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("UpdateProductById")]
        public async Task<ActionResult<GetProductDTO>> UpdateProductById(Guid id, UpdateProductDTO product)
        {
            try
            {
                var result = await _repository.UpdateAsync(id, product);
                if (result == null) return NotFound("Product not found");
                var updatedProduct = _mapper.Map<GetProductDTO>(result);

                return Ok(updatedProduct);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("DeleteProductById")]
        public async Task<ActionResult> DeleteProductById(Guid id)
        {
            try
            {
                var result = await _repository.DeleteAsync(id);
                if (!result) return NotFound("Product not found");
                return Ok("Product deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("SearchProducts")]
        public async Task<ActionResult<PaginationModel<GetProductDTO>>> SearchProducts(string name, int pageSize = 10, int pageNumber = 1)
        {
            try
            {
                var products = await _repository.SearchAsync(name, pageSize, pageNumber);
                var result = _mapper.Map<PaginationModel<GetProductDTO>>(products);
                if (products.TotalNumberOfPages == 0) return NotFound(result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
