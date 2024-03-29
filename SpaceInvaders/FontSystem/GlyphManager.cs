﻿using SpaceInvaders.Manager;
using System.Diagnostics;
using SpaceInvaders.GraphicalObjects;
using System.Xml;

namespace SpaceInvaders.FontSystem
{
    internal sealed class GlyphManager : GMan
    {
        //----------------------------------------------------------------------
        // Constructor
        //----------------------------------------------------------------------
        private GlyphManager(int reserveNum = 3, int reserveGrow = 1)
            : base(reserveNum, reserveGrow)
        {
            pRefNode = (Glyph)dCreateNode();
        }
        ~GlyphManager()
        {
#if(TRACK_DESTRUCTOR)
            Debug.WriteLine("~GlyphManager():{0}", this.GetHashCode());
#endif
            pRefNode = null;
            GlyphManager.pInstance = null;
        }

        //----------------------------------------------------------------------
        // Static Manager methods can be implemented with base methods 
        // Can implement/specialize more or less methods your choice
        //----------------------------------------------------------------------
        public static void Create(int reserveNum = 3, int reserveGrow = 1)
        {
            // make sure values are ressonable 
            Debug.Assert(reserveNum > 0);
            Debug.Assert(reserveGrow > 0);

            // initialize the singleton here
            Debug.Assert(pInstance == null);

            // Do the initialization
            if (pInstance == null)
            {
                pInstance = new GlyphManager(reserveNum, reserveGrow);
            }
        }
//        public static void Destroy()
//        {
//            // Get the instance
//            GlyphManager pMan = GlyphManager.privGetInstance();
//#if(TRACK_DESTRUCTOR)
//            Debug.WriteLine("--->GlyphManager.Destroy()");
//#endif
//            pMan.baseDestroy();
//        }
        public static Glyph Add(Glyph.Name name, int key, TextureID textName, float x, float y, float width, float height)
        {
            GlyphManager pMan = GlyphManager.privGetInstance();

            Glyph pNode = (Glyph)pMan.BaseAdd();
            Debug.Assert(pNode != null);

            pNode.Set(name, key, textName, x, y, width, height);
            return pNode;
        }

        public static void AddXml(Glyph.Name glyphName, string assetName, TextureID textName)
        {
            XmlTextReader reader = new XmlTextReader(assetName);

            int key = -1;
            int x = -1;
            int y = -1;
            int width = -1;
            int height = -1;

            // I'm sure there is a better way to do this... but this works for now
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element: // The node is an element.
                        if (reader.GetAttribute("key") != null)
                        {
                            key = Convert.ToInt32(reader.GetAttribute("key"));
                        }
                        else if (reader.Name == "x")
                        {
                            while (reader.Read())
                            {
                                if (reader.NodeType == XmlNodeType.Text)
                                {
                                    x = Convert.ToInt32(reader.Value);
                                    break;
                                }
                            }
                        }
                        else if (reader.Name == "y")
                        {
                            while (reader.Read())
                            {
                                if (reader.NodeType == XmlNodeType.Text)
                                {
                                    y = Convert.ToInt32(reader.Value);
                                    break;
                                }
                            }
                        }
                        else if (reader.Name == "width")
                        {
                            while (reader.Read())
                            {
                                if (reader.NodeType == XmlNodeType.Text)
                                {
                                    width = Convert.ToInt32(reader.Value);
                                    break;
                                }
                            }
                        }
                        else if (reader.Name == "height")
                        {
                            while (reader.Read())
                            {
                                if (reader.NodeType == XmlNodeType.Text)
                                {
                                    height = Convert.ToInt32(reader.Value);
                                    break;
                                }
                            }
                        }
                        break;

                    case XmlNodeType.EndElement: //Display the end of the element 
                        if (reader.Name == "character")
                        {
                            // have all the data... so now create a glyph
                          //  Debug.WriteLine("key:{0} x:{1} y:{2} w:{3} h:{4}", key, x, y, width, height);
                            GlyphManager.Add(glyphName, key, textName, x, y, width, height);
                        }
                        break;
                }
            }

            // Debug.Write("\n");
        }

        public static void Remove(Glyph pNode)
        {
            Debug.Assert(pNode != null);
            GlyphManager pMan = GlyphManager.privGetInstance();
            pMan.baseRemove(pNode);
        }
        public static Glyph Find(Glyph.Name name, int key)
        {
            GlyphManager pMan = GlyphManager.privGetInstance();

            // Compare functions only compares two Nodes
            pMan.pRefNode.name = name;
            pMan.pRefNode.key = key;

            Glyph pData = (Glyph)pMan.BaseFind(pMan.pRefNode);
            return pData;
        }

        //public static void Dump()
        //{
        //    GlyphManager pMan = GlyphManager.privGetInstance();
        //    Debug.Assert(pMan != null);

        //    Debug.WriteLine("------ Glyph Manager ------");
        //    pMan.baseDump();
        //}
        //----------------------------------------------------------------------
        // Override Abstract methods
        //----------------------------------------------------------------------
        override protected bool dCompareNodes(DLink pLinkA, DLink pLinkB)
        {
            // This is used in baseFind() 
            Debug.Assert(pLinkA != null);
            Debug.Assert(pLinkB != null);

            Glyph pDataA = (Glyph)pLinkA;
            Glyph pDataB = (Glyph)pLinkB;

            return pDataA.name == pDataB.name && pDataA.key == pDataB.key;
        }

        override protected DLink dCreateNode()
        {
            DLink pNode = new Glyph();
            Debug.Assert(pNode != null);
            return pNode;
        }

        override protected void dClearNode(DLink pLink)
        {
            Debug.Assert(pLink != null);
            Glyph pNode = (Glyph)pLink;
            pNode.dClean();
        }

        //override protected void derivedDumpNode(MLink pLink)
        //{
        //    Debug.Assert(pLink != null);
        //    Glyph pNode = (Glyph)pLink;

        //    Debug.Assert(pNode != null);
        //    pNode.Dump();
        //}

        //----------------------------------------------------------------------
        // Private methods
        //----------------------------------------------------------------------
        private static GlyphManager privGetInstance()
        {
            // Safety - this forces users to call Create() first before using class
            Debug.Assert(pInstance != null);
            return pInstance;
        }

        //----------------------------------------------------------------------
        // Data
        //----------------------------------------------------------------------
        private static GlyphManager pInstance;
        private Glyph pRefNode;
    }
}
