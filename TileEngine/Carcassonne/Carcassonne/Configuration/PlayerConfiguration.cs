using System;
using System.Xml;
using System.Text;
using System.IO;

namespace Carcassonne.Configuration
{
    public sealed class PlayerConfiguration
    {
        #region Properties

        public string Name
        {
            get;
            set;
        }

        public bool LoadAvatar
        {
            get;
            set;
        }

        public int ColorID
        {
            get;
            set;
        }

        #endregion

        #region Constructor

        public PlayerConfiguration()
        {
            try
            {
                if (!File.Exists(".//Configuration//PlayerConfiguration.xml"))
                    CreateDefaultXML();

                LoadFromXML();
             
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //TODO:Handle Exception here
            }
        }

        #endregion

        public void Dispose()
        {
            SaveToXML();
        }

        #region Create Default XML File

        public void CreateDefaultXML()
        {
            using (XmlWriter writer = XmlWriter.Create(".//Configuration//PlayerConfiguration.xml"))
            {
                writer.WriteStartDocument();
                writer.WriteWhitespace("\n");
                writer.WriteStartElement("PlayerInformation");
                writer.WriteWhitespace("\n\t");
                writer.WriteElementString("Name", "Player");
                writer.WriteWhitespace("\n\t");
                writer.WriteElementString("LoadAvatar", "False");
                writer.WriteWhitespace("\n\t");
                writer.WriteElementString("Color", "2");
                writer.WriteWhitespace("\n");

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        #endregion

        #region Load/Save

        public void LoadFromXML()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(".//Configuration//PlayerConfiguration.xml");

            foreach (XmlNode xNode in xDoc.SelectNodes("PlayerInformation"))
            {
                Name = xNode.SelectSingleNode("Name").InnerText;
                if (xNode.SelectSingleNode("LoadAvatar").InnerText == "True")
                    LoadAvatar = true;
                else
                    LoadAvatar = false;

                ColorID = System.Convert.ToInt32(xNode.SelectSingleNode("Color").InnerText);
            }
        }

        public void SaveToXML()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(".//Configuration//PlayerConfiguration.xml");
            XmlNode xNode = xDoc.SelectSingleNode("PlayerInformation");
            xNode.RemoveAll();

            XmlNode xName = xDoc.CreateElement("Name");
            XmlNode xLoadAvatar = xDoc.CreateElement("LoadAvatar");
            XmlNode xColor = xDoc.CreateElement("Color");
            xName.InnerText = Name;
            xLoadAvatar.InnerText = LoadAvatar.ToString();
            xColor.InnerText = ColorID.ToString();
            xNode.AppendChild(xName);
            xNode.AppendChild(xLoadAvatar);
            xNode.AppendChild(xColor);

            xDoc.Save(".//Configuration//PlayerConfiguration.xml");
        }

        #endregion
        
    }
}
