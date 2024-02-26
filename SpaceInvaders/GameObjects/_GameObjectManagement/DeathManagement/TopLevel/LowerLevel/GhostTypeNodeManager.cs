﻿using SpaceInvaders.Manager;
using System.Diagnostics;

namespace SpaceInvaders.GameObjects
{
    class GhostTypeNodeManager : GTNMan
    {
        private GameObjectTypeEnum Name;
        private GhostNode poSpriteBatchRef = new GhostNode();

        public GhostTypeNodeManager()
        {
            Name = GameObjectTypeEnum.Undef;
        }
        public GhostTypeNodeManager(GameObjectTypeEnum name)
        {
            Name = name;
        }

        public void Attach(GameObject toAttach)
        {
            GhostNode pNode = (GhostNode)this.baseAdd();
            pNode.Set(toAttach);
        }
        //badsmell multiple return
        public GameObject Detatch()
        {
            GhostNode toReturn = (GhostNode)getActiveHead();
            if (toReturn != null)
            {
                baseRemove(toReturn);
                return toReturn.getGameObject();
            }

            return null;
        
        }

        //bad smell, doesn't do anything currently.
        protected override void dClearNode(DLink pLink)
        {
        }

        protected override bool dCompareNodes(DLink pLinkA, DLink pLinkB)
        {
            Debug.Assert(pLinkA != null);
            Debug.Assert(pLinkB != null);

            GhostNode left = (GhostNode)pLinkA;
            GhostNode right = (GhostNode)pLinkB;

            if (left.getName() == right.getName())
            {
                return true;
            }
            return false;
        }

        internal void Set(GameObjectTypeEnum id)
        {
            Name = id;   
        }

        protected override DLink dCreateNode()
        {
            DLink newNode = new GhostNode();
            Debug.Assert(newNode != null);

            return newNode;
        }

    }

}