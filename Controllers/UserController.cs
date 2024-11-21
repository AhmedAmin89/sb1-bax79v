using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseSystem.Data;
using WarehouseSystem.Models;

namespace WarehouseSystem.Controllers
{
    public class UserController : BaseController<User>
    {
        public UserController(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<IActionResult> Index()
        {
            var users = await _dbSet
                .Where(u => !u.IsDeleted)
                .OrderBy(u => u.Username)
                .ToListAsync();
            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<IActionResult> Create(User user)
        {
            if (ModelState.IsValid)
            {
                // Hash password before saving
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
                _dbSet.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }
    }
}