using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using UserGroup.Web.Data;
using UserGroup.Web.ViewModels;

namespace UserGroup.Web.Controllers
{
    public class EventsController : Controller
    {
        

        public IActionResult Index()
        {
            return View(MockData.Events);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(EventViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                MockData.Events.Add(viewModel);
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        public IActionResult Edit(int id)
        {
            return View(MockData.Events[id]);
        }

        [HttpPost]
        public IActionResult Edit(EventViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                MockData.Events[viewModel.Id] = viewModel;
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            MockData.Events.RemoveAt(id);
            return RedirectToAction(nameof(Index));
        }
    }
}