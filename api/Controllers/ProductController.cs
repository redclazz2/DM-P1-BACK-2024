using api.Dto;
using api.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private IMapper _mapper;

        public ProductController(IProductRepository _productRepository, IMapper _mapper){
            this._mapper = _mapper;
            this._productRepository = _productRepository;
        }

        [HttpGet]
        [ProducesResponseType(200,Type = typeof(IEnumerable<ProductDto>))]
        public IActionResult GetProducts(){
            var products = _mapper.Map<List<ProductDto>>(_productRepository.GetProducts());
            if(ModelState.IsValid){
                return Ok(products);
            }else{
                return BadRequest(ModelState);
            }
        }
    }
}