using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using Swashbuckle.Swagger.Annotations;
using TodoListAPI.Caching;
using TodoListAPI.Dtos;
using TodoListAPI.Extensions;
using TodoListAPI.Interfaces;
using TodoListAPI.Security;

namespace TodoListAPI.v1.Controllers
{
    [TokenAuthenticate]
    [SwaggerResponse(HttpStatusCode.Unauthorized)]
    [SwaggerResponse(HttpStatusCode.NotAcceptable)]
    [SwaggerResponse(statusCode: 429, description: "too many requests (throttling)")]
    [SwaggerResponseContentType(contentType: "application/json", exclusive: true)]
    public class ListsController : ApiController
    {
        public ListsController(ITodoListService todoListService)
        {
            _todoListService = todoListService ?? throw new ArgumentNullException(nameof(todoListService));
        }

        // GET api/lists
        /// <summary>
        /// get to-do lists for user
        /// </summary>
        [SafeHttpCache]
        [SwaggerResponse(HttpStatusCode.OK, type: typeof(IEnumerable<TodoListDto>), description: "returns to-do lists for user, includes pagination info in response header Link")]
        [SwaggerResponse(HttpStatusCode.NotModified)]
        public IHttpActionResult Get([FromUri] PageRequest pageRequest)
        {
            PaginatedResult<TodoListDto> result = _todoListService.Get(pageRequest);

            IEnumerable<PaginationLinkInfo> paginationLinks = result.GetPaginationLinks(Request.RequestUri);

            return result.IsEmpty 
                       ? StatusCode(HttpStatusCode.NoContent) 
                       : Ok(result.Data).WithLinks(paginationLinks);
        }
        
        // GET api/lists/22
        [SafeHttpCache]
        public IHttpActionResult Get(int id)
        {
            TodoListDto todoListDto = _todoListService.Get(id);
            return Ok(todoListDto);
        }

        // POST api/<controller>
        [SwaggerResponse(HttpStatusCode.Created, type: typeof(TodoListDto), description: "creates a new to-do list")]
        public IHttpActionResult Post([FromBody]string value)
        {
            string location = "api/lists/11";
            TodoListDto todoListDto = new TodoListDto(id: 11, name: "foo-name", lastUpdatedAt: DateTime.Now);

            return Created(location, todoListDto);
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }

        private readonly ITodoListService _todoListService;
    }
}
