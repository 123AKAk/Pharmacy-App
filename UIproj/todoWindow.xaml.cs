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

namespace UIproj
{
    /// <summary>
    /// Interaction logic for todoWindow.xaml
    /// </summary>
    public partial class todoWindow : Window
    {
        ToDoList tdl = new ToDoList();
        public todoWindow()
        {
            InitializeComponent();
            DataContext = tdl;
        }
        public void maddfunc()
        {
            DataContext = tdl;
            lvToDo.Items.Refresh();
        }

        private void drag(object sender, MouseButtonEventArgs e)
        {
            
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

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!txtItemDesc.Text.Trim().Equals(string.Empty))
            {
                tdl.Additem(txtItemDesc.Text.Trim());
            }
            txtItemDesc.Text = "";
            lvToDo.Items.Refresh();

            MainWindow apage = new MainWindow();
            apage.func();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            tdl.Save();

            MainWindow apage = new MainWindow();
            apage.func();
        }

        private void MarkAsDone(object sender, RoutedEventArgs e)
        {
            if (lvToDo.SelectedItem != null)
            {
                MessageBoxResult done = MessageBox.Show("Mark this as done?", "Done?", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                if (done == MessageBoxResult.Yes)
                {
                    (lvToDo.SelectedItem as ToDoItem).DoneDateTime = DateTime.Now.ToString("yyyy-MM-ddThh:mm:ss.ms");
                }
                lvToDo.Items.Refresh();
            }
        }

        private void lvToDo_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MarkAsDone(sender, e);
        }

        private void EditMenu_Click(object sender, RoutedEventArgs e)
        {
            if (lvToDo.SelectedItem != null)
            {
                todoEditPopup ep = new todoEditPopup(lvToDo.SelectedItem as ToDoItem);
                ep.Owner = this;
                ep.ShowDialog();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (lvToDo.SelectedItem != null)
            {
                MessageBoxResult del = MessageBox.Show("Delete this item?", "Delete?", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                if (del == MessageBoxResult.Yes)
                {
                    tdl.DeleteItem(lvToDo.SelectedItem as ToDoItem);
                }
                lvToDo.Items.Refresh();
            }
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
