using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static ReFactory.GameScripts.Machines.ConveyorBelt.BeltParams;

using Debug = ObscurusDebuggerTools.ObscurusDebugger;

/*
=======Thoughts=======
Instead of just rotating a placable entity with 'R', is it possible to use 'R' as a 'primer' 
in which you hold 'R' and use '->' or '<-' to rotate 'CW' or 'CCW' respectively?
======================

 
 */

namespace ConveyorDirection
{
    public enum Direction
    {
        North, // Dir 0
        South, // Dir 1
        East, // Dir 2
        West // Dir 3
    }
}

namespace ReFactory.GameScripts.Machines.ConveyorBelt
{
    public class BeltParams
    {
        public struct Belt
        {
            #region BeltState & Cell Ref
/* 
=================BeltState & Cell Reference====================

============Letter Abbreviation Legend=========================
            N = North
            S = South
            W = West
            E = East
            TL = Top Left // *Cell of belt*
            TR = Top Right // *Cell of belt*
            BL = Bottom Left // *Cell of belt*
            BR = Bottom Right // *Cell of belt*
===============================================================

            Belt Direction States: // 12 Belt States

                                        TL - TR
                        Cell Example:   |     |
                                        BL - BR

                Vertical Belt:
                    N, S = 0
                    S, N = 1

                Horizontal Belt:
                    W, E = 2
                    E, W = 3

                Right Hand Belt Turn:
                    N, W = 4
                    W, S = 5
                    S, E = 6
                    E, N = 7

                Left Hand Belt Turn:
                    N, E = 8
                    E, S = 9
                    S, W = 10
                    W, N = 11

            Cell Translations:

            Vertical Belt: [ Direction = N, S || S, N ]
                N, S Line: TL -> BL && TR -> BR
                S, N Line: BL -> TL && BR -> TR

            Horizontal Belt: [ Direction = W, E || E, W ]
                W, E Line: TL -> TR && BL -> BR
                E, W Line: TR -> TL && BR -> BL

        Curved Belts:
            Right Hand Belt Turn: [ Direction = N, W || W, S || S, E || E, N ]

                                        TL - TR
                        Cell Example:   |     |
                                        BL - BR
                N, W Turn:
                    Single Cell: TL 
                        Three Cell: TR -> BR -> BL

                W, S Turn:
                    Single Cell: BL, 
                        Three Cell: TL -> TR -> BR

                S, E Turn: 
                    Single Cell: BR, 
                        Three Cell: BL -> TL -> TR
                E, N Turn:
                    Single Cell: TR
                        Three Cell: BR -> BL -> TL


            Left Hand Belt Turn: [ Direction = N, E || E, S || S, W || W, N ]

                                        TL - TR
                        Cell Example:   |     |
                                        BL - BR

                N, E Turn:
                    Single Cell: TR, 
                        Three Cell: TL -> BL -> BR
                E, S Turn:
                    Single Cell: BR, 
                        Three Cell: TR -> TL -> BL
                S, W Turn:
                    Single Cell: BL, 
                        Three Cell: BR -> TR -> TL
                W, N Turn:
                    Single Cell: TL
                        Three Cell: BL -> BR -> TR
*/
            #endregion

            public String BeltID; // --> (ConveyorBeltType + "Pos: " + position)
            public String ConveyorBeltType { get; private set;} // --> Basic, Quick, Hyper
            Vector2 cellPosition; // 'this' Belts grid cell position.
            Vector2 position; // 'this' Belts world position.
            private Func<Texture2D> _getSprite = () => GameContent.StraightConveyorBelt.GetSprite();
            public Texture2D sprite => _getSprite();
            public HashSet<Point> OccupiedRelativePoints => new() { new Point(0, 0) };
            public float DestroyTime => 0.1f;
            public Func<bool> OnDestroy => () => Inventory.AddItem(new Item("Belt", 1));
            public List<Belt> neighbor; // Neighbors are belts in the World Cell directly next to 'this' belt, whether or not it is connected.

            BeltEntity[] _leftSideEntity; // List of Entities on the left side of the belt.
            BeltEntity[] _rightSideEntity; // List of Entities on the right side of the belt.

            Dictionary<BeltEntityCell, Entity[]> TLCell; // Top left cell
            Dictionary<BeltEntityCell, Entity[]> TRCell; // Top right cell
            Dictionary<BeltEntityCell, Entity[]> BLCell; // Bottom left cell
            Dictionary<BeltEntityCell, Entity[]> BRCell; // Bottom right cell
            public int BeltState; // Current State of 'this' Belt.

            public Belt(string conveyorBeltType, Vector2 cellPosition, Vector2 position, int beltState)
            {
                BeltID = conveyorBeltType + "Pos: " + position;
                ConveyorBeltType = conveyorBeltType;
                this.cellPosition = cellPosition;
                this.position = position;
                _leftSideEntity = new BeltEntity[0];
                _rightSideEntity = new BeltEntity[0];
                TLCell = new Dictionary<BeltEntityCell, Entity[]>();
                TRCell = new Dictionary<BeltEntityCell, Entity[]>();
                BLCell = new Dictionary<BeltEntityCell, Entity[]>();
                BRCell = new Dictionary<BeltEntityCell, Entity[]>();
                neighbor = new List<Belt>();
                _getSprite = () => GameContent.StraightConveyorBelt.GetSprite();
                BeltState = beltState;
            }

        }

        public struct BeltEntity
        {

            public float progress;  //this should be the time it takes to move 1 "_beltEntityCell"
            public Entity entity; // Items/Entities that can occupy a _beltEntityCell


            public bool Progress(float deltaTime) // **Needs Implemented**
            {
                return true;
            }
        }

        public struct BeltEntityCell
        {
            public String TopLeft, TopRight, BotLeft, BotRight;
            public int TL, TR, BL, BR;
            public Entity[] beltItem;

            public override bool Equals([NotNullWhen(true)] object obj)
            {
                return base.Equals(obj);
            }

            public override int GetHashCode()
            {
                Debug.Log(GetHashCode);
                return base.GetHashCode();
            }

            public override string ToString()
            {
                return base.ToString();
            }

            public static bool operator ==(BeltEntityCell left, BeltEntityCell right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(BeltEntityCell left, BeltEntityCell right)
            {
                return !(left == right);
            }
        }

        public void MoveEntities(/*/--> Belt/CellPosition, BeltState, _leftSideEntity, _rightSideEntity, neighbor, TLCell, TRCell, BLCell, BRCell <--/*/)
        {
            // Get 'Belt/CellPosition' && 'BeltState'
            // Get Left & Right Entities
            // Get Neighbor

            #region BeltState Ref
            /*
                        Turn-based Organization:

                            Vertical:
                                N, S = 0
                                S, N = 1

                            Horizontal:
                                W, E = 2
                                E, W = 3

                            Right Hand Turn:
                                N, W = 4
                                W, S = 5
                                S, E = 6
                                E, N = 7

                            Left Hand Turn:
                                N, E = 8
                                E, S = 9
                                S, W = 10
                                W, N = 11

                        Cardinal-based Organization:

                            North:
                                N, S = 0 //Straight
                                N, W = 4 //Right
                                N, E = 8 //Left
                            South:
                                S, N = 1 //Straight
                                S, E = 6 //Right
                                S, W = 10 //Left
                            West:
                                W, E = 2 //Straight
                                W, S = 5 //Right
                                W, N = 11 //Left
                            East:
                                E, W = 3 //Straight
                                E, N = 7 //Right
                                E, S = 9 //Left
            */
            #endregion

        }
    }
}

namespace ReFactory.GameScripts.Machines.ConveyorBelt.ConveyorUtilities
{
    class MasterBeltHandler // Handles Global Belt States
    {
        public static List<Belt> AllBelts = new(); // List for storing all belts placed on the grid
        public static List<List<Belt>> Stream = new(); // List of belts connected in a chain from 1 to 'X'

        private static void BuildStream()
        {
            // After writing I realized that streams will likely be built when a new Belt is placed, so Version 1 Steps can be removed, along with this note, After Version 2 is implemented.
            #region Version 1 Build Stream Notes
            // Iterate through 'AllBelts' by Cell Position
            // ** For belts with 0/'Zero' Connected Neighbors, Create New Stream *Single Stream*
            // For belt with Connected Neighbor & Not in a Stream, Create New Stream // Increment cellMarker to the next cell
            // Add Neighbor to Stream, Then Check for Neighbor
            // Repeat until end of Stream, then ( cellChecker = cellMarker ) to continue
            // For belts that are part of a stream, skip to next cell
            #endregion 

            Vector2 cellChecker; // Current Cell to be checked
            Vector2 cellMarker; // First Cell to be checked after following neighbor stream
        }


        public MasterBeltHandler()
        {
            AllBelts = new List<Belt>();
        }

        public void AddBelt(Belt belt)
        {
            AllBelts.Add(belt);
        }

        public void RemoveBelt(Belt belt)
        {
            AllBelts.Remove(belt);
        }

    }
}