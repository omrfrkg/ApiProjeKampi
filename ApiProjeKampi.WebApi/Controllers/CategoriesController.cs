﻿using ApiProjeKampi.WebApi.Context;
using ApiProjeKampi.WebApi.Dtos.CategoryDtos;
using ApiProjeKampi.WebApi.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ApiProjeKampi.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ApiContext _context;
        private readonly IMapper _mapper;

        public CategoriesController(ApiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult CategoryList()
        {
            var values = _context.Categories.ToList();
            return Ok(_mapper.Map<List<ResultCategoryDto>>(values));
        }

        [HttpPost]
        public IActionResult CreateCategory(CreateCategoryDto createCategoryDto)
        {
            var value = _mapper.Map<Category>(createCategoryDto);
            _context.Categories.Add(value);
            _context.SaveChanges();
            return Ok("Kategori Ekleme İşlemi Başarılı");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id) {
            var value = _context.Categories.Find(id);
            _context.Categories.Remove(value);
            _context.SaveChanges();
            return Ok("Kategori Silme İşlemi Başarılı");
        }

        [HttpGet("GetCategory")]
        public IActionResult GetCategory(int id) {
            var value = _context.Categories.Find(id);
            return Ok(_mapper.Map<GetCategoryByIdDto>(value));
        }

        [HttpPut]
        public IActionResult UpdateCategory(UpdateCategoryDto updateCategoryDto) {
            var value = _mapper.Map<Category>(updateCategoryDto);
            _context.Categories.Update(value);
            _context.SaveChanges();
            return Ok("Kategori Güncelleme İşlemi Başarılı");
        }

    }
}
