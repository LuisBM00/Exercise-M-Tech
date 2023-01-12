using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Exercise_M_Tech.Models;
using System.Text.RegularExpressions;

namespace Exercise_M_Tech.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly APPDBContext _context;

        public EmployeesController(APPDBContext context)
        {
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                var emp = _context.employees.ToList();
                emp = emp.Where(e => e.Name.ToLower().Contains(searchString.ToLower())).ToList();
                return View(emp.ToList());
            }
              return View(await _context.employees.OrderBy(e => e.BornDate).ToListAsync());
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.employees == null)
            {
                return NotFound();
            }

            var employee = await _context.employees
                .FirstOrDefaultAsync(m => m.ID == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,LastName,RFC,BornDate,Status")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                if (!isValidRFC(employee)) return NotFound("Error in RFC Format");
                if (RFCExists(employee.RFC))
                {
                    return NotFound("RFC: "+ employee.RFC+ "already registered.");
                }
                else
                {
                    employee.RFC = employee.RFC.ToUpper();
                    _context.Add(employee);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }               
                
            }
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            
            if (id == null || _context.employees == null)
            {
                return NotFound();
            }

            var employee = await _context.employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();                
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,LastName,RFC,BornDate,Status")] Employee employee)
        {
            if (employee.RFC.Length > 13) return NotFound("RFC is limit to 13 lenght characters");
            if (!isValidRFC(employee)) return NotFound("Error in RFC Format");

            if (id != employee.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (RFCExists(employee.RFC)) return NotFound("RFC: "+ employee.RFC+" already registered.");
                try
                {
                    employee.RFC = employee.RFC.ToUpper();
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.ID))
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
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.employees == null)
            {
                return NotFound();
            }

            var employee = await _context.employees
                .FirstOrDefaultAsync(m => m.ID == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.employees == null)
            {
                return Problem("Entity set 'APPDBContext.employees'  is null.");
            }
            var employee = await _context.employees.FindAsync(id);
            if (employee != null)
            {
                _context.employees.Remove(employee);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int ID)
        {
          return _context.employees.Any(e => e.ID == ID);
        }

        private bool RFCExists(string RFC)
        {
            return _context.employees.Any(e => e.RFC == RFC);
        }

        private bool isValidRFC(Employee employee)
        {
            
            if (Regex.IsMatch(employee.RFC, "[A-z]{4}[0-9]{6}[A-z0-9]{3}") || Regex.IsMatch(employee.RFC, "[A-z]{3}[0-9]{6}[A-z0-9]{3}"))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
