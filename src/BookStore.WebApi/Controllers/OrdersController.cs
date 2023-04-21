using AutoMapper;
using BookStore.Domain.Interfaces;
using BookStore.Domain.Models;
using BookStore.WebApi.Dtos.Order;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.WebApi.Controllers;

[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IMapper _mapper;

    public OrdersController(IMapper mapper, IOrderService orderService)
    {
        _mapper = mapper;
        _orderService = orderService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        IEnumerable<Order> orders = await _orderService.GetAll();

        return this.Ok(_mapper.Map<IEnumerable<OrderResultDto>>(orders));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        Order? order = await _orderService.GetById(id);

        return order is null
            ? this.NotFound()
            : this.Ok(_mapper.Map<OrderResultDto>(order));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add([FromBody] OrderAddDto orderDto)
    {
        if (!this.ModelState.IsValid) return this.BadRequest();

        Order order = _mapper.Map<Order>(orderDto);
        Order? result = await _orderService.Add(order);

        return result is null
            ? this.BadRequest()
            : this.Ok(_mapper.Map<OrderResultDto>(result));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remove(int id)
    {
        Order? order = await _orderService.GetById(id);

        if (order is null) return this.NotFound();

        Order? result = await _orderService.Remove(order);

        if (result is null) return this.BadRequest();

        return this.Ok(_mapper.Map<OrderResultDto>(result));
    }
}
