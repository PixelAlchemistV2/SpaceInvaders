﻿using SpaceInvaders.CollisionSystem;

namespace SpaceInvaders.GameState
{
    class PlayerDeathObserver : ColObserver
    {
        public override void dClean()
        {

        }

        public override void Notify()
        {
            
        }
    }

    internal sealed class PlayerLoseLifeObserver : ColObserver
    {
        public override void dClean()
        {
           
        }

        public override void Notify()
        {
            GameStateManager.LoseLife();
        }
    }
}