using TodoListAPI.Dtos;
using TodoListAPI.Services;

namespace TodoListAPI.Interfaces
{
    public interface ITodoListService
    {
        PaginatedResult<TodoListDto> Get(PageRequest pageRequest);

        TodoListDto Get(int id);
    }
}
