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
using EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EntityFramework
{
 
    public partial class CourseWindow : Window
    {
        public Dat154Context dx { get; set; }
        private readonly LocalView<Course> Courses;

        public CourseWindow(Dat154Context dx)
        {
            InitializeComponent();

            this.dx = dx;
            
            //Initialize the LocalView lists
            Courses = dx.Courses.Local;

            //Load in the courses from the database
            dx.Courses.Load();

            //Set data to the list from MainWindow
            courseList.DataContext = Courses.OrderBy(c => c.Coursename);
        }

        private void courseList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //Retrive a course from the GUI list
            Course c = (Course)courseList.SelectedItem;
            if(c != null)
            {
                // Find all grades that have the selected course code
                var grades = dx.Grades.Where(g => g.Coursecode == c.Coursecode).ToList();

                studentsWithCourseList.ItemsSource = grades;
            }
        }
    }
}
