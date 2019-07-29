using AutoMapper;
using TodoListAPI.Domain;

namespace TodoListAPI.Dtos
{
    public class DtosMappingProfile : Profile
    {
        public DtosMappingProfile()
        {
            CreateMap<TodoList, TodoListDto>();
        }
    }
}
