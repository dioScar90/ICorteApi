namespace ICorteApi.Infraestructure.Interfaces;

public interface IAuditable
{
    DateTime CreatedOnUtc { get; }
    DateTime? ModifiedOnUtc { get; }
}
