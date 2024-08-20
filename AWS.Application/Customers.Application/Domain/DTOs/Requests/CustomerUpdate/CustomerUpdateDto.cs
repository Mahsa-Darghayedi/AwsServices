using Customers.Application.Domain.DTOs.Requests.CustomerRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customers.Application.Domain.DTOs.Requests.CustomerUpdate
{
    public class CustomerUpdateDto
    {
        public int Id { get; set; }
        public required CustomerRequestDto Customer { get; set; }
    }
}
