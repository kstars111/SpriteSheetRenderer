﻿using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace SpriteSheetRendererExamples
{
    public class DynamicAnimationsDemo : MonoBehaviour, IConvertGameObjectToEntity
    {
        public static Entity character;
        public SpriteSheetAnimator animator;
        [SerializeField] Shader m_shader;
        public void Convert(Entity entity, EntityManager eManager, GameObjectConversionSystem conversionSystem)
        {
            SpriteSheetCache.Instance.Init(m_shader);
            //eManager.SetNameInd(entity, "CONVERt ENTITY");



            // 1) Create Archetype
            EntityArchetype archetype = eManager.CreateArchetype(
                     typeof(LocalToWorld),
                     typeof(Translation),
                     typeof(Rotation),
                     typeof(NonUniformScale),
                     //required params
                     typeof(SpriteIndex),
                     typeof(SpriteSheetAnimation),
                     typeof(SpriteSheetMaterial),
                     typeof(SpriteSheetColor),
                     typeof(BufferHook)
            );
            SpriteSheetManager.RecordAnimator(animator);


            // 4) Instantiate the entity
            character = SpriteSheetManager.Instantiate(archetype, animator);
            //eManager.SetName(character, "DynamicAnimationsDemo");
            // 3) Populate components
            var color = Color.white;
            eManager.AddComponentData(character, new Translation { Value = new float3(15) });
            eManager.AddComponentData(character, new NonUniformScale { Value = new float3(-3, 7, 0) });
            eManager.AddComponentData(character, new SpriteSheetColor { color = new float4(color.r, color.g, color.b, color.a) });

            SpriteMovement.Sprite = character;
        }
    }
}