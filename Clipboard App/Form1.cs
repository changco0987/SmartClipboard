using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace Clipboard_App
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //To avoid the windows form from flickering
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams handleParams = base.CreateParams;
                handleParams.ExStyle |= 0x02000000;
                return handleParams;
            }
        }

        private string folderPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Save txt");
        private int fileCount = 1;

        private void fileCreation()
        {
            /*
             * This part of the program will check all the file/txt name exist in the saved file directory
             * to avoid replacing the old one
             * To check each the existing file name to avoid data lost or overwritten and merging of data
            */
            var getFilePath = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).ToString() + @"\Save txt");
            foreach (var file in getFilePath)
            {
                string fileName = System.IO.Path.Combine(folderPath, @"\Text " + fileCount + ".txt");
                string[] getFileName = file.Split('\\');
                if (getFileName[getFileName.Length - 1].Equals(fileName.TrimStart('\\')))
                {
                    //Increment the fileCount and repeat the checking
                    fileCount++;
                }
                else
                {
                    /*
                     * To exit the foreach loop iteration 
                     * if theres an intance that sufficient fileCount file created
                     * Example
                     * Text1.txt
                     * Text3.txt
                     * then it will create a file base on this lost number 
                     * in this case the Text2.txt file is being created
                     */
                    continue;
                }

            }

            //Creating and saving the user input
            FileStream input = new FileStream(folderPath + @"\Text " + fileCount + ".txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            StreamWriter txtBoxInput = new StreamWriter(input);
            txtBoxInput.Write(textBox3.Text);
            txtBoxInput.Close();
            input.Close();
            textBox3.Text = "";
            panelRefresh();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

         void panelRefresh()
        {
            string fileName = "";
            string getAllFileName = "";
            textBox1.Text = "";//To erase any previous text for future panel refresh
            //To load the previous file count
            var getFilePath = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).ToString() + @"\Save txt");

            foreach (var file in getFilePath)
            {
                string[] getFileName = file.Split('\\');
                //This will remove the file extension of the file (.txt) and return only the file name
                fileName = getFileName[getFileName.Length - 1].Remove(getFileName[getFileName.Length - 1].IndexOf("."));
                getAllFileName = getAllFileName + "," +fileName;

            
            }
            //To get all file new from getAllFileName variable and initialized it in an array of string
            string[] allFileName = getAllFileName.Split(',');

            //This will sort all the Text file numbers (Ascending)
            for (int count = 1;count<allFileName.Length;count++) 
            {
                int lastCountValue = count;//To be able to get the and continue to the previous count value
                int fileNumber = Convert.ToInt32(allFileName[count].Remove(0, 4));
                for (int check = count+1;check<allFileName.Length;check++) 
                {
                    if (fileNumber > Convert.ToInt32(allFileName[check].Remove(0, 4))) 
                    {
                        //It will hold the value that nessecary to swap it's position
                        string numberExchange = allFileName[count];
                        //This will swap the lower value into the lower index
                        allFileName[count] = allFileName[check];
                        //This will swap the higher value into the higher index
                        allFileName[check] = numberExchange;
                        count++;
                    }
                }
                count = lastCountValue;
            }

            //This will make all sorted file name appears in the panel
            for (int count = 1;count<allFileName.Length;count++) 
            {
                textBox1.Text = textBox1.Text + allFileName[count] + Environment.NewLine;
            }



        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //it will refresh the app immediately once it's launch/open
            panelRefresh();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void saveBTN_Click_1(object sender, EventArgs e)
        {

            //It checks whether program has input or null
            if (string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox message = new MessageBox();
                message.Show();
            }
            else if (textBox3.Text != null)
            {
                /*
                 * It will create a folder to the documents path
                 * that your app will used to store your txt files
                 */
                
                System.IO.Directory.CreateDirectory(folderPath);
                fileCreation();

            }
        }
    }
}
