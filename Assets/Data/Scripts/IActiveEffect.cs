using System.Collections.Generic;
public interface IActiveEffect
{
    void Execute(float deltaTime, Dictionary<string, AttributeEntity> attributes, Dictionary<string, ResourceEntity> resources);
}