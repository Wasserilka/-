using Lesson_1_2.Models;
using Lesson_1_2.Responses;
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
