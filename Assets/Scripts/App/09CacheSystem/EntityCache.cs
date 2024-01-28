using SangoUtils_Unity_App.Entity;
using SangoUtils_Unity_App.Scene;
using SangoUtils_FixedNum;
using System.Collections.Generic;
using SangoUtils_Bases_Universal;
using SangoUtils_Extensions_UnityEngine;

namespace SangoUtils_Unity_App.Cache
{
    public class EntityCache : BaseCache
    {
        public PlayerEntity PlayerEntity_This { get; private set; }

        private Dictionary<string, PlayerEntity> _playerEntitysOnline = new();

        public PlayerEntity AddEntityLocal(string entityID)
        {
            TransformData transformData = new(new(0, 0, 0), new(0, 0, 0, 0), new(1, 1, 1));
            PlayerEntity_This = new(entityID, transformData, PlayerState.Online);
            return PlayerEntity_This;
        }

        public PlayerEntity AddEntityOnline(string entityID)
        {
            TransformData transformData = new(new(0, 0, 0), new(0, 0, 0, 0), new(1, 1, 1));
            PlayerEntity entity = new(entityID, transformData, PlayerState.Online);
            SceneMainInstance.Instance.AddNewOnlineCapsule(entity);
            _playerEntitysOnline.Add(entity.EntityID, entity);
            return entity;
        }

        public void RemoveEntityLocal()
        {

        }

        public void RemoveEntityOnline()
        {

        }

        public void AddEntityMoveKeyOnline(string entityID, FixedVector3 logicDirection)
        {
            if (entityID != PlayerEntity_This.EntityID)
            {
                if (_playerEntitysOnline.TryGetValue(entityID, out PlayerEntity entity))
                {
                    //entity.LogicDirection = logicDirection;
                    entity.LogicPosition = logicDirection;
                }
                else
                {
                    entity = AddEntityOnline(entityID);
                    //entity.LogicDirection = logicDirection;
                    entity.LogicPosition = logicDirection;
                }
            }
            else
            {
                //PlayerEntity_This.LogicDirection = logicDirection;
                PlayerEntity_This.LogicPosition = logicDirection;
            }
        }
    }
}