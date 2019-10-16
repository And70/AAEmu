﻿using AAEmu.Game.Core.Managers;
using AAEmu.Game.Core.Network.Connections;
using AAEmu.Game.Models.Game.Duels;

namespace AAEmu.Game.Models.Tasks.Duels
{
    public class DuelEndTimerTask : Task
    {
        protected Duel _duel;
        protected DuelDetType _det;
        protected uint _challengerId;

        public DuelEndTimerTask(Duel duel, uint challengerId)
        {
            _duel = duel;
            _challengerId = challengerId;
            _det = DuelDetType.Draw;
        }

        public override async void Execute()
        {
            if (_duel.DuelEndTimerTask == null)
                return;

            await _duel.DuelEndTimerTask.Cancel();
            _duel.DuelEndTimerTask = null;
            DuelManager.Instance.DuelStop(_challengerId, _det);
        }
    }
}
