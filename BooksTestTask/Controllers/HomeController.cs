using BooksTestTask.DomainModels;
using BooksTestTask.Services;
using BooksTestTask.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BooksTestTask.Controllers;

public class HomeController : Controller
{
    private readonly BookService _bookService;

    public HomeController(BookService bookService)
    {
        _bookService = bookService;
    }


    [HttpGet]
    [Route("~/", Name = "Index")]
    public async Task<IActionResult> Index(string? title, string? author, string? toc)
    {
        var books = await _bookService.SearchAsync(title, author, toc);

        ViewBag.BookTitle = title;
        ViewBag.BookAuthor = author;
        ViewBag.BookToc = toc;

        return View(books);
    }


    [HttpGet]
    [Route("~/create", Name = "Create")]
    public IActionResult Create()
    {
        return View(new CreateBook());
    }
    
    [HttpPost]
    [Route("~/create", Name = "Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateBook model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var newId = await _bookService.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Ошибка при сохранении: " + ex.Message);
            return View(model);
        }
    }


    [HttpGet]
    [Route("~/details/{id}", Name = "Details")]
    public async Task<IActionResult> Details(int id)
    {
        var book = await _bookService.GetDetailByIdAsync(id);

        if (book == null)
        {
            return NotFound();
        }

        return View(book);
    }


    [HttpGet]
    [Route("~/delete/{id}", Name = "Delete")]
    public async Task<IActionResult> Delete(int id)
    {
        var book = await _bookService.GetDeleteByIdAsync(id);

        if (book == null)
        {
            return NotFound();
        }

        return View(book);
    }

    [HttpPost, ActionName("Delete")]
    [Route("~/delete/{id}", Name = "Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            await _bookService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Ошибка при удалении: " + ex.Message);

            var book = await _bookService.GetDeleteByIdAsync(id);
            return View("Delete", book);
        }
    }


    [HttpGet]
    [Route("~/edit/{id}", Name = "Edit")]
    public async Task<IActionResult> Edit(int id)
    {
        var book = await _bookService.GetEditByIdAsync(id);

        if (book == null)
        {
            return NotFound();
        }

        return View(book);
    }

    [HttpPost]
    [Route("~/edit/{id}", Name = "Edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EditBook model)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            await _bookService.UpdateAsync(model);

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Ошибка при сохранении: " + ex.Message);
            return View(model);
        }
    }
}
