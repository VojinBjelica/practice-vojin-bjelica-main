using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieStore.Core.Models;
using MovieStore.Movies.Commands;
using MovieStore.Movies.Queries;

namespace MovieStore.Controllers
{
    [ApiController]
    [Route("movies")]
    [Authorize]
    public class MovieController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MovieController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GetAllMovies.Response>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var movies = await _mediator.Send(new GetAllMovies.Query());
            return Ok(movies);

        }

        [HttpGet("{MovieId:guid}")]
        [ProducesResponseType(typeof(Movie), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] GetMovieById.Query request)
        {
            var movie = await _mediator.Send(request);
            return movie == null ? NotFound() : Ok(movie);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] CreateMovie.Command request)
        {
            await _mediator.Send(request);
            return Ok();
        }

        [HttpPut()]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> Update([FromBody] UpdateMovie.Command request)
        {
            var result = await _mediator.Send(request);
            return result ? NoContent() : NotFound();
        }

        [HttpDelete("{MovieId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] DeleteMovie.Command request)
        {
            var result = await _mediator.Send(request);
            return result ? NoContent() : NotFound();
        }
    }
}
