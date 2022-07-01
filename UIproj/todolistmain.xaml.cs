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

namespace UIproj
{
    /// <summary>
    /// Interaction logic for todolistmain.xaml
    /// </summary>
    public partial class todolistmain : Page
    {
        ToDoList tdl = new ToDoList();

        public todolistmain()
        {
            InitializeComponent();
            DataContext = tdl;
            scroller.ScrollToBottom();
        }

        public void func()
        {
            MessageBox.Show("Hey");
            DataContext = tdl;
            lvToDo.Items.Refresh();
            scroller.ScrollToBottom();
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            lvToDo.Items.Refresh();
            todoWindow openWin = new todoWindow();
            openWin.ShowDialog();
        }

        private void lvToDo_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            lvToDo.Items.Refresh();
            todoWindow openWin = new todoWindow();
            openWin.ShowDialog();
        }

        public Button sarah;
        private void Btndrawer_MouseEnter(object sender, MouseEventArgs e)
        {
            sarah = (Button)sender;
            sarah.Foreground = Brushes.DarkGray;
        }

        private void Btndrawer_MouseLeave(object sender, MouseEventArgs e)
        {
            sarah = (Button)sender;
            sarah.Foreground = Brushes.Gray;
        }
    }
}

