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
 
    public partial class GradeWindow : Window
    {
        public Dat154Context dx { get; set; }
        private readonly LocalView<Grade> Grades;

        public GradeWindow(Dat154Context dx)
        {
            InitializeComponent();

            this.dx = dx;
            
            //Initialize the LocalView lists
            Grades = dx.Grades.Local;

            //Load in the grades from the database
            dx.Grades.Load();

            //Set data to the list
            gradeList.DataContext = Grades.Where(g => !String.IsNullOrWhiteSpace(g.Grade1));

            failedGradesList.DataContext = this.dx.Grades.Include(gr => gr.CoursecodeNavigation)
                                                    .Where(g => !String.IsNullOrWhiteSpace(g.Grade1) && g.Grade1.Equals("F"))
                                                    .ToList();
        }

        private void gradeList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            //Retrive a course from the GUI list
            Grade g = (Grade)gradeList.SelectedItem;
            String grade = g.Grade1;

            if(g != null)
            {
                //Retrive the grader greater than the one selected
                var greaterGrades = dx.Grades.Include(gr => gr.CoursecodeNavigation)
                                             .Where(g => g.Grade1.CompareTo(grade) <= 0 && !String.IsNullOrWhiteSpace(g.Grade1))
                                             .ToList();

                gradesLabel.Text = "All grades equal or greater than " + grade;
                gradesGreaterList.DataContext = greaterGrades;
            }
        }
    }
}
