using AutoMapper;
using BookStore.Domain.Interfaces;
using BookStore.Domain.Models;
using BookStore.WebApi.Dtos.Book;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.WebApi.Controllers;

[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;
    private readonly IMapper _mapper;

    public BooksController(IMapper mapper, IBookService bookService)
    {
        _mapper = mapper;
        _bookService = bookService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        IEnumerable<Book> books = await _bookService.GetAll();

        return this.Ok(_mapper.Map<IEnumerable<BookResultDto>>(books));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        Book? book = await _bookService.GetById(id);

        return book is null
            ? this.NotFound()
            : this.Ok(_mapper.Map<BookResultDto>(book));
    }

    [HttpGet]
    [Route("get-books-by-category/{categoryId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBooksByCategory(int categoryId)
    {
        IEnumerable<Book> books = await _bookService.GetBooksByCategory(categoryId);

        return books.Any()
            ? this.Ok(_mapper.Map<IEnumerable<BookResultDto>>(books))
            : this.NotFound();
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add([FromBody] BookAddDto bookDto)
    {
        if (!this.ModelState.IsValid) return this.BadRequest();

        Book book = _mapper.Map<Book>(bookDto);
        Book? bookResult = await _bookService.Add(book);

        return bookResult is null
            ? this.BadRequest()
            : this.Ok(_mapper.Map<BookResultDto>(bookResult));
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromBody] BookEditDto bookDto)
    {
        if (!this.ModelState.IsValid) return this.BadRequest();

        Book? bookResult = await _bookService.Update(_mapper.Map<Book>(bookDto));

        return bookResult is null
            ? this.BadRequest()
            : this.Ok(bookDto);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remove(int id)
    {
        Book? book = await _bookService.GetById(id);

        if (book is null) return this.NotFound();

        await _bookService.Remove(book);

        return this.Ok();
    }

    [HttpGet]
    [Route("search/{bookName}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<Book>>> Search(string bookName)
    {
        List<Book> books = _mapper.Map<List<Book>>(await _bookService.Search(bookName));

        if (books == null || books.Count == 0) return this.NotFound("None book was founded");

        return this.Ok(books);
    }

    [HttpGet]
    [Route("search-book-with-category/{searchedValue}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<Book>>> SearchBookWithCategory(string searchedValue)
    {
        List<Book> books = _mapper.Map<List<Book>>(await _bookService.SearchBookWithCategory(searchedValue));

        if (!books.Any()) return this.NotFound("None book was founded");

        return this.Ok(_mapper.Map<IEnumerable<BookResultDto>>(books));
    }
}
