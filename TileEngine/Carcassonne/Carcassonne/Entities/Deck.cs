using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Carcassonne
{
    public class Deck
    {

        #region Declarations

        public List<Texture2D> textures;
        public List<int> deck;
        public List<int> backupDeck;
        public List<string> textureName;
        public Dictionary<int,int> reservedTiles;
        private List<int> UndoTiles;
        Random rand;

        #endregion

        #region Constructor

        public Deck()
        {
            textures = new List<Texture2D>();
            deck = new List<int>();
            backupDeck = new List<int>();
            textureName = new List<string>();
            rand = new Random();
            reservedTiles = new Dictionary<int, int>();
            UndoTiles = new List<int>();
            CanUndo = false;
        }

        #endregion

        #region Initialize

        public void Initialize(ContentManager content)
        {    
            foreach (string texturename in textureName)
            {
                textures.Add(content.Load<Texture2D>(texturename));
            }
            BackUpDeck();
        }

        #endregion

        #region Properties

        private bool CanUndo
        {
            get;
            set;
        }

        public bool HasReservedTiles
        {
            get
            {
                return (reservedTiles.Count > 0);
            }
        }

        public int Count
        {
            get
            {
                return deck.Count;
            }
        }

        #endregion

        #region Get / Remove Methods

        public int GetNextTile()
        {
            int nextTile = deck[0];
            UndoTiles.Add(nextTile);
            CanUndo = true;
            deck.RemoveAt(0);
            return nextTile;
        }

        public Texture2D GetNextTileTexture()
        {
            return textures[GetNextTile()];
        }

        public Texture2D GetTileTexture(int tileID)
        {
            return textures[tileID];
        }

        public void RemoveTile(int tileID)
        {
            for (int x = 0; x < deck.Count; x++)
                if (deck[x] == tileID)
                {
                    UndoTiles.Add(tileID);
                    CanUndo = true;
                    deck.RemoveAt(x);
                    return;
                }
        }

        public void UndoTileRemoval()
        {
            if (CanUndo)
            {
                List<int> tempDeck = new List<int>(deck);
                deck.Clear();
                deck.Add((UndoTiles[UndoTiles.Count - 1]));

                for (int i = 0; i < tempDeck.Count; i++)
                    deck.Add(tempDeck[i]);

                UndoTiles.RemoveAt(UndoTiles.Count - 1);
                CanUndo = false;
            }
        }

        #endregion

        #region ShuffleDeck

        public void ReserveTileAt(int x)
        {
            reservedTiles.Add(x,deck[x]);
        }

        private int GetReservedTile(int x)
        {
            if (reservedTiles.ContainsKey(x))
                return reservedTiles[x];

            return -1;
        }

        private void NewTile(ref int val)
        {
            if (reservedTiles.ContainsValue(deck[val]))
            {
                val = rand.Next(0, deck.Count);
                NewTile(ref val);
            }
        }

        public void BackUpDeck()
        {
            backupDeck = deck;
        }

        public void ShuffleDeck()
        {
            List<int> newDeck = new List<int>();
            deck = backupDeck;
            int deckSize = deck.Count;

            for (int i = 0; i < deckSize; i++)
            {
                int val = GetReservedTile(i);
                if (val != -1)
                    newDeck.Add(val);
                else
                {
                    val = rand.Next(0, deck.Count);
                    NewTile(ref val);
                    newDeck.Add(deck[val]);
                    deck.RemoveAt(val);
                }
            }
            deck = newDeck;
        }

        #endregion
    }
}
