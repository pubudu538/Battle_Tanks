using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Game_V1._2
{
    public partial class Form1 : Form
    {
        String serverIp;
        String clientIp;
        int serverportNumber;
        int mapSize;
        int clientport;        

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            setValues();

            System.Threading.Thread thStartGame = new System.Threading.Thread(startGame);
            thStartGame.Start();
            Dispose();
        }
        private void setValues()
        {
            serverIp = serverIPtb.Text;
            clientIp = clientIPtb.Text;
            serverportNumber = Convert.ToInt32(serverPorttb.Text);
            clientport = Convert.ToInt32(clientporttb.Text);
            mapSize = Convert.ToInt32(mapsizetb.Text);

        }

        private void startGame()
        {
            Game1 game = new Game1(serverIp,clientIp, serverportNumber, clientport, mapSize);
            game.Run();

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void mapsizetb_TextChanged(object sender, EventArgs e)
        {

        }

        private void clientporttb_TextChanged(object sender, EventArgs e)
        {

        }

        private void serverIPtb_TextChanged(object sender, EventArgs e)
        {

        }

        private void serverPorttb_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
