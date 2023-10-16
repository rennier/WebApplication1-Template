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
using WebApplication1.Data;
using WebApplication1.Data.Migrations;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ZproductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ZproductsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        //Index-Admin can see all records
        //Guser can see only Guser records
        //Users will only see their records

        [Authorize(Roles = "GUser,Admin")]
        public async Task<ActionResult> Index()
        {
            if (User.IsInRole("GUser"))
            {
                //Dashboard Code
                var pendingCount = _context.Zproducts.Count(p => p.Status == "Pending");
                var approvedCount = _context.Zproducts.Count(p => p.Status == "Approved");

                ViewBag.PendingCount = pendingCount;
                ViewBag.ApprovedCount = approvedCount;

                //Show records that the current user created by Pending and Approved
                var currentUser = await _userManager.GetUserAsync(User);

                var zproducts = await _context.Zproducts.Where(p => p.CreatedBy == currentUser.UserName).ToListAsync();
                return View(zproducts);
            }
            else // User is Admin
            {
                //Show records by All users  by Pending and Approved
                var pendingCount = _context.Zproducts.Count(p => p.Status == "Pending");
                var approvedCount = _context.Zproducts.Count(p => p.Status == "Approved");

                ViewBag.PendingCount = pendingCount;
                ViewBag.ApprovedCount = approvedCount;
                var zproducts = await _context.Zproducts.ToListAsync();
                return View(zproducts);
            }
        }
        //dashboard button-when  pending card clicked 
        //Admin can see both guser and admin pending records when clicked dashboard button
        //Guser can see  guser  pending records only , when clicked dashboard button
        [Authorize(Roles = "GUser,Admin")]
        public async Task<ActionResult> PendingProducts()
        {
            List<Zproducts> pendingProducts;

            if (User.IsInRole("Admin"))
            {
                pendingProducts = await _context.Zproducts.Where(p => p.Status == "Pending").ToListAsync();
            }
            else
            {
                var currentUser = await _userManager.GetUserAsync(User);
                pendingProducts = await _context.Zproducts.Where(p => p.Status == "Pending" && p.CreatedBy == currentUser.UserName).ToListAsync();
            }

            return View("Index", pendingProducts);
        }


        //dashboard button-when  approved card clicked 
        //Admin can see both guser and admin approved records when clicked dashboard button
        //Guser can see  guser  approved records only , when clicked dashboard button

        [Authorize(Roles = "GUser,Admin")]
        public async Task<ActionResult> ApprovedProducts()
        {
            List<Zproducts> approvedProducts;

            if (User.IsInRole("Admin"))
            {
                approvedProducts = await _context.Zproducts.Where(p => p.Status == "Approved").ToListAsync();
            }
            else
            {
                var currentUser = await _userManager.GetUserAsync(User);
                approvedProducts = await _context.Zproducts.Where(p => p.Status == "Approved" && p.CreatedBy == currentUser.UserName).ToListAsync();
            }

            return View("Index", approvedProducts);
        }


        // GET: Zproducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Zproducts == null)
            {
                return NotFound();
            }

            var zproducts = await _context.Zproducts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (zproducts == null)
            {
                return NotFound();
            }

            return View(zproducts);
        }



        // GET: Zproducts/Create
        [Authorize(Roles = "Admin,GUser")]
        public IActionResult Create()
        {
            return View();
        }

     

        // POST: Zproducts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,GUser")]
        public async Task<IActionResult> Create( Zproducts zproducts)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                zproducts.CreatedBy = currentUser.UserName;
                zproducts.CreatedDate = DateTime.Now;
                zproducts.EditedDate = null;

                zproducts.Status = "Pending";
                _context.Add(zproducts);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(zproducts);
        }



        // GET: Zproducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Zproducts == null)
            {
                return NotFound();
            }

            var zproducts = await _context.Zproducts.FindAsync(id);
            if (zproducts == null)
            {
                return NotFound();
            }
            return View(zproducts);
        }

        // POST: Zproducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,Status")] Zproducts zproducts)
        {
            if (id != zproducts.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ViewBag.EditedDate = zproducts.CreatedDate;
                    var currentUser = await _userManager.GetUserAsync(User);
                    zproducts.EditedBy = currentUser.UserName;
                    zproducts.EditedDate = DateTime.Now;
                     

                    _context.Update(zproducts);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ZproductsExists(zproducts.Id))
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
            return View(zproducts);
        }




















        // POST: Zproducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,Status,CreatedBy")] Zproducts zproducts)
        //{
        //    if (id != zproducts.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            var currentUser = await _userManager.GetUserAsync(User);

        //            var existingProduct = await _context.Zproducts.FirstOrDefaultAsync(p => p.Id == id);
        //            if (existingProduct != null)
        //            {
        //                existingProduct.Name = zproducts.Name;
        //                existingProduct.Description = zproducts.Description;
        //                existingProduct.Price = zproducts.Price;
        //                existingProduct.Status = zproducts.Status;
        //                existingProduct.EditedBy = currentUser.UserName;
        //                existingProduct.EditedDate = DateTime.Now;
        //                existingProduct.CreatedDate = existingProduct.CreatedDate; // Keep the CreatedDate unchanged
        //            }

        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ZproductsExists(zproducts.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(zproducts);
        //}


















        // GET: Zproducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Zproducts == null)
            {
                return NotFound();
            }

            var zproducts = await _context.Zproducts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (zproducts == null)
            {
                return NotFound();
            }

            return View(zproducts);
        }

        // POST: Zproducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Zproducts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Zproducts'  is null.");
            }
            var zproducts = await _context.Zproducts.FindAsync(id);
            if (zproducts != null)
            {
                
                _context.Zproducts.Remove(zproducts);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ZproductsExists(int id)
        {
          return (_context.Zproducts?.Any(e => e.Id == id)).GetValueOrDefault();
        }









        public async Task<IActionResult> Verify(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zproducts = await _context.Zproducts.FindAsync(id);
            if (zproducts == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != null)
            {
                zproducts.VerifiedBy = currentUser.UserName;
            }             
            zproducts.VerifiedDate = DateTime.Now;
            zproducts.UnverifiedBy = null;
            zproducts.UnverifiedDate = null;

            zproducts.Status = "Approved";
            await _context.SaveChangesAsync();

            //return View(zproduct);
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Unverify(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zproducts = _context.Zproducts.Find(id);
            if (zproducts == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser != null)
            {
                zproducts.UnverifiedBy = currentUser.UserName;
            }
            zproducts.UnverifiedDate = DateTime.Now;
            zproducts.VerifiedBy = null;
            zproducts.VerifiedDate = null;
            
           
            zproducts.Status = "Pending";
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

         















    }
}
