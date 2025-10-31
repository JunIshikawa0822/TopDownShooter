public interface IFactory<T> where T : APooledObject
{
    T ObjectInstantiate();
}
