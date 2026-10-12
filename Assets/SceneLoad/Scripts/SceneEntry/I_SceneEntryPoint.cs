using System.Collections.Generic;
using System;

public interface ISceneEntryPoint
{
    bool TryGetDependency<T>(out T dependency) where T : class;
}
