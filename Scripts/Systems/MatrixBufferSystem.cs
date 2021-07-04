﻿using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

public class MatrixBufferSystem : SystemBase
{
    NativeList<Entity> m_bufferEntities;

    protected override void OnCreate()
    {
        base.OnCreate();
        m_bufferEntities = new NativeList<Entity>(50, Allocator.Persistent);
    }

    protected override void OnUpdate()
    {
        DynamicBufferManager.CopyBufferEntities(m_bufferEntities);
        var bufferEntities = m_bufferEntities.AsArray();
        var entityManager = EntityManager;

        Entities.ForEach((Entity e, in BufferHook hook, in LocalToWorld localToWorld) =>
        {
            var buffer = GetBuffer<MatrixBuffer>(bufferEntities[hook.bufferEnityID]);
            buffer[hook.bufferID] = localToWorld.Value;
        })
        .WithReadOnly(bufferEntities)
        .WithChangeFilter<LocalToWorld>()
        .Schedule();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        m_bufferEntities.Dispose();
    }
}