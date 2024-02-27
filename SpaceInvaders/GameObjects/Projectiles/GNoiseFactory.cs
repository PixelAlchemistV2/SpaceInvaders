﻿using SpaceInvaders.CollisionSystem;
using SpaceInvaders.GameState;
using SpaceInvaders.GraphicalObjects;
using SpaceInvaders.Observers;

namespace SpaceInvaders.GameObjects.Projectiles
{
    class GNoiseRoot : GameObject
    {
        public GNoiseRoot(SpriteID id, float posX = 0, float posY = 0) : base(id, posX, posY)
        {
        }

        public override void Remove()
        {
            GameObject cur = (GameObject)this.getChild();

            while (cur != null)
            {
                cur.Remove();

                cur = (GameObject)cur.getSibling();
            }
            base.Remove();
        }

        public override void Accept(ColVistor other)
        {
            other.VisitNoiseRoot(this);
        }

        public override void VisitShieldRoot(ShieldRoot shieldRoot)
        {
            Reactions.Reaction(this, shieldRoot);    
        }

        public override void cClean()
        {
        }

        public override void Update()
        {
            UpdateChildren();
            base.Update();
        }

        private void UpdateChildren()
        {
            GameObject cur = (GameObject)this.getChild();

            while (cur != null)
            {
                cur.Update();
                cur = (GameObject)cur.getSibling();
            }
            UpdateColObj();
        }

        public void UpdateColObj()
        {
            GameObject r = (GameObject)this.getChild();
            if (r != null)
            {
                CollisionRect ColTotal = this.getCollisionObject().getColRect();
                ColTotal.Set(r.getCollisionObject().getColRect());

                while (null != r)
                {
                    ColTotal.Union(r.getCollisionObject().getColRect());

                    r = (GameObject)r.getSibling();
                }

                this.getCollisionObject().getColRect().Set(ColTotal);
            }
            this.x = this.poColObj.getColRect().x;
            this.y = this.poColObj.getColRect().y;
        }



    }
    class GNoisePoint : GameObject
    {
        int count;
        public GNoisePoint(SpriteID id, float posX = 0, float posY = 0) : base(id, posX, posY)
        {
            count = 0;
        }

        public override void Accept(ColVistor other)
        {
            other.VisitNoisePoint(this);
        }

        public void ResetCounter()
        {
            count = 0;
        }

        public override void cClean()
        {

        }

        public override void Update()
        {
            count++;
            
            base.Update();
            if (3 == count)
            {
                if (!this.getMarked())
                {
                    this.markForDeath();

                    RemoveObserver pObserver = new RemoveObserver(this);
                    DelayedObjectManager.Attach(pObserver);
                }
            }
        }
    }

    class GNoiseFactory
    {
        private static GNoiseFactory pInstance;
        private GhostTypeNode pNoiseGhost;
        //GNoiseRoot pGNR;//make one for each player
        SpriteBatch pBatch;


        private GNoiseFactory()
        {
           pNoiseGhost = GhostManager.Find(GameObjectTypeEnum.Noise);
           //pGNR = new GNoiseRoot(SpriteID.NullSprite);//make this alookup of the active root
           pBatch = SpriteBatchManager.Find(SpriteBatchID.CBox);
          //  GameObjectManager.AttachTree(pGNR, new PCS.PCSTree());
        }

        public static GNoiseFactory getInstance()
        {
            if (pInstance == null)
            {
                pInstance = new GNoiseFactory();
            }
            return pInstance;
        }

        public static GNoisePoint getGNPoint()
        {
            GNoiseFactory bf = GNoiseFactory.getInstance();

            GNoisePoint toReturn;
            toReturn = (GNoisePoint)bf.pNoiseGhost.detatch();
            if (toReturn != null)
            {
                //Debug.Print("GhostManager Returned a point!\n");
            }
            if (toReturn == null)
            {
                //toReturn = new GNoisePoint(SpriteID.Undef);
                toReturn = new GNoisePoint(SpriteID.NullSprite);
                toReturn.setName(GameObjectTypeEnum.Noise);
            }
            GameObjectManager.Insert(toReturn, (GameObject)GameStateManager.getActiveNoiseRoot());
            toReturn.ResetCounter();
            return toReturn;
        }

    }
}