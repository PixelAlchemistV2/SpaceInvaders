﻿using SpaceInvaders.CollisionSystem;
using SpaceInvaders.GraphicalObjects;

namespace SpaceInvaders.GameObjects.Player
{

    class Missile : GameObject
    {
        public Missile() : base(SpriteID.HeroShot)
        {

        }

        ~Missile()
        {

        }

        public override void Accept(ColVistor other)
        {
            
        }

        public override void cClean()
        {
           
        }
    }

}
