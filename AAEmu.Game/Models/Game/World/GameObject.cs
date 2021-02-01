﻿using System;

using AAEmu.Game.Core.Managers.World;
using AAEmu.Game.Core.Network.Game;
using AAEmu.Game.Models.Game.AI_old.Abstracts;
using AAEmu.Game.Models.Game.Char;
using AAEmu.Game.Models.Game.Units;

namespace AAEmu.Game.Models.Game.World
{
    public class GameObject
    {
        public Guid Guid { get; set; } = Guid.NewGuid();
        public uint ObjId { get; set; }
        public uint InstanceId { get; set; } = 0;
        public bool DisabledSetPosition { get; set; }
        public Point Position { get; set; }
        public Point WorldPosition { get; set; }
        public WorldPos WorldPos { get; set; }
        public Region Region { get; set; }
        public DateTime Respawn { get; set; }
        public DateTime Despawn { get; set; }
        public virtual bool IsVisible { get; set; }
        public GameObject ParentObj { get; set; }
        public virtual float ModelSize { get; set; } = 0f;

        /// <summary>
        /// Object AI
        /// </summary>
        public ACreatureAi Ai { get; protected set; }
        /// <summary>
        /// Cast object AI as Visible AI
        /// </summary>
        public AVisibleObjectAi VisibleAi => Ai;
        /// <summary>
        /// Object family
        /// </summary>
        public BaseUnitType UnitType { get; set; }


        public virtual void SetPosition(Point pos)
        {
            if (DisabledSetPosition)
            {
                return;
            }

            Position = pos.Clone();
            WorldManager.Instance.AddVisibleObject(this);
        }

        public virtual void SetPosition(float x, float y, float z)
        {
            if (DisabledSetPosition)
            {
                return;
            }

            Position.X = x;
            Position.Y = y;
            Position.Z = z;
            WorldManager.Instance.AddVisibleObject(this);
        }

        public virtual void SetPosition(float x, float y, float z, sbyte rotationX, sbyte rotationY, sbyte rotationZ)
        {
            if (DisabledSetPosition)
            {
                return;
            }

            Position.X = x;
            Position.Y = y;
            Position.Z = z;
            Position.RotationX = rotationX;
            Position.RotationY = rotationY;
            Position.RotationZ = rotationZ;
            WorldManager.Instance.AddVisibleObject(this);
        }

        public virtual void Spawn()
        {
            WorldManager.Instance.AddObject(this);
            Show();
        }

        public virtual void Delete()
        {
            Hide();
            WorldManager.Instance.RemoveObject(this);
        }

        public virtual void Show()
        {
            IsVisible = true;
            WorldManager.Instance.AddVisibleObject(this);
        }

        public virtual void Hide()
        {
            IsVisible = false;
            WorldManager.Instance.RemoveVisibleObject(this);
        }

        public virtual void BroadcastPacket(GamePacket packet, bool self)
        {
        }

        public virtual void AddVisibleObject(Character character)
        {
        }

        public virtual void RemoveVisibleObject(Character character)
        {
        }
    }
}
