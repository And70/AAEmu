﻿using System;
using AAEmu.Commons.Utils;
using AAEmu.Game.Core.Managers;
using AAEmu.Game.Core.Managers.World;
using AAEmu.Game.Core.Packets.G2C;
using AAEmu.Game.Models.Game.NPChar;
using AAEmu.Game.Models.Game.Units.Movements;
using AAEmu.Game.Models.Tasks.UnitMove;
using AAEmu.Game.Utils;

namespace AAEmu.Game.Models.Game.Units.Route
{
    /// <summary>
    /// Jerky movement
    /// </summary>
    public class Jerky : Patrol
    {
        public short Degree { get; set; } = 360;

        /// <summary>
        /// Jerky movement
        /// </summary>
        /// <param name="caster">Trigger role</param>
        /// <param name="npc">NPC</param>
        /// <param name="degree">Default angle 360 degrees</param>
        public override void Execute(Npc npc)
        {
            var x = npc.Position.X;
            var y = npc.Position.Y;

            var rnd = Rand.Next(0, 1000);
            if (rnd > 500)
            {
                if (Count < Degree / 2)
                {
                    npc.Position.X += (float)0.1;
                }
                else if (Count < Degree)
                {
                    npc.Position.X -= (float)0.1;
                }
            }
            else
            {
                if (Count < Degree / 4 || (Count > (Degree / 4 + Degree / 2) && Count < Degree))
                {
                    npc.Position.Y += (float)0.1;
                }
                else if (Count < (Degree / 4 + Degree / 2))
                {
                    npc.Position.Y -= (float)0.1;
                }
            }

            // Simulated unit
            const MoveTypeEnum type = (MoveTypeEnum)1;
            // Return moveType object
            var moveType = (UnitMoveType)MoveType.GetType(type);

            // Change NPC coordinates
            moveType.X = npc.Position.X;
            moveType.Y = npc.Position.Y;
            moveType.Z = AppConfiguration.Instance.HeightMapsEnable ? WorldManager.Instance.GetHeight(npc.Position.ZoneId, npc.Position.X, npc.Position.Y) : npc.Position.Z;

            var angle = MathUtil.CalculateAngleFrom(x, y, npc.Position.X, npc.Position.Y);
            var rotZ = MathUtil.ConvertDegreeToDirection(angle);
            moveType.RotationX = 0;
            moveType.RotationY = 0;
            moveType.RotationZ = rotZ;

            moveType.Flags = 5;     // 5-идти, 4-бежать (мобы прыжками), 3-стоять на месте
            moveType.DeltaMovement = new sbyte[3];
            moveType.DeltaMovement[0] = 0;
            moveType.DeltaMovement[1] = 127; // 88.. 118
            moveType.DeltaMovement[2] = 0;
            moveType.Stance = 1;    // COMBAT = 0x0, IDLE = 0x1
            moveType.Alertness = 0; // IDLE = 0x0, ALERT = 0x1, COMBAT = 0x2
            moveType.Time = Seq;    // должно всё время увеличиваться, для нормального движения

            // Broadcasting Mobile State
            npc.BroadcastPacket(new SCOneUnitMovementPacket(npc.ObjId, moveType), true);

            // If the number of executions is less than the angle, continue adding tasks or stop moving
            if (Count < Degree)
            {
                Repeat(npc);
            }
            else
            {
                // Stop moving
                moveType.DeltaMovement[1] = 0;
                npc.BroadcastPacket(new SCOneUnitMovementPacket(npc.ObjId, moveType), true);
                //LoopAuto(npc);
                // остановиться в вершине на time секунд
                double time = (uint)Rand.Next(10, 25);
                TaskManager.Instance.Schedule(new UnitMovePause(this, npc), TimeSpan.FromSeconds(time));
            }
        }
    }
}
