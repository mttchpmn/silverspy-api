using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using SilverSpy.Models;

namespace SilverSpy.DataAccess
{
    public class DataAccessProvider : IDataAccessProvider
    {
        private readonly TransactionContext _context;

        public DataAccessProvider(TransactionContext context)
        {
            _context = context;
        }

        public async void  AddTransaction(TransactionItem transaction)
        {
            Console.WriteLine($"Adding transaction {transaction}");
            await _context.TransactionItems.AddAsync(transaction);
            await _context.SaveChangesAsync();
        }

        public async void DeleteTransaction(long id)
        {
            var transactionItem = await _context.TransactionItems.FindAsync(id);
            _context.TransactionItems.Remove(transactionItem);
            await _context.SaveChangesAsync();
        }

        public async Task<ActionResult<TransactionItem>> GetTransaction(long id)
        {
            var transactionItem = await _context.TransactionItems.FindAsync(id);
            return transactionItem;
        }

        public async Task<ActionResult<IEnumerable<TransactionItem>>> GetTransactions()
        {
            var transactions = await _context.TransactionItems.ToListAsync();
            return transactions;
        }
        
    }
}