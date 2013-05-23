using System;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Design;

namespace TileEngine.Input
{
    public class KeyboardInput
    {

        #region Declarations

        float timePassed;
        float timeToHandleInput;
        bool usesShift;
        int repCounter;

        KeyboardState mainState;
        KeyboardState prevState;

        #endregion

        #region Constructor

        public KeyboardInput()
        {
            Text = "";
            timePassed = 0.0f;
            repCounter = 0;
            timeToHandleInput = 0.6f;
            usesShift = false;
            BufferFull = false;
        }

        #endregion

        #region Properties

        public string Text { get; set; }

        public bool BufferFull { get; set; }

        #endregion

        #region Input Speed

        public void CompareValues(string input, string prevInput)
        {
            if (input == prevInput)
                repCounter++;
            else
                repCounter = 0;

            if (repCounter >= 20)
                timeToHandleInput = 0.07f;
            else if (repCounter >= 40)
                timeToHandleInput = 0.02f;
            else
                timeToHandleInput = 0.6f;
        }

        #endregion

        #region Convertion

        public string Convert(Keys key)
        {
            string output = "";

            if (key >= Keys.A && key <= Keys.Z)
                output += key.ToString();

            else if (key >= Keys.NumPad0 && key <= Keys.NumPad9)
                output += ((int)(key - Keys.NumPad0)).ToString();

            else if (key >= Keys.D0 && key <= Keys.D9)
            {
                string num = ((int)(key - Keys.D0)).ToString();

                #region special num chars

                if (usesShift)
                {
                    switch (num)
                    {
                        case "1":
                            {
                                num = "!";
                            }
                            break;
                        case "2":
                            {
                                num = "@";
                            }
                            break;
                        case "3":
                            {
                                num = "#";
                            }
                            break;
                        case "4":
                            {
                                num = "$";
                            }
                            break;
                        case "5":
                            {
                                num = "%";
                            }
                            break;
                        case "6":
                            {
                                num = "^";
                            }
                            break;
                        case "7":
                            {
                                num = "&";
                            }
                            break;
                        case "8":
                            {
                                num = "*";
                            }
                            break;
                        case "9":
                            {
                                num = "(";
                            }
                            break;
                        case "0":
                            {
                                num = ")";
                            }
                            break;
                        default:
                            break;
                    }
                }
                #endregion

                output += num;
            }

            else if (key == Keys.OemPeriod)
                output += ".";

            else if (key == Keys.OemTilde)
                output += "'";

            else if (key == Keys.Space)
                output += " ";

            else if (key == Keys.OemMinus)
                //TODO: fix underscore
                    output += "_";

            else if (key == Keys.OemPlus)
                output += "+";

            else if (key == Keys.OemQuestion)
                output += "?";

            else if (key == Keys.Back) //backspace
                output += "\b";

            if (!usesShift)
                output = output.ToLower();

            return output;
        }

        private void ConvertFirstKey(Keys[] keys, ref string str)
        {
            if (keys.Length > 0)
            {
                foreach (Keys key in keys)
                {
                    if (key.ToString() != "None")
                    {
                        str = Convert(key);
                        break;
                    }
                }
            }
        }

        #endregion

        #region Update

        public void Update(GameTime gameTime)
        {
            mainState = Keyboard.GetState();

            Keys[] key1 = mainState.GetPressedKeys();
            Keys[] key2 = prevState.GetPressedKeys();

            usesShift = (key1.Contains(Keys.LeftShift) || key1.Contains(Keys.RightShift));

            string input = "";
            string prevInput = "";

            ConvertFirstKey(key1, ref input);
            ConvertFirstKey(key2, ref prevInput);

            CompareValues(input, prevInput);

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timePassed += elapsed;

            if (((input != prevInput || timeToHandleInput <= timePassed)
                && !BufferFull) || (BufferFull && input == "\b"))
            {
                foreach (char x in input)
                {
                    //backspace
                    if (x == '\b')
                    {
                        if (Text.Length >= 1)
                        {
                            Text = Text.Remove(Text.Length - 1, 1);
                        }
                    }
                    else
                        Text += x;
                }
                timePassed = 0.0f;
            }
            prevState = mainState;

        }

        #endregion

    }
}