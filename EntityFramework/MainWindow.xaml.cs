using EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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


namespace EntityFramework
{
    public partial class MainWindow : Window
    {
        //Database handler
        //readonly is used when the variable is not supposed to change its pointer to the adress space 
        private readonly Dat154Context dx = new();

        //Variabels for the database tabels
        private readonly LocalView<Student> Students;
        private readonly LocalView<Grade> Grades;
        private readonly LocalView<Course> Courses;

        public MainWindow()
        {
            InitializeComponent();

            //Initialize the LocalView lists
            Students = dx.Students.Local;

            //Load in the students from the database
            dx.Students.Load();

            //Set data to the list from MainWindow
            studentList.DataContext = Students.OrderBy(s => s.Studentname);

        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            studentList.DataContext = Students.Where(s => s.Studentname.Contains(searchField.Text))
                                              .OrderBy(s => s.Studentname);
        }

        //Event handler for opening the Editor Window
        private void editButton_Click(object sender, RoutedEventArgs e)
        {

            Editor ed = new(dx);

            ed.Show();
        }

        private void studentList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //Retrive a student from the GUI list
            Student s = (Student)studentList.SelectedItem;

            if(s != null)
            {
                Editor ed = new(s, dx);
                ed.Show();
            }

        }

        private void coursesButton_Click(object sender, RoutedEventArgs e)
        {
            CourseWindow cw = new CourseWindow(dx);

            cw.Show();
        }

        private void gradesButton_Click(object sender, RoutedEventArgs e)
        {
            GradeWindow gw = new GradeWindow(dx);

            gw.Show();

        }
    }
}
