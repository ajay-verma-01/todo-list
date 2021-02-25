using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoListService.Infrastructure.Exceptions
{
    public class RecordNotFoundException<T> : Exception
    {
        public RecordNotFoundException(T ID) : base($"No Records for passed ID={ID}")
        {
        }
    }
}
