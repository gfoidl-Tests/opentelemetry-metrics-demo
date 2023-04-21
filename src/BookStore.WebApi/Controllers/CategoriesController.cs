using AutoMapper;
using BookStore.Domain.Interfaces;
using BookStore.Domain.Models;
using BookStore.WebApi.Dtos.Category;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.WebApi.Controllers;

[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly IMapper _mapper;

    public CategoriesController(IMapper mapper, ICategoryService categoryService)
    {
        _mapper = mapper;
        _categoryService = categoryService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        IEnumerable<Category> categories = await _categoryService.GetAll();

        return this.Ok(_mapper.Map<IEnumerable<CategoryResultDto>>(categories));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        Category? category = await _categoryService.GetById(id);

        return category is null
            ? this.NotFound()
            : this.Ok(_mapper.Map<CategoryResultDto>(category));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add([FromBody] CategoryAddDto categoryDto)
    {
        if (!this.ModelState.IsValid) return this.BadRequest();

        Category category = _mapper.Map<Category>(categoryDto);
        Category? categoryResult = await _categoryService.Add(category);

        return categoryResult is null
            ? this.BadRequest()
            : this.Ok(_mapper.Map<CategoryResultDto>(categoryResult));
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromBody] CategoryEditDto categoryDto)
    {
        if (!this.ModelState.IsValid) return this.BadRequest();

        Category? categoryResult = await _categoryService.Update(_mapper.Map<Category>(categoryDto));

        return categoryResult is null
            ? this.BadRequest()
            : this.Ok(categoryDto);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remove(int id)
    {
        Category? category = await _categoryService.GetById(id);

        if (category is null) return this.NotFound();

        bool result = await _categoryService.Remove(category);

        if (!result) return this.BadRequest();

        return this.Ok();
    }

    [HttpGet]
    [Route("search/{category}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<Category>>> Search(string category)
    {
        List<Category> categories = _mapper.Map<List<Category>>(await _categoryService.Search(category));

        return categories == null || categories.Count == 0
            ? (ActionResult<List<Category>>)this.NotFound("None category was founded")
            : (ActionResult<List<Category>>)this.Ok(categories);
    }
}
