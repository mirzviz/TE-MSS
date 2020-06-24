using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TE_MSS;

namespace TEdemo
{
    public partial class Form1 : Form
    {
        private TE_MSS_impl m_TEMSS = new TE_MSS_impl();
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonAddToMap_Click(object sender, EventArgs e)
        {
            m_TEMSS.initializeTEMss();
            //m_TEMSS.CreateMssObject("<Symbol ID=\"SFGPUCI----F---\"><Attribute ID=\"M\">2</Attribute><Attribute ID=\"T\">1</Attribute></Symbol>", -122.49460, 37.78816);
            //m_TEMSS.CreateMssObjectWithInsertPoint("<Symbol ID=\"SFGPUCI----F---\"><Attribute ID=\"M\">2</Attribute><Attribute ID=\"T\">1</Attribute></Symbol>", -122.49460, 37.78816);
            //TODO: get N Point strings
            /*m_TEMSS.CreateNPointObject("<Symbol ID=\"SFGPUCI----F---\"><Attribute ID=\"M\">2</Attribute><Attribute ID=\"T\">1</Attribute></Symbol>", -122.49460, 37.78816);*/
            /*m_TEMSS.CreateNPointObject("<Symbol ID=\"SFGPUSMM--AF---\"><Attribute ID=\"XE\">9SCHUNB--------</Attribute></Symbol>", -122.49460, 37.78816);*/

            //3 points
            //m_TEMSS.CreateNPointObject("<Symbol ID=\"GFTPB----------\"/>", -122.49460, 37.78816); 

            m_TEMSS.CreateNPointObject("<Symbol ID=\"GFGPGLB----I---\"><Attribute ID=\"T\">a</Attribute><Attribute ID=\"T1\">b</Attribute></Symbol>", -122.49460, 37.78816);
        }
    }
}
