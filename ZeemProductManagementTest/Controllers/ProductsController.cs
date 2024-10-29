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

        [HttpGet("GetAll")]
        public async Task<ActionResult<PaginationModel<GetProductDTO>>> GetAll(int pageSize = 10, int pageNumber = 1)
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

        [HttpGet("GetById")]
        public async Task<ActionResult<GetProductDTO>> GetById(Guid id)
        {
            try
            {
                var product = await _repository.GetByIdAsync(id);
                if (product == null) return NotFound();
                var result = _mapper.Map<GetProductDTO>(product);
                return Ok(result);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("Create")]
        public async Task<ActionResult<GetProductDTO>> Create(CreateProductDTO product)
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
    }
}
