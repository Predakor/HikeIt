namespace Application.Commons.Mappers.Interfaces;

public interface IMapper<in Input, out Output>
    where Input : class
    where Output : class
{
    Output Map(Input input);
}
