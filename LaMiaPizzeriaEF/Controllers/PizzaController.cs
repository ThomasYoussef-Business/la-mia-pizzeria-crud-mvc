using LaMiaPizzeriaEF.Database;
using LaMiaPizzeriaEF.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace LaMiaPizzeriaEF.Controllers {
    public class PizzaController : Controller {
        // MAIN PAGE
        [Route("/pizze")]
        public IActionResult Index() {
            using var db = new PizzasDbContext();
            List<Pizza> pizzas = db.Pizzas.ToList();

            foreach (Pizza pizza in pizzas) {

                db.Entry(pizza)
                  .Reference(p => p.Category)
                  .Load();
            }

            return View(pizzas);
        }

        [Route("pizze/{id}")]
        public IActionResult Pizza(int id) {
            using var db = new PizzasDbContext();
            Pizza? pizza = db.Pizzas.Find(id);
            db.Entry(pizza)
              .Reference(p => p.Category)
              .Load();
            db.Entry(pizza)
              .Collection(p => p.Tags)
              .Load();
            return View(pizza);
        }

        // ADD A NEW PIZZA
        public IActionResult New() {
            using var db = new PizzasDbContext();
            PizzaViewModel viewmodel = new() {
                Pizza = new Pizza(),
                Categories = db.Categories.ToList(),
                Tags = db.Tags.ToSelectListItem()
            };
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult New(PizzaViewModel viewmodel) {
            if (!ModelState.IsValid) {
                using (var db = new PizzasDbContext()) {
                    viewmodel.Categories = db.Categories.ToList();
                    viewmodel.Tags = db.Tags.ToSelectListItem();
                    return View(viewmodel);
                }
            }

            using (var db = new PizzasDbContext()) {
                viewmodel.Pizza.Tags = viewmodel.TagIds.ConvertAll(tagId => db.Tags.Find(int.Parse(tagId)));
                // TODO: Fix this awful syntax, makes me feel like i'm programming in python fused with C++
                int modifications = db.AddPizza(viewmodel.Pizza);
                Console.WriteLine(modifications);
            }

            return RedirectToAction(nameof(Index));
        }

        // EDIT A PIZZA
        // TODO: Already added tags get wiped when editing a pizza
        public IActionResult Edit(int id) {
            using PizzasDbContext db = new();
            if (db.Pizzas.Find(id) is Pizza pizzaToEdit) {
                PizzaViewModel viewmodel = new() {
                    Pizza = pizzaToEdit,
                    Categories = db.Categories.ToList(),
                    Tags = db.Tags.ToSelectListItem()
                };

                db.Entry(pizzaToEdit)
                  .Collection(pizza => pizza.Tags)
                  .Load();
                foreach (var viewmodelTag in viewmodel.Tags) {
                    viewmodelTag.Selected = pizzaToEdit.HasTag(viewmodelTag.Value);
                }

                return View(viewmodel);
            }
            else {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, PizzaViewModel viewmodel) {
            if (!ModelState.IsValid) {
                using var db = new PizzasDbContext();
                viewmodel.Categories = db.Categories.ToList();
                viewmodel.Tags = db.Tags.ToSelectListItem();

                db.Entry(viewmodel.Pizza)
                  .Collection(pizza => pizza.Tags)
                  .Load();
                foreach (var viewmodelTag in viewmodel.Tags) {
                    viewmodelTag.Selected = viewmodel.Pizza.HasTag(viewmodelTag.Value);
                }

                return View(viewmodel);
            }

            using (var db = new PizzasDbContext()) {
                if (db.Pizzas.FirstOrDefault(p => p.Id == id) is Pizza pizzaToModify) {
                    db.Entry(pizzaToModify)
                      .Collection(p => p.Tags)
                      .Load();

                    pizzaToModify.Name = viewmodel.Pizza.Name;
                    pizzaToModify.Description = viewmodel.Pizza.Description;
                    pizzaToModify.Price = viewmodel.Pizza.Price;
                    pizzaToModify.PictureUrl = viewmodel.Pizza.PictureUrl;
                    pizzaToModify.CategoryId = viewmodel.Pizza.CategoryId;

                    pizzaToModify.Tags.Clear();
                    pizzaToModify.Tags.AddRange(viewmodel.TagIds.ToTagList(db));
                    db.SaveChanges();
                }
                else {
                    return NotFound();
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // DELETE A PIZZA
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id) {
            using PizzasDbContext db = new();
            int deletedItems = db.DeletePizza(id);

            if (deletedItems > 0) {
                return RedirectToAction(nameof(Index));
            }
            else {
                return NotFound();
            }
        }
    }
}
