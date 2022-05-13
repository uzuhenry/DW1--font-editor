using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using Microsoft.Win32;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool fileIsLoaded = false;
        public string archivoabiegto = "";
        public int lastlitsboxindex = 0;
        public MainWindow()
        {
            InitializeComponent();
            
        }
        public class MyButton : Button
        {
            public bool encendido { get; set; }
            
        }

        private void botonLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "1bpp font files (*.1bppf)|*.1bppf";
            if (openFileDialog.ShowDialog() == true)
            {
                listascroll.Items.Clear();
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                Encoding jenc = Encoding.GetEncoding(932);
                string hex = "";
                string flex = "";
                int countercharer = 0;
                fileIsLoaded = true;
                archivoabiegto = openFileDialog.FileName + "-temp";
                File.Copy(openFileDialog.FileName, archivoabiegto,true);   
                using (FileStream fs = new FileStream(archivoabiegto, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader br = new BinaryReader(fs, new ASCIIEncoding()))
                    {
                        byte[] chunk;
                        chunk = br.ReadBytes(2);
                        Array.Reverse(chunk);
                                        
                        while (chunk.Length > 0 & hex != "0000")
                        {
                            hex = BitConverter.ToString(chunk).Replace("-", string.Empty);
                            
                            flex = jenc.GetString(chunk);
                            countercharer++;
                            listascroll.Items.Add("Car - "+countercharer.ToString()+": " + hex + " - " + flex);
                            chunk = br.ReadBytes(2);
                            Array.Reverse(chunk);
                        }
                        
                    }
                }
            }
        }

        private void listascroll_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool caracterbinario (char amijo)
            {
                if (amijo == '0')
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            if (listascroll.SelectedIndex != -1)
            {
                lastlitsboxindex = listascroll.SelectedIndex;
            }
            
            if (fileIsLoaded & (listascroll.SelectedIndex != -1)){ 

                using (FileStream fs = new FileStream(archivoabiegto, FileMode.Open, FileAccess.Read))
                {
                    using (BinaryReader br = new BinaryReader(fs, new ASCIIEncoding()))
                    {
                        byte[] chunk;
                        byte[] punk;
                        string hex = "";
                        string bufferdeglyfo1 = "";
                        string bufferdeglyfo2 = "";
                        string[] lineaglyfo = new string[20];
                        br.BaseStream.Position = 0;
                        cuadriculagraf2.Children.Clear();
                        chunk = br.ReadBytes(2);
                        Array.Reverse(chunk);
                        hex = BitConverter.ToString(chunk).Replace("-", string.Empty);

                        while (chunk.Length > 0 & hex != "0000")
                        {
                            chunk = br.ReadBytes(2);
                            Array.Reverse(chunk);
                            hex = BitConverter.ToString(chunk).Replace("-", string.Empty);
                        }

                        for (int i=0; i< (listascroll.SelectedIndex); i++)
                        {
                            chunk = br.ReadBytes(22);
                        }
                        for (int j=0; j< 11; j++)
                        {
                            chunk = br.ReadBytes(1);
                            bufferdeglyfo1 = Convert.ToString(chunk[0], 2).PadLeft(8, '0');
                            chunk = br.ReadBytes(1);
                            bufferdeglyfo2 = Convert.ToString(chunk[0], 2).PadLeft(8, '0');
                            lineaglyfo[j]=bufferdeglyfo2 + bufferdeglyfo1;
                            char[] chars = lineaglyfo[j].ToCharArray();

                            for (int k = 0; k < lineaglyfo[j].Length-4; k++)
                            {
                                MyButton relleno = new MyButton();
                                relleno.encendido = false;
                                relleno.Click+= new RoutedEventHandler(relleno_Click);
                                
                                if (!caracterbinario(chars[k])) {
                                    
                                    relleno.Background = new SolidColorBrush(Colors.White);
                                    relleno.encendido = true;
                                }
                                else
                                {
                                    relleno.Background = new SolidColorBrush(Colors.Black);
                                }
                                cuadriculagraf2.Children.Add(relleno);
                            }

                        }
                    }
                }
            }
        }
        private void relleno_Click(object sender, RoutedEventArgs e)
        {
            MyButton clicked = (MyButton)sender;
            
            if (clicked.encendido)
            {
                clicked.Background = new SolidColorBrush(Colors.Black);
                clicked.encendido = false;
            }
            else
            {
                clicked.Background = new SolidColorBrush(Colors.White);
                clicked.encendido = true;
            }

            
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (fileIsLoaded)
            {
                File.Delete(archivoabiegto);
            }
        }

        private void botonSaveglyfo_Click(object sender, RoutedEventArgs e)
        {
            byte[] Concatenacion(byte[] a, byte[] b)
            {
                byte[] output = new byte[a.Length + b.Length];
                for (int i = 0; i < a.Length; i++)
                    output[i] = a[i];
                for (int j = 0; j < b.Length; j++)
                    output[a.Length + j] = b[j];
                return output;
            }
            if (fileIsLoaded)
            {
                listascroll.Items[listascroll.SelectedIndex] += "*";
                using (FileStream fs = new FileStream(archivoabiegto, FileMode.Open, FileAccess.Write))
                {
                    using (BinaryWriter br = new BinaryWriter(fs, new ASCIIEncoding()))
                    {
                        byte[] chunk = new byte[2];
                        byte[] punk = new byte[22];
                        int saltoinicial= (listascroll.Items.Count*2);
                        int saltocaracter = ((lastlitsboxindex) * 22);
                        int contadorespecial =0 ;
                        string hex = "";
                        string pex = "";
                        br.Seek(saltoinicial, SeekOrigin.Begin);
                        br.Seek(saltocaracter, SeekOrigin.Current);
                        

                        for (int i = 0; i < cuadriculagraf2.Children.Count; i++)
                        {
                            MyButton celdilla = (MyButton)cuadriculagraf2.Children[i];
                            if (celdilla.encendido)
                            {
                                hex += "0";
                            }
                            else
                            {
                                hex += "1";
                            }
                            if (contadorespecial == 0)
                            {
                                
                            }
                            contadorespecial++;
                            if (contadorespecial >= 12){
                                contadorespecial = 0;
                                hex += "1111";

                            }
                            
                        }
                        for (int j = 0; j < 22; j+=2)
                        {
                            chunk[0] = Convert.ToByte(hex.Substring(8 * j, 8), 2);
                            chunk[1] = Convert.ToByte(hex.Substring(8 * (j+1), 8), 2);
                            Array.Reverse(chunk);
                            pex += Convert.ToString(chunk[0], 2).PadLeft(8, '0');
                            pex += Convert.ToString(chunk[1], 2).PadLeft(8, '0');
                        }
                        for (int k=0; k < 22; k++)
                        {
                            punk[k] = Convert.ToByte(pex.Substring(8 * k, 8), 2);
                            
                        }
                        br.Write(punk);
                    }
                }
            }
        }

        private void botonSavefile_Click(object sender, RoutedEventArgs e)
        {
            if (fileIsLoaded)
            {
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();

                saveFileDialog1.Filter = "1bppf files (*.1bppf)|*.1bppf|All files (*.*)|*.*";
                saveFileDialog1.FilterIndex = 2;
                saveFileDialog1.RestoreDirectory = true;

                if (saveFileDialog1.ShowDialog() == true)
                {
                    string extensorin = saveFileDialog1.FileName;
                    string extensoron = extensorin.Substring(extensorin.LastIndexOf('.')-1 ,6);
                    if (extensoron == ".1bppf")
                    {
                        File.Copy(archivoabiegto, extensorin, true);
                    }
                    else
                    {
                        File.Copy(archivoabiegto, extensorin + ".1bppf", true);
                    }
                }
            }
        }
    }
}
