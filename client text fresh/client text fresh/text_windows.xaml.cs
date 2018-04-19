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
using System.Windows.Shapes;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.ComponentModel;
using System.IO.Ports;

namespace client_text_fresh
{
    /// <summary>
    /// text_windows.xaml 的互動邏輯
    /// </summary>
    public partial class text_windows : Window
    {
        public IPEndPoint server_ipep;
        public string server_ip_address;
        public Socket client2server;
        public socket_enclu now_sock_en;
        public delegate void file_handle();
        public byte[] recv_byte = new byte[2048];
        public List<file_combobox> file_now_list = new List<file_combobox>();
        public file_combobox sub_combo_filelist;
        public BackgroundWorker fresh_the_filelist = new BackgroundWorker();
        public bool filelist_end = false;
        ManualResetEvent pause_fresh_the_filelist = new ManualResetEvent(true);
        ManualResetEvent pause_test_listen = new ManualResetEvent(true);
        public string nowa;
        public BackgroundWorker test_listen = new BackgroundWorker();
        public FileStream nowfile_stream;
        public StreamReader file_reader;
        public string now_filename;
        public byte[] other_recv_byte = new byte[2048];
        public byte[] file_reg_byte;
        public string fortest;
        public bool send_wanted_file_click = false;
        public bool fresh_filelist_click = false;
        public string recv_string = "";
        public string the_last = "";
        public Paragraph p;
        public string[] richboxtext = new string[1];
        public int richbox_line_index = 0;
        public text_windows(string the_ip)
        {
            server_ip_address = the_ip;
            InitializeComponent();
            server_ipep = new IPEndPoint(IPAddress.Parse(server_ip_address), 8080);
            client2server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            client2server.Connect(server_ipep);
            now_sock_en = new socket_enclu(client2server);          
            start_fresh_filelist();
            this.Dispatcher.BeginInvoke((Action)delegate ()
            {
                p = doc_box.Document.Blocks.FirstBlock as Paragraph;
            });
        }
       
        public void pause_test()
        {
            pause_test_listen.Reset();
        }
        public void resume_test()
        {
            pause_test_listen.Set(); 
        }
       
        public void recv_back(object sender,DoWorkEventArgs e)
        {
            while (true)
            {
                if (fresh_filelist_click)
                {
                    fortest = "";
                    int the_byte_num = now_sock_en.call_sock().Receive(recv_byte);

                    string filename_encode = Encoding.Unicode.GetString(recv_byte, 0, the_byte_num);
                    if (filename_encode != "end")
                    {
                        this.Dispatcher.BeginInvoke((Action)delegate ()
                        {
                            file_list_combobox.Items.Add(filename_encode);                            
                           /* this.doc_box.AppendText(filename_encode);
                            fortest = "";*/
                        });
                    }
                   
                    else
                    {                      
                        filelist_end = true;
                    }
                    recv_byte = new byte[2048];
                    if (filelist_end)
                    {
                        this.Dispatcher.BeginInvoke((Action)delegate ()
                        {
                            file_list_combobox.Items.Add("創新檔案");                            
                        });
                        filelist_end = false;
                        fresh_filelist_click = false;
                    }
                }
                if(send_wanted_file_click)
                {                   
                    int wait_recv = now_sock_en.call_sock().Receive(other_recv_byte);
                    string recv_file_string = "";
                    recv_file_string = Encoding.Unicode.GetString(other_recv_byte, 0, wait_recv);
                    the_last = recv_file_string.Substring(recv_file_string.Length - 1);
                    if (recv_file_string != "end")
                    {
                        this.Dispatcher.BeginInvoke((Action)delegate ()
                        {
                            if (the_last == "\n")
                            {
                                doc_box.AppendText(recv_file_string.Substring(0, recv_file_string.Length - 1) + "\r");
                            }
                            else
                            {
                                doc_box.AppendText(recv_file_string.Substring(0, recv_file_string.Length));
                                doc_box.AppendText(Convert.ToString(doc_box.Document.Blocks.Count));
                            }
                        
                        });
                        if (the_last == "\n")
                        {                           
                            richboxtext[richbox_line_index] = recv_file_string.Substring(0, recv_file_string.Length - 1) + "\n";
                        }
                        else
                        {                           
                            richboxtext[richbox_line_index] = recv_file_string.Substring(0, recv_file_string.Length);                           
                        }                      
                        Array.Resize(ref richboxtext, richboxtext.Length + 1);
                        richbox_line_index++;
                        byte[] file_write_byte = new byte[2048];
                        recv_string = recv_string + recv_file_string;                                                
                    }
                    else
                    {                       
                        File.WriteAllText(now_filename, recv_string,Encoding.UTF8);                       
                        recv_string = "";
                        send_wanted_file_click = false;
                    }
                    other_recv_byte = new byte[2048];
                }
                pause_fresh_the_filelist.WaitOne(Timeout.Infinite);                
            }
        }
        public void start_fresh_filelist()
        {
            fresh_the_filelist.DoWork += new DoWorkEventHandler(recv_back);
            fresh_the_filelist.WorkerSupportsCancellation = true;
            fresh_the_filelist.RunWorkerAsync();
        }
        private void Fresh_file_list_Click(object sender, RoutedEventArgs e)
        {
            fresh_filelist_click = true;
            file_list_combobox.Items.Clear();
            
            now_sock_en.sendto("read the filelist"); 
        }
        private void pause_fresh_filelist()
        {
            pause_fresh_the_filelist.Reset();
        }
        private void resume_fresh_filelist()
        {
            pause_fresh_the_filelist.Set();
        }

        private void Send_fileread_button_Click(object sender, RoutedEventArgs e)
        {
            send_wanted_file_click = true;
            string the_selectedfile = file_list_combobox.SelectedItem.ToString();
            now_filename = the_selectedfile;
            string nowpath = System.IO.Directory.GetCurrentDirectory();
            this.Dispatcher.BeginInvoke((Action)delegate ()
            {
                doc_box.Document.Blocks.Clear();
            });
            if (now_filename == "創新檔案")
            {
                now_filename = Create_newfile.Text+".txt"; 
                the_selectedfile = now_filename;
            }
            now_sock_en.sendto(the_selectedfile);
            this.Dispatcher.BeginInvoke((Action)delegate ()
            {
                doc_box.AppendText(Convert.ToString(richbox_line_index +" || "));
                for (int i = 0; i < richbox_line_index - 1; ++i)
                {
                    doc_box.AppendText(richboxtext[i]);                   
                }
            });
        }

        private void send_filetext_Click(object sender, RoutedEventArgs e)
        {
            now_sock_en.sendto("send the file");
            TextRange tr = new TextRange(doc_box.Document.ContentStart, doc_box.Document.ContentEnd);
            string very_big_sending_string="";
            for(int i = 0; i < richbox_line_index - 1; ++i)
            {
                very_big_sending_string = very_big_sending_string + richboxtext[i];
            }
            now_sock_en.sendto(very_big_sending_string);
            Thread.Sleep(20);
            now_sock_en.sendto("end");
          
        }
        private void Close_windows(object sender,CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /*
        now_sock_en.call_sock().BeginReceive(recv_byte, 0, 2048, 0, new AsyncCallback(handle_file_name), now_sock_en.call_sock());
    }
    private void handle_file_name(IAsyncResult re)
    {
        string the_name_file = Encoding.ASCII.GetString(recv_byte);

        Thefile.Content = the_name_file;
        if (the_name_file != "end")
        {
            file_now_list.Add(the_name_file);
            now_sock_en.call_sock().BeginReceive(recv_byte, 0, 2048, 0, new AsyncCallback(handle_file_name), now_sock_en.call_sock());
        }
        else
        {

        }
    }*/

    }
}
