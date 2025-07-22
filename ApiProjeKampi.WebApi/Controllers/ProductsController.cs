using ApiProjeKampi.WebApi.Context;
using ApiProjeKampi.WebApi.Dtos.ProductDtos;
using ApiProjeKampi.WebApi.Entities;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiProjeKampi.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IValidator<UpdateProductDto> _updateValidator;
        private readonly IValidator<CreateProductDto> _createValidator;
        private readonly ApiContext _context;
        private readonly IMapper _mapper;

        public ProductsController(IValidator<UpdateProductDto> validator, ApiContext context, IMapper mapper, IValidator<CreateProductDto> createValidator)
        {
            _updateValidator = validator;
            _context = context;
            _mapper = mapper;
            _createValidator = createValidator;
        }

        [HttpGet]
        public IActionResult ProductList()
        {
            var values = _context.Products.ToList();
            return Ok(values);
        }

        [HttpPost]
        public IActionResult CreateProduct(CreateProductDto createProductDto)
        {
            var validationResult = _createValidator.Validate(createProductDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(x => x.ErrorMessage));
            }
            else
            {
                var value = _mapper.Map<Product>(createProductDto);
                _context.Products.Add(value);
                _context.SaveChanges();
                return Ok("Ürün ekleme işlemi başarılı!");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var value = _context.Products.Find(id);
            _context.Products.Remove(value);
            _context.SaveChanges();
            return Ok("Silme işlemi başarılı!");


        }

        [HttpGet("GetProduct")]
        public IActionResult GetProduct(int id) {
            var value = _context.Products.Find(id);
            return Ok(_mapper.Map<GetProductByIdDto>(value));
        }

        [HttpPut]
        public IActionResult UpdateProduct(UpdateProductDto updateProductDto) {
            var validationResult = _updateValidator.Validate(updateProductDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(x => x.ErrorMessage));
            }
            else
            {
                var value = _mapper.Map<Product>(updateProductDto);
                _context.Products.Update(value);
                _context.SaveChanges();
                return Ok("Ürün güncelleme işlemi başarılı!");
            }
        }

        [HttpPost("CreateProductWithCategory")]
        public IActionResult CreateProductWithCategory(CreateProductDto createProductDto)
        {
            var value = _mapper.Map<Product>(createProductDto);
            _context.Products.Add(value);
            _context.SaveChanges();
            return Ok("Ekleme işlemi başarılı");
        }

        [HttpGet("ProductListWithCategory")]
        public IActionResult ProductListWithCategory()
        {
            var value = _context.Products.Include(x => x.Category).ToList();
            return Ok(_mapper.Map<List<ResultProductWithCategoryDto>>(value));
        }

    }
}
