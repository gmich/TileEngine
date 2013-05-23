using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using Microsoft.Xna.Framework;

namespace Carcassonne.Configuration
{
    
    public sealed class DeckConfiguration
    {

        #region Declarations

        private readonly DeckManager deckManager;

        #endregion

        #region Constructor

        public DeckConfiguration(DeckManager deckManager)
        {
            this.deckManager = deckManager;
        }

        #endregion

        #region Load Methods

        public void LoadFullDeck()
        {
            if (!Directory.Exists(".\\Configuration\\TileSets"))
                Directory.CreateDirectory(".\\Configuration\\TileSets");
            if (!File.Exists(".\\Configuration\\TileSets\\tilesets.xml"))
            {
                XmlTextWriter xA = new XmlTextWriter(".\\Configuration\\TileSets\\tilesets.xml", Encoding.UTF8);
                xA.WriteStartElement("Gallery");
                xA.WriteEndElement();
                xA.Close();
            }

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(".\\Configuration\\TileSets\\tilesets.xml");

            int counter = 0; 
            int tileCounter= 0;

            foreach (XmlNode xNode1 in xDoc.SelectNodes("Gallery"))
            {

                {
                    foreach (XmlNode xNode2 in xNode1.ChildNodes)
                    {

                        if (counter <= 1)
                        {
                            deckManager.AddMasterDeck();
                            tileCounter = 0;
                        }

                        foreach (XmlNode xNode3 in xNode2.ChildNodes)
                        {
                            deckManager.AddTextureName((xNode3.SelectSingleNode("Name").InnerText), (int)MathHelper.Min(counter, 1));

                            int quantity = Convert.ToInt32(xNode3.SelectSingleNode("Quantity").InnerText);
                            for (int i = 0; i < quantity; i++)
                            {
                                deckManager.AddQuantities(tileCounter, (int)MathHelper.Min(counter, 1));
                            }
                            tileCounter++;
                        }
                        counter++;
                    }
                }
            }  
        }

        #endregion

    }
}