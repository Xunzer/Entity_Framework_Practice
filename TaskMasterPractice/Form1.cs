using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TaskMasterPractice.Model;

namespace TaskMasterPractice
{
    public partial class Form1 : Form
    {
        private tmDBContext tmContext;
        public Form1()
        {
            InitializeComponent();

            tmContext = new tmDBContext();

            var statuses = tmContext.Statuses.ToList();
               
            // initialize all the current tasks from the database
            foreach (Status s in statuses)
            {   
                // adding the different types of statuses into the combo box
                cboStatus.Items.Add(s);
            }
            refreshData();
        }

        private void refreshData()
        {
            // need a BindingSource containing LinQ query as middle step according to Microsoft
            BindingSource bi = new BindingSource();

            var query = from t in tmContext.Tasks
                        orderby t.DueDate
                        select new { t.Id, TaskName = t.Name, StatusName = t.Status.Name, t.DueDate };

            bi.DataSource = query.ToList();
            dataGridView1.DataSource = bi;

            dataGridView1.Refresh();
        }

        private void cmdCreateTask_Click(object sender, EventArgs e)
        {
            if (cboStatus.SelectedItem != null && txtTask.Text != string.Empty)
            {
                var newTask = new Model.Task
                {
                    Name = txtTask.Text,
                    StatusId = (cboStatus.SelectedItem as Model.Status).Id,
                    DueDate = dateTimePicker1.Value
                };

                // add this new task to the db context but not to the database yet
                tmContext.Tasks.Add(newTask);
                // save the changes to database and update the view
                tmContext.SaveChanges();
                refreshData();
            }
            else
            {
                MessageBox.Show("Please make sure all the data has been entered!");
            }
        }

        private void cmdDeleteTask_Click(object sender, EventArgs e)
        {
            // find the id based on the first cell of selected row then find the item
            var t = tmContext.Tasks.Find((int)dataGridView1.SelectedCells[0].Value);

            tmContext.Tasks.Remove(t);

            tmContext.SaveChanges();
            refreshData();
        }

        private void cmdUpdateTask_Click(object sender, EventArgs e)
        {
            if (cmdUpdateTask.Text == "Update")
            {   
                // show all the attributes from the selected task in the task window
                txtTask.Text = dataGridView1.SelectedCells[1].Value.ToString();
                dateTimePicker1.Value = (DateTime)dataGridView1.SelectedCells[3].Value;
                foreach (Status s in cboStatus.Items)
                {
                    if (s.Name == dataGridView1.SelectedCells[2].Value.ToString())
                    {
                        cboStatus.SelectedItem = s;
                    }
                }
                // swap the button to save
                cmdUpdateTask.Text = "Save";
            }
            else if (cmdUpdateTask.Text == "Save")
            {
                // find the id based on the first cell of selected row then find the item
                var t = tmContext.Tasks.Find((int)dataGridView1.SelectedCells[0].Value);

                // set the texts in the task creation table
                t.Name = txtTask.Text;
                t.StatusId = (cboStatus.SelectedItem as Status).Id;
                t.DueDate = dateTimePicker1.Value;

                tmContext.SaveChanges();
                refreshData();

                // swap the button to update
                cmdUpdateTask.Text = "Update";

                // reset the texts in the task creation table
                txtTask.Text = string.Empty;
                dateTimePicker1.Value = DateTime.Now;
                cboStatus.SelectedItem = null;
                cboStatus.Text = "Please Select...";
            }
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            // swap the button to update
            cmdUpdateTask.Text = "Update";

            // reset the texts in the task creation table
            txtTask.Text = string.Empty;
            dateTimePicker1.Value = DateTime.Now;
            cboStatus.SelectedItem = null;
            cboStatus.Text = "Please Select...";
        }
    }
}
