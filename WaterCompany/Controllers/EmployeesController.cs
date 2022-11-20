using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WaterCompany.Data;
using WaterCompany.Data.Entities;
using WaterCompany.Helpers;
using WaterCompany.Models;

namespace WaterCompany.Controllers
{
	public class EmployeesController : Controller
	{
		private readonly IEmployeeRepository _employeeRepository;
		private readonly IUserHelper _userHelper;
		private readonly IBlobHelper _blobHelper;
		private readonly IConverterHelper _converterHelper;
        private readonly UserManager<User> _userManager;
        private readonly DataContext _context;

        public EmployeesController(IEmployeeRepository employeeRepository,
			IUserHelper userHelper,
			IBlobHelper blobHelper,
			IConverterHelper converterHelper, UserManager<User> userManager, DataContext context)
		{
			_employeeRepository = employeeRepository;
			_userHelper = userHelper;
			_blobHelper = blobHelper;
			_converterHelper = converterHelper;
            _userManager = userManager;
            _context = context;
        }

		[Authorize(Roles = "Admin")]
		public IActionResult Index()
		{
			return View(_employeeRepository.GetAllWithUsers());
		}

		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return new NotFoundViewResult("EmployeeNotFound");
			}

			var employee = await _employeeRepository.GetByIdAsync(id.Value);
			if (employee == null)
			{
				return new NotFoundViewResult("EmployeeNotFound");
			}

			return View(employee);
		}

		public async Task<IActionResult> Edit(int? id)
		{
			if (this.User.IsInRole("Employee"))
			{
				var thisUser = await _userManager.GetUserAsync(HttpContext.User);
				var userId = thisUser.Id;
				var employeeId = _context.Employees.Include(u => u.User).Where(u => u.User == thisUser).Select(u => u.Id).Single();
				id = employeeId;
			}


			if (id == null)
			{
				return new NotFoundViewResult("EmployeeNotFound");
			}

			var employee = await _employeeRepository.GetByIdAsync(id.Value);
			if (employee == null)
			{
				return new NotFoundViewResult("EmployeeNotFound");
			}

			var model = _converterHelper.ToEmployeeViewModel(employee);
			return View(model);
		}

		[Authorize(Roles = "Admin")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, EmployeeViewModel model, User user)
		{
			if (id != model.Id)
			{
				return new NotFoundViewResult("EmployeeNotFound");
			}

			if (ModelState.IsValid)
			{
				try
				{
					Guid imageId = Guid.Empty;

					if (model.ImageFile != null && model.ImageFile.Length > 0)
					{
						imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "employees");
					}

					var employee = _converterHelper.ToEmployee(user, model, imageId, false);
					model.ImageId = imageId;

					var employeeOld = await _employeeRepository.GetByIdAsync(id);
					user = await _userHelper.GetUserByEmailAsync(employeeOld.Email);

					user.FirstName = model.FirstName;
					user.LastName = model.LastName;
					user.Email = model.Email;
					user.PhoneNumber = model.PhoneNumber;
					user.ImageId = model.ImageId;

					await _userHelper.UpdateUserAsync(user);
					await _employeeRepository.UpdateAsync(model);
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!await _employeeRepository.ExistAsync(model.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Index));
			}
			return View(model);
		}

		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return new NotFoundViewResult("EmployeeNotFound");
			}

			var employee = await _employeeRepository.GetByIdAsync(id.Value);
			if (employee == null)
			{
				return new NotFoundViewResult("EmployeeNotFound");
			}

			return View(employee);
		}

		[Authorize(Roles = "Admin")]
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var employee = await _employeeRepository.GetByIdAsync(id);
			await _employeeRepository.DeleteAsync(employee);
			return RedirectToAction(nameof(Index));
		}

		public IActionResult EmployeeNotFound()
		{
			return View();
		}
	}
}
