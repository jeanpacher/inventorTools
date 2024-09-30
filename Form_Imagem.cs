using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bosch_ImportData
{
    public partial class Form_Imagem : Form
    {
        public Form_Imagem()
        {
            InitializeComponent();
        }
        
        

        private void Form_Imagem_Load(object sender, EventArgs e)
        {
            
        }

        public void imagem(Image image)
        {
            pictureBox1.Image = image;
        }

    }
}
