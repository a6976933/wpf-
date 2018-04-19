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


namespace client_text_fresh
{
    public class file_combobox
    {
        public string name { get; set; }
        public string be_selected { get; set; }
    }
    public class socket_enclu
    {
        private Socket nowsock;
        private NetworkStream nowsock_stream;
        private StreamReader nowsock_readstream;
        private StreamWriter nowsock_writestream;
        public socket_enclu(Socket s)
        {
            nowsock = s;
            nowsock_stream = new NetworkStream(nowsock);
            nowsock_readstream = new StreamReader(nowsock_stream);
            nowsock_writestream = new StreamWriter(nowsock_stream);
        }
        public void sendto(string send_string)
        {
            byte[] send_byte = new byte[2048];
            send_byte = Encoding.UTF8.GetBytes(send_string);
            nowsock.Send(send_byte);
        }
        public void streamsend_writeline(string send_string)
        {
            nowsock_writestream.WriteLine(send_string);
            nowsock_writestream.Flush();
        }
        public Socket call_sock()
        {
            return nowsock;
        }
        public string streamrecv_readline()
        {
            string read_string = nowsock_readstream.ReadLine();
            return read_string;
        }
        
    }
}
