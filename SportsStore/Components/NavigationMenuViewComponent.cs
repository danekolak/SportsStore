using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using System.Linq;

namespace SportsStore.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private readonly IProductRepository _repo;

        public NavigationMenuViewComponent(IProductRepository repo)
        {
            _repo = repo;
        }
        public IViewComponentResult Invoke()
        {
            ViewBag.SelectedCategory = RouteData?.Values["category"];
            return View(_repo.Products.Select(x => x.Category).Distinct().OrderBy(x => x));
        }
    }
}
