using AutoMapper;
using BookStore.Domain.Interfaces;
using BookStore.Domain.Models;
using BookStore.WebApi.Dtos.Inventory;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.WebApi.Controllers;

[Route("api/[controller]")]
public class InventoriesController : ControllerBase
{
    private readonly IInventoryService _inventoryService;
    private readonly IMapper _mapper;

    public InventoriesController(IMapper mapper, IInventoryService inventoryService)
    {
        _mapper = mapper;
        _inventoryService = inventoryService;
    }

    [HttpGet("{bookId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int bookId)
    {
        Inventory? inventory = await _inventoryService.GetById(bookId);

        if (inventory is null) return this.NotFound();

        return this.Ok(_mapper.Map<InventoryResultDto>(inventory));
    }

    [HttpGet]
    [Route("get-inventory-by-book-name/{bookName}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<Inventory>>> SearchInventoryForBook(string bookName)
    {
        List<Inventory> inventory = _mapper.Map<List<Inventory>>(await _inventoryService.SearchInventoryForBook(bookName));

        if (!inventory.Any()) return this.NotFound("None inventory was founded");

        return this.Ok(_mapper.Map<IEnumerable<InventoryResultDto>>(inventory));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add([FromBody] InventoryAddDto inventoryDto)
    {
        if (!this.ModelState.IsValid) return this.BadRequest();

        Inventory inventory = _mapper.Map<Inventory>(inventoryDto);
        Inventory? inventoryResult = await _inventoryService.Add(inventory);

        return inventoryResult is null
            ? this.BadRequest()
            : this.Ok(_mapper.Map<InventoryResultDto>(inventoryResult));
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromBody] InventoryEditDto inventoryDto)
    {
        if (!this.ModelState.IsValid) return this.BadRequest();

        await _inventoryService.Update(_mapper.Map<Inventory>(inventoryDto));

        return this.Ok(inventoryDto);
    }

    [HttpDelete("{bookId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remove(int bookId)
    {
        Inventory? inventory = await _inventoryService.GetById(bookId);

        if (inventory is null) return this.NotFound();

        bool result = await _inventoryService.Remove(inventory);

        if (!result) return this.BadRequest();

        return this.Ok();
    }
}
