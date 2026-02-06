using Microsoft.AspNetCore.Mvc;
using MvcSegundaPracticaACL.Models;
using MvcSegundaPracticaACL.Repositories;

namespace MvcSegundaPracticaACL.Controllers
{
    public class ComicsController : Controller
    {
        RepositoryComics repo;

        public ComicsController()
        {
            this.repo = new RepositoryComics();

        }
        public IActionResult Index()
        {
            List<Comic> comic = this.repo.GetComics();
            return View(comic);
        }

        public IActionResult Detalles(int id)
        {
            Comic comic = this.repo.DetallesComic(id);
            return View(comic);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string nombre, string img, string descripcion)
        {
            await this.repo.CreateComicAsync(nombre, img, descripcion);
            return RedirectToAction("Index");
        } 
    }
}
