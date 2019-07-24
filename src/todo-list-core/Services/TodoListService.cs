using System;
using System.Linq;
using AutoMapper;
using TodoListAPI.Domain;
using TodoListAPI.Dtos;
using TodoListAPI.Interfaces;

namespace TodoListAPI.Services
{
    public class TodoListService : ITodoListService
    {
        public TodoListService(ISessionInfo session, IRepository<TodoList> repo, IMapper mapper)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [Obsolete]
        private void SeedSomeDataForTests()
        {
            for (var i = 0; i < 6; i++)
            {
                TodoList newList = TodoList.Create("foo-name", DateTime.Parse("01/01/01"), _session.UserId);
                _repo.Add(newList);
            }

            _repo.SaveChanges();
        }

        #region Implementation of ITodoListService

        public PaginatedResult<TodoListDto> Get(PageRequest pageRequest)
        {
            SeedSomeDataForTests();
            
            IQueryable<TodoList> todoListsQuery = _repo.Query().Where(todoList => todoList.OwnerId == _session.UserId);
            
            int pageNumber = pageRequest.page_number;
            int pageSize = pageRequest.page_size;
            
            TodoList[] listsPage = todoListsQuery.Skip(( pageNumber - 1 ) * pageSize)
                                                 .Take(pageSize)
                                                 .ToArray();

            var listDtosPage = _mapper.Map<TodoListDto[]>(listsPage);

            int listsCount = todoListsQuery.Count();
            
            var paginationInfo = new PaginationInfo(pageRequest, listsCount);

            return new PaginatedResult<TodoListDto>(listDtosPage, paginationInfo);
        }

        public TodoListDto Get(int id)
        {
            return new TodoListDto(1, "foo", DateTime.Parse("02/15/11 08:34 AM"));
        }

        #endregion

        private readonly ISessionInfo _session;
        private readonly IRepository<TodoList> _repo;
        private readonly IMapper _mapper;
    }
}
