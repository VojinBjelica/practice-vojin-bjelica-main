using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieStore.Core.Models;
using MovieStore.Customers.Commands;
using MovieStore.Customers.Queries;
using System.Security.Claims;

namespace Movies.Controllers
{
    [ApiController]
    [Route("customers")]
    [Authorize]
    public class CustomerController : ControllerBase
    {

        private readonly IMediator _mediator;
        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Customer>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var customers = await _mediator.Send(new GetAllCustomers.Query());
            return Ok(customers);
        }

        [HttpGet("{Id:guid}")]
        [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] GetCustomerById.Query request)
        {
            var customer = await _mediator.Send(request);
            return customer == null ? NotFound() : Ok(customer);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create()
        {
            var email = User.FindFirstValue("preferred_username");
            var result = await _mediator.Send(new CreateCustomer.Command { Email = email });
            return Ok(result);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> Update([FromBody] UpdateCustomer.Command request)
        {
            var result = await _mediator.Send(request);
            return result ? NoContent() : NotFound();

        }

        [HttpDelete("{CustomerId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] DeleteCustomer.Command request)
        {

            var result = await _mediator.Send(request);
            return result ? NoContent() : NotFound();

        }

        [HttpPatch("{CustomerId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Promote([FromRoute] PromoteCustomer.Command request)
        {
            var result = await _mediator.Send(request);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return NotFound();
        }

        [HttpPost("{CustomerId:guid}/purchase/{MovieId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PurchaseMovie([FromRoute] PurchaseMovie.Command request)
        {

            var result = await _mediator.Send(request);
            if (result.IsSuccess)
            {
                return NoContent();
            }
            return NotFound();
        }

    }
}