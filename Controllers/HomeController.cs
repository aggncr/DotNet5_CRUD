using DotNet5_CRUD.Models;
using DotNet5_CRUD.Models.DataAccess;
using DotNet5_CRUD.Models.DataAccess.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DotNet5_CRUD.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        #region Book

        public IActionResult Index()
        {
            var books = _context.Books.Include(i => i.Author)
                                      .OrderByDescending(o => o.Id)
                                      .Select(s => new ListBookViewModel
                                      {
                                          Id = s.Id,
                                          Name = s.Name,
                                          PageSize = s.PageSize,
                                          Author = s.Author.FullName
                                      }).ToList();

            return View(books);
        }

        public IActionResult AddOrEditBook(int? id)
        {
            ViewBag.Title = "Add Book";
            var model = new CreateOrUpdateBookViewModel();
            model.Authors.AddRange(ListAuthors());

            if (id.HasValue)
            {
                ViewBag.Title = "Edit Book";

                var book = _context.Books.Find(id.Value);
                if (book is null)
                    return RedirectToAction(nameof(Error));

                model.Id = book.Id;
                model.Name = book.Name;
                model.PageSize = book.PageSize;
                model.AuthorId = book.AuthorId;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOrEditBook(CreateOrUpdateBookViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Authors = new List<SelectListItem>
                {
                    new SelectListItem { Value = "", Text = "Choose" }
                };

                model.Authors.AddRange(ListAuthors());
                return View(model);
            }

            if (model.Id != 0)
            {
                var book = _context.Books.Find(model.Id);
                if (book is null)
                {
                    return View(model);
                }

                book.Name = model.Name;
                book.PageSize = model.PageSize;
                book.AuthorId = model.AuthorId.Value;
            }
            else
            {
                _context.Books.Add(new Book
                {
                    Name = model.Name,
                    PageSize = model.PageSize,
                    AuthorId = model.AuthorId.Value
                });
            }

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpDelete]
        public IActionResult DeleteBook([FromBody] int id)
        {
            var book = _context.Books.Find(id);
            if (book is null)
                return RedirectToAction(nameof(Error));

            _context.Books.Remove(book);
            _context.SaveChanges();

            return new JsonResult(new { isSuccess = true });
        } 

        #endregion

        #region Author

        public IActionResult Author()
        {
            var authors = _context.Authors.OrderByDescending(o => o.Id)
                                          .Select(s => new ListAuthorViewModel
                                          {
                                              Id = s.Id,
                                              FullName = s.FullName
                                          }).ToList();

            return View(authors);
        }

        public IActionResult AddOrEditAuthor(int? id)
        {
            ViewBag.Title = "Add Author";
            var model = new CreateOrUpdateAuthorViewModel();
            if (id.HasValue)
            {
                ViewBag.Title = "Edit Author";

                var author = _context.Authors.Find(id.Value);
                if (author is null)
                    return RedirectToAction(nameof(Error));

                model.Id = author.Id;
                model.FullName = author.FullName;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOrEditAuthor(CreateOrUpdateAuthorViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.Id != 0)
            {
                var author = _context.Authors.Find(model.Id);
                if (author is null)
                {
                    return View(model);
                }

                author.FullName = model.FullName;
            }
            else
            {
                _context.Authors.Add(new Author
                {
                    FullName = model.FullName
                });
            }

            _context.SaveChanges();

            return RedirectToAction(nameof(Author));
        }

        [HttpDelete]
        public IActionResult DeleteAuthor([FromBody] int id)
        {
            var author = _context.Authors.Find(id);
            if (author is null)
                return RedirectToAction(nameof(Error));

            _context.Authors.Remove(author);
            _context.SaveChanges();

            return new JsonResult(new { isSuccess = true });
        }

        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private List<SelectListItem> ListAuthors()
        {
            return _context.Authors.OrderBy(o => o.FullName).Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.FullName }).ToList();
        }
    }
}
