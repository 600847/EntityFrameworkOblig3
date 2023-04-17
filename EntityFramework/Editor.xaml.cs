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
 
    public partial class Editor : Window
    {
        //Retrive the Dat154Context from the mainWindow
        public Dat154Context dx { get; set; }
        private readonly LocalView<Course> Courses;
        private readonly LocalView<Grade> Grades;
        private readonly LocalView<Student> Students;
        public Editor(Dat154Context dx)
        {
            this.dx = dx;

            InitializeComponent();
            //Initialize the LocalView lists
            Students = dx.Students.Local;

            //Load in the courses from the database
            dx.Students.Load();

            //Set data to the list from MainWindow
            studentList.DataContext = Students.OrderBy(s => s.Id);
        }
        //Constructor for retriveing a Student object from the MainWindow
        public Editor(Student s, Dat154Context dx)
        {
            InitializeComponent();

            //Send the values from the object to the GUI
            sname.Text = s.Studentname;
            sname_Copy.Text = s.Studentname;

            //Initialize the LocalView lists
            Courses = dx.Courses.Local;

            //Load in the courses from the database
            dx.Courses.Load();

            //Set data to the list from MainWindow
            studentList.DataContext = Courses.OrderBy(c => c.Coursename);

        }

      

        private void studentRemove_Click(object sender, RoutedEventArgs e)
        {
            Student s = (Student)studentList.SelectedItem;
            Course c = (Course)studentWithCoursesList.SelectedItem;

            if (s != null)
            {
                var grade = dx.Grades.FirstOrDefault(g => g.Studentid == s.Id && g.Coursecode == c.Coursecode);

                if (grade != null)
                {
                    // Remove the grade entry from the database and update the context
                    dx.Grades.Remove(grade);
                    dx.SaveChanges();
                    MessageBox.Show("The student has been removed from the course successfully.");
                    sname.Text = sname_Copy.Text = "";
                }
            }

        }

        private void studentListEdit_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //Retrive a course from the GUI list
            Student s = (Student)studentList.SelectedItem;
            if (s != null)
            {
                // Find all students having that course
                var courses = dx.Grades
                                .Where(g => g.Studentid == s.Id)
                                .Select(g => g.CoursecodeNavigation)
                                .ToList();

                // Find all courses that the student doesn't have
                var allCourses = dx.Courses.ToList(); // Assuming you have a collection of all courses in your data source
                var freeCourses = allCourses.Except(courses).ToList();

                FreeCourses.ItemsSource = freeCourses;
                studentWithCoursesList.ItemsSource = courses;
                studentWithCourses.Text = "Student: " + s.Studentname;
                sname.Text = s.Studentname;
                sname_Copy.Text = s.Studentname;

            }
           
        }

        private void studentWithCoursesList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Course c = (Course)studentWithCoursesList.SelectedItem;
            if(c != null)
            {
                Student s = (Student)studentList.SelectedItem;
                sname.Text = s.Studentname;
                sname_Copy.Text = s.Studentname;
                cname.Text = c.Coursename;
            }
        }

        private void studentAdd_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve the selected student and course from the respective controls
            Student s = (Student)studentList.SelectedItem;
            Course c = (Course)FreeCourses.SelectedItem;

            if (s != null && c != null)
            {
                // Check if the student already has the course
                var existingGrade = dx.Grades.FirstOrDefault(g => g.Studentid == s.Id && g.Coursecode == c.Coursecode);

                if (existingGrade != null)
                {
                    // Display a message indicating that the student already has the course
                    MessageBox.Show("The student already has this course.");
                }
                else
                {
                    // Create a new Grade object and add it to the dx context
                    Grade newGrade = new Grade()
                    {
                        Studentid = s.Id,
                        Coursecode = c.Coursecode,
                        Grade1 = "A"
                    };

                    dx.Grades.Add(newGrade);
                    dx.SaveChanges();

                    // Update the UI or perform other necessary actions
                    MessageBox.Show("The student has been added to the course successfully.");
                    sname.Text = sname_Copy.Text = "";
                }
            }
        }

    }
}
