using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SilverSpy.DataAccess;
using SilverSpy.Models;

namespace SilverSpy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionItemsController : ControllerBase
    {
        private readonly TransactionContext _context;

        public TransactionItemsController(TransactionContext context)
        {
            _context = context;
        }

        // GET: api/TransactionItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionItem>>> GetTransactionItems()
        {
            Console.WriteLine("FETCHING ITEMS");
            return await _context.TransactionItems.ToListAsync();
        }

        // GET: api/TransactionItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionItem>> GetTransactionItem(long id)
        {
            var transactionItem = await _context.TransactionItems.FindAsync(id);

            if (transactionItem == null)
            {
                return NotFound();
            }

            return transactionItem;
        }

        // PUT: api/TransactionItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransactionItem(long id, TransactionItem transactionItem)
        {
            if (id != transactionItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(transactionItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TransactionItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TransactionItem>> PostTransactionItem(TransactionItem transactionItem)
        {
            _context.TransactionItems.Add(transactionItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTransactionItem", new { id = transactionItem.Id }, transactionItem);
        }

        // DELETE: api/TransactionItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransactionItem(long id)
        {
            var transactionItem = await _context.TransactionItems.FindAsync(id);
            if (transactionItem == null)
            {
                return NotFound();
            }

            _context.TransactionItems.Remove(transactionItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TransactionItemExists(long id)
        {
            return _context.TransactionItems.Any(e => e.Id == id);
        }
    }
}
