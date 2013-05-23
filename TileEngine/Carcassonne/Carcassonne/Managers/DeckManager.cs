using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Carcassonne.Configuration;

namespace Carcassonne
{
    public class DeckManager
    {

        #region Declarations

        public List<Deck> decks;
        private DeckConfiguration deckConfiguration;
        private ContentManager Content;
        #endregion

        #region Constructor

        public DeckManager(ContentManager Content)
        {
            this.Content = Content;
        }

        #endregion

        #region Initialize

        public void Initialize()
        {
            foreach (Deck deck in decks)
            {
                deck.Initialize(Content);
            }
        }

        #endregion

        #region NewGame()

        public void NewGame()
        {
            decks = new List<Deck>();
            deckConfiguration = new DeckConfiguration(this);
            deckConfiguration.LoadFullDeck();

            Initialize();
        }

        #endregion

        #region Properties

        public int CountAll
        {
            get
            {
                int deckSum = 0;
                foreach (Deck deck in decks)
                    deckSum += deck.Count;
                return deckSum;
            }
        }

        public bool AreTilesLeft
        {
            get
            {
                return (CountAll > 0);
            }  
        }

        #endregion
        
        #region Configure Deck Methods

        public void AddMasterDeck()
        {
            decks.Add(new Deck());
        }

        public void AddTextureName(string name, int deckID)
        {
            decks[deckID].textureName.Add(name);
        }

        public void AddQuantities(int quantity, int deckID)
        {
            decks[deckID].deck.Add(quantity);
        }

        #endregion

        #region Get / Remove Tiles

        public Texture2D GetNextTileTexture()
        {
            ValidateDecks();
            return decks[0].GetNextTileTexture();
        }

        public int GetNextTile()
        {
            ValidateDecks();
            return decks[0].GetNextTile();
        }

        public void ValidateDecks()
        {
            if (decks[0].Count == 0)
                decks.RemoveAt(0);
        }

        public Texture2D GetTileTexture(int tileID)
        {
            return decks[0].GetTileTexture(tileID);
        }

        public void RemoveTile(int tileID)
        {
            decks[0].RemoveTile(tileID);
        }

        public void UndoTileRemoval()
        {
            decks[0].UndoTileRemoval();
        }

        #endregion

        #region Shuffle Decks

        public void ShuffleRiver(int x)
        {
            decks[x].ReserveTileAt(0);
            decks[x].ReserveTileAt(11);
            decks[x].BackUpDeck();

            decks[x].ShuffleDeck();
        }

        public void ShuffleBase(int x)
        {
           // decks[x].ReserveTileAt(0);
            decks[x].BackUpDeck();

            decks[x].ShuffleDeck();
        }

        #endregion

    }
}
