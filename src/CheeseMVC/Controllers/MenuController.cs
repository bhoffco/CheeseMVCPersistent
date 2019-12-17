using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CheeseMVC.Data;
using CheeseMVC.Models;
using CheeseMVC.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CheeseMVC.Controllers
{
    public class MenuController : Controller
    {
        private CheeseDbContext context;

        public MenuController(CheeseDbContext dbContext)
        {
            context = dbContext;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            List<Menu> menus = context.Menus.ToList();

            return View(menus);
        }
        public IActionResult Add()
        {
            AddMenuViewModel addMenuViewModel = new AddMenuViewModel();
            return View(addMenuViewModel);
        }
        [HttpPost]
        public IActionResult Add(AddMenuViewModel addMenuViewModel)
        {
            if (ModelState.IsValid)
            {
                Menu newMenu = new Menu
                {
                    Name = addMenuViewModel.Name,
                };

                context.Menus.Add(newMenu);
                context.SaveChanges();

                return Redirect("/Menu/ViewMenu/" + newMenu.ID);
            }

            return View(addMenuViewModel);
        }
        public IActionResult ViewMenu(int id)
        {
            if (id == 0)
            {
                return Redirect("/");
            }

            Menu theMenu = context.Menus.Single(m => m.ID == id);
            List<CheeseMenu> items = context.CheeseMenus.Include(item => item.Cheese).Where(cm => cm.MenuID == id).ToList();

            ViewMenuViewModel viewMenuViewModel = new ViewMenuViewModel(items)
            {
                Menu = theMenu,
                Items = items
            };

            ViewBag.Title = $"MENU: {theMenu.Name}";

            return View(viewMenuViewModel);
        }
        public IActionResult AddItem(int id)
        {
            Menu getMenu = context.Menus.Single(cm => cm.ID == id);
            List<SelectListItem> theCheeses = context.Cheeses
                .Select(x =>
                new SelectListItem()
                {
                    Value = x.ID.ToString(),
                    Text = x.Name
                })
                .ToList();

            AddMenuItemViewModel addMenuItemViewModel = new AddMenuItemViewModel()
            {
                Menu = getMenu,
                Cheeses = theCheeses

            };
            return View(addMenuItemViewModel);
        }
        [HttpPost]
        public IActionResult AddItem(AddMenuItemViewModel addMenuItemViewModel)
        {
            if (ModelState.IsValid)
            {
                int menuID = addMenuItemViewModel.MenuID;
                int cheeseID = addMenuItemViewModel.cheeseID;
                
                IList<CheeseMenu> existingItems = context.CheeseMenus
                    .Where(cm => cm.CheeseID == cheeseID)
                    .Where(cm => cm.MenuID == menuID).ToList();
                if (existingItems.Count == 0)
                {
                    CheeseMenu addedMenuItem = new CheeseMenu()
                    {
                        CheeseID = cheeseID,
                        MenuID = menuID
                    };

                    context.CheeseMenus.Add(addedMenuItem);
                    context.SaveChanges();

                }
                return Redirect("/Menu/ViewMenu/" + menuID);
            }
            return View();
        }
    }
}
