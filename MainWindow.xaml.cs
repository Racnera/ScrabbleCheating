///
///Ethan Hunter, 314243
///4/23/2019
///make a program that tells you all possible words that can be created with letters given.
///create annograms.
///
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
namespace _314243_ScrabbleCheating
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int steps = 0;
        string tiles = "";
        public MainWindow()
        { 
            InitializeComponent(); 
            string check = "";
            StreamReader sr = new StreamReader("Words.txt");//your list of words
            StreamWriter wr = new StreamWriter("NewWords.txt"); //new list of words
            while (!sr.EndOfStream)
            {
                
                string word = sr.ReadLine().ToUpper(); //old word to uppercase
                if ((word.Length < 8)&& (word!=check)&&(word.Length!=1)) //if the old word can be made with our seven tiles, and scrabble doesnt allow one letter words
                {
                                              //write new word, uppercase and possible to be made
                    wr.WriteLine(" " +word ); //space before is for nested loops later, so that i&j at 0 are not going to be letters
                    
                }
                check = word; //make sure the same word is not in the text file twice.
            }
            wr.Flush();
            wr.Close();
            sr.Close();

            string output = "";//output string is added to later 
            ScrabbleGame sg = new ScrabbleGame();
            tiles = sg.drawInitialTiles(); // gets the tiles from the random seed in ScrabbleGame.cs ; weighted like actuall scrabble game
            tiles = "  zjqxv";
            //MessageBox.Show(tiles);           //see the tiles in your hand before the program shows all possible words
            preface.Content += tiles + "\" you can make the words: \n"; 
            StreamReader reader = new StreamReader("NewWords.txt");

            
            DateTime start = new DateTime();
            DateTime end = new DateTime();
            //MessageBox.Show("start Timer");
            start = DateTime.Now;
            while (!reader.EndOfStream)
            {
                string word = reader.ReadLine(); 
                List<string> inHand = new List<string>();
                List<string> letters = new List<string>();
                int letterhold =7; //length set here so that you always start with 7 letters/word-checked, this is changed later.
                int wordhold = word.Length; //
                int n = 1;
                int counter = 0;
                for (int i = 0; i < 7; i++)
                {
                    if(tiles[i]==' ')//make sure it's not a blank tile
                    {
                        n++;
                        counter++;
                    }
                    if (tiles[i] != ' ')
                    {
                        inHand.Add(tiles[i].ToString());
                    } //these are the tiles you have added to a list   
                }
                for (int i = 0; i < word.Length; i++)
                {
                    letters.Add(word[i].ToString());            // these are the letters that make up the word in a list
                }  
 

                for (int i=0; i<wordhold; i++) //loops through the letters in the word
                {
                    for (int j=0; j<letterhold-counter; j++) //loops through letters in hand
                    {

                        //remove both items from list if letters[i] == inHand[j],  for blank tiles, create a counter that allows left over letters in list<letters>
                        if (letters[i] == inHand[j]) //if the letter in hand is the same as the letter in the word (at their respective positions):
                        {
                            letters.RemoveAt(i); //remove both from list
                            inHand.RemoveAt(j); //    ||
                            letterhold--;//shrink loop length because you have less letters in the list
                            wordhold--;// ||
                            i--;
                            j--;//since the list removes at poistion i, characters in positions greater than 'i' move down one 
                                //(characters in position 'i+1' becomes 'i', 'i+2' becomes 'i+1' etc.) 
                                //this puts i and j to their previous values, which have already been checked. 
                                //that will either be the 'SPACE' at the beginning of the word, or the letter before.
                                //this stops the program from skipping letter checks.
                                //another way this could be done is to keep i--, and instead set j equal to 0.
                        }
                        if (i < 0) { i = 0; } // since we subtract from i earlier, this makes sure that i!<0
                                              // this prevents out of bounds errors
                        if (j < 0) { j=0; }//above, but with j
                    }        
                }
                 //MessageBox.Show(letters.Count.ToString());
               // MessageBox.Show(n.ToString());
                if (letters.Count <= n)
                {

                    output += word +'\n';
                    steps++;
                    
                } //puts word on output if letters in hand can make the word.
                if (letters.Count != n)
                {
                    //removes all letters from list before moving onto next word
                    letters.RemoveRange(0,letters.Count);                  
                }
               
                
            
            }// end while
            reader.Close();
            end = DateTime.Now;
            //MessageBox.Show("endtimer");
            TimeSpan dif = end.Subtract(start);
            time.Content += (dif.TotalMilliseconds.ToString()) + " milliseconds"; //timing if you're into that.
            lblOut.Text = output;
           // MessageBox.Show(steps.ToString());
        }


    }
}