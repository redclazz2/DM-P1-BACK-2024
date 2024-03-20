using api.Dto;
using api.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;
        private IMapper _mapper;

        public ProductsController(IProductRepository _productRepository, IMapper _mapper)
        {
            this._mapper = _mapper;
            this._productRepository = _productRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ProductDto>))]
        public IActionResult GetProducts()
        {
            var products = _mapper.Map<List<ProductDto>>(_productRepository.GetProducts());
            if (ModelState.IsValid)
            {
                return Ok(products);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ProductDto>))]
        public IActionResult GetFavoriteProducts(int userId)
        {
            var products = _mapper.Map<List<ProductDto>>(_productRepository.GetFavoriteProducts(userId));
            if (ModelState.IsValid)
            {
                return Ok(products);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost("Favorite")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult CreateFavorite(
            [FromBody] FavoriteDto favorite
        )
        {
            var _favorite = _productRepository.GetFavoriteProducts(favorite.UserId).
                Where(p => p.Id == favorite.ProductId).FirstOrDefault();

            if (_favorite != null)
            {
                ModelState.AddModelError("", "Favorite product already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_productRepository.CreateFavorite(favorite))
            {
                ModelState.AddModelError("", "Something went wrong when creating a favorite");
                return StatusCode(500, ModelState);
            }

            return Ok("Success");
        }

        [HttpDelete("Favorite")]
        public IActionResult DeleteFavorite([FromBody]
            FavoriteDto favorite)
        {
            var _favorite = _productRepository.GetFavoriteProducts(favorite.UserId).
                Where(p => p.Id == favorite.ProductId).FirstOrDefault();

            if (_favorite == null)
            {
                ModelState.AddModelError("", "Favorite product does not exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_productRepository.DeleteFavorite(favorite))
            {
                ModelState.AddModelError("", "Something went wrong when creating a favorite");
                return StatusCode(500, ModelState);
            }
            

            return Ok("Success");
        }
    }
}