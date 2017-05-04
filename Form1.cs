using System;
using System.Diagnostics;
using System.Windows.Forms;
using WebSocket4Net;
using System.Drawing;

namespace SweetSweetGoodnight
{
    public partial class SweetSweetGoodnight : Form
    {
        private int standartTime = 60;
        private int time = 0;
        private Boolean temporizadorActivo = false;

        public SweetSweetGoodnight()
        {
            InitializeComponent();
        }
        
        static WebSocket Client;

        private void connect_Click(object sender, EventArgs e)
        {
            if (connect.Text.Equals("Desconectar"))
            {
                Client.Close();
                connect.Text = "Conectar";
                this.connect.BackColor = Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            }
            else if (connect.Text.Equals("Conectar"))
            {
                var url = txbIp.Text;
                Client = new WebSocket("ws://" + url);
                Client.Opened += new EventHandler(Client_Opened);
                Client.Error += new EventHandler<SuperSocket.ClientEngine.ErrorEventArgs>(Client_Error);
                Client.Closed += new EventHandler(Client_Closed);
                Client.MessageReceived += new EventHandler<MessageReceivedEventArgs>(Client_MessageReceived);

                Client.Open();
                connect.Text = "Desconectar";
                connect.BackColor = Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            }
        }
        
        void Client_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            Console.WriteLine("Message received : " + e.Message);
            if(e.Message.Equals("Goodnight"))
            {
                temporizadorActivo = true;
                Process.Start("shutdown", "/s /f");
            }
        }

        void Client_Closed(object sender, EventArgs e)
        {
            Console.WriteLine("Websocket closed");
        }

        void Client_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
            Console.WriteLine("Error thrown trying to open websocket : " + e.Exception.Message);
        }

        void Client_Opened(object sender, EventArgs e)
        {
            Console.WriteLine("Websocket connection open.");
            Client.Send("Hi!! (Windows Version)");
        }

        private void abort_Click(object sender, EventArgs e)
        {
            temporizadorActivo = false;
            time = 0;
            lblTemporizador.Text = "Temporizador: 0";
            Process.Start("shutdown", "/a");
        }

        private void temporizador_Tick(object sender, EventArgs e)
        {
            if (temporizadorActivo)
            {
                time++;
                lblTemporizador.Text = "Temporizador: " + (standartTime-time);
            }
        }
    }
}
