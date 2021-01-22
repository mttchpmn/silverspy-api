using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SilverSpy.Models;

namespace SilverSpy.DataAccess
{
    public interface IDataAccessProvider
    {
        void AddTransaction(TransactionItem transaction);
        void DeleteTransaction(long id);
        Task<ActionResult<TransactionItem>> GetTransaction(long id);
        Task<ActionResult<IEnumerable<TransactionItem>>> GetTransactions();
    }
}