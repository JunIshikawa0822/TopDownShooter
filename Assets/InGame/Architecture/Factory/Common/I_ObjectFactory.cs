public interface IObjectFactory<T> where T : APooledObject
{
    T ObjectInstantiate();
}