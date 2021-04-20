using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using UserGroup.Web.Data;
using UserGroup.Web.ViewModels;

namespace UserGroup.Web.Controllers
{
    public class SpeakersController : Controller
    {
        

        public IActionResult Index()
        {
            return View(MockData.Speakers);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(SpeakerViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                viewModel.Id = MockData.Speakers.Max(s => s.Id) + 1;
                MockData.Speakers.Add(viewModel);
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        public IActionResult Edit(int id)
        {
            return View(MockData.Speakers[id]);
        }

        [HttpPost]
        public IActionResult Edit(SpeakerViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                MockData.Speakers[viewModel.Id] = viewModel;
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            MockData.Speakers.RemoveAt(id);
            return RedirectToAction(nameof(Index));
        }
    }
}