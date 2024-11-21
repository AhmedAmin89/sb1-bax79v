using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseSystem.Data;
using WarehouseSystem.Models;
using WarehouseSystem.Services;

namespace WarehouseSystem.Controllers
{
    [Authorize]
    public abstract class BaseController<T> : Controller where T : BaseEntity
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;
        protected readonly ICurrentUserService _currentUserService;

        protected BaseController(
            ApplicationDbContext context,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<IActionResult> Index()
        {
            var entities = await _dbSet.Where(e => !e.IsDeleted).ToListAsync();
            return View(entities);
        }

        public virtual async Task<IActionResult> Details(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null || entity.IsDeleted)
            {
                return NotFound();
            }
            return View(entity);
        }

        [Authorize(Roles = "Admin")]
        public virtual IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public virtual async Task<IActionResult> Create(T entity)
        {
            if (ModelState.IsValid)
            {
                _dbSet.Add(entity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(entity);
        }

        [Authorize(Roles = "Admin")]
        public virtual async Task<IActionResult> Edit(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null || entity.IsDeleted)
            {
                return NotFound();
            }
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public virtual async Task<IActionResult> Edit(int id, T entity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _dbSet.Update(entity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await EntityExists(id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(entity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public virtual async Task<IActionResult> Delete(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            entity.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        protected virtual async Task<bool> EntityExists(int id)
        {
            return await _dbSet.AnyAsync(e => !e.IsDeleted && EF.Property<int>(e, "Id") == id);
        }
    }
}