using Lesson_1_2.DAL.Models;
using Lesson_1_2.DAL.DTO;
using AutoMapper;

namespace Lesson_1_2
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Card, CardDto>();
        }
    }
}
