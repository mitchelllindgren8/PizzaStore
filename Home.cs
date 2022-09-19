using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PizzaProject
{
    public partial class pizzaStoreForm : Form
    {
        private int itr = 0;
        private string oldToppingsG = "";
        private int itrPizza = 0;
        private int count = 0;
        private string toppingsG = "";
        public EventHandler ClickEvent;

        public pizzaStoreForm()
        {
            InitializeComponent();
            loadOwnerpage();
            loadChefpage();
        }

        private void loadChefpage()
        {
            panelChefContext.Controls.Clear();
            itrPizza = 0;

            SqlConnection conncection = new SqlConnection(@"Data Source=DESKTOP-AK16H71\SQLEXPRESS;Initial Catalog=Pizza;Integrated Security=True");
            int pizzaListSize = 0;
            string queryC = "SELECT COUNT(*) FROM dbo.PizzaListt";

            //find the pizza list size
            using (conncection)
            {
                using (SqlCommand cmmd = new SqlCommand(queryC, conncection))
                {
                    conncection.Open();
                    pizzaListSize = (int)cmmd.ExecuteScalar();
                    SqlDataReader readerr;
                    readerr = cmmd.ExecuteReader();
                    conncection.Close();
                }
            }
            conncection.Close();

            if (pizzaListSize <= 2)
            {
                //sets panel to original size
                panelChefContext.Size = new System.Drawing.Size(760, 317);
            }

            SqlConnection conT = new SqlConnection(@"Data Source=DESKTOP-AK16H71\SQLEXPRESS;Initial Catalog=Pizza;Integrated Security=True");
            string queryS = "Select * from PizzaListt";
            SqlCommand cmdT = new SqlCommand();
            cmdT.Connection = conT;
            cmdT.CommandText = queryS;
            conT.Open();
            SqlDataReader rdr = cmdT.ExecuteReader();

            //creates Pizza Panels with data pulled from SQL database
            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-AK16H71\SQLEXPRESS;Initial Catalog=Pizza;Integrated Security=True");
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("Select PizzaToppings from PizzaListt where id=@id", conn);
                    cmd.Parameters.AddWithValue("id", rdr["id"]);
                    SqlDataReader reader;
                    reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        int id = (int)rdr["id"];

                        Panel panelPizza = new Panel();
                        panelChefContext.Controls.Add(panelPizza);
                        panelPizza.BackColor = System.Drawing.Color.SandyBrown;
                        panelPizza.Location = new System.Drawing.Point(27, 17+itrPizza);
                        itrPizza += 146;
                        panelPizza.Name = "panelPizza";
                        panelPizza.Size = new System.Drawing.Size(688, 136);
                        panelPizza.TabIndex = 1;

                        panelChefContext.Controls.Add(panelCspacer);
                        panelCspacer.Location = new System.Drawing.Point(0, 10 + itrPizza);

                        Label lbToppings = new Label();
                        panelPizza.Controls.Add(lbToppings);
                        lbToppings.AutoSize = true;
                        lbToppings.Location = new System.Drawing.Point(44, 54);
                        lbToppings.MaximumSize = new System.Drawing.Size(375, 65);
                        lbToppings.Name = "lbToppings";
                        lbToppings.Size = new System.Drawing.Size(370, 65);
                        lbToppings.TabIndex = 2;
                        lbToppings.Text = rdr["PizzaToppings"].ToString();

                        PictureBox pbPizza = new PictureBox();
                        panelPizza.Controls.Add(pbPizza);
                        pbPizza.BackColor = System.Drawing.Color.Silver;
                        pbPizza.Image = global::PizzaProject.Properties.Resources.pizza;
                        pbPizza.Location = new System.Drawing.Point(500, 4);
                        pbPizza.Name = "pictureBox1temp";
                        pbPizza.Size = new System.Drawing.Size(128, 128);
                        pbPizza.TabIndex = 0;
                        pbPizza.TabStop = false;

                        Label pizzaId = new Label();
                        panelPizza.Controls.Add(pizzaId);
                        pizzaId.AutoSize = true;
                        pizzaId.Font = new System.Drawing.Font("Georgia", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                        pizzaId.Location = new System.Drawing.Point(32, 16);
                        pizzaId.Name = "label10temp";
                        pizzaId.Size = new System.Drawing.Size(142, 29);
                        pizzaId.TabIndex = 1;
                        pizzaId.Text = "Pizza #" + rdr["id"].ToString();

                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("The event list is empty", "No Data");
                    }

                    conn.Close();
                }
            } else
            {
                Console.WriteLine("No Pizza List data");
            }

            conncection.Close();
        }

        private void loadOwnerpage()
        {
            //removes empty strings in topping list
            if (count == 0)
            {
                string emptyTopping = "";
                deleteTopping(emptyTopping);
            }

            SqlConnection conT = new SqlConnection(@"Data Source=DESKTOP-AK16H71\SQLEXPRESS;Initial Catalog=Pizza;Integrated Security=True");
            string query = "Select * from ToppingList";
            SqlCommand cmdT = new SqlCommand();
            cmdT.Connection = conT;
            cmdT.CommandText = query;
            conT.Open();
            SqlDataReader rdr = cmdT.ExecuteReader();

            //creates topping list with data pulled from SQL database
            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-AK16H71\SQLEXPRESS;Initial Catalog=Pizza;Integrated Security=True");
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("Select Topping from ToppingList where Topping=@Topping", conn);
                    cmd.Parameters.AddWithValue("Topping", rdr["Topping"]);
                    SqlDataReader reader;
                    reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        Label lbltopping = new Label();
                        lbltopping.AutoSize = true;
                        lbltopping.Font = new System.Drawing.Font("Georgia", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                        lbltopping.Location = new System.Drawing.Point(330, 20 + itr);
                        itr += 50;
                        lbltopping.Name = "lbltopping";
                        lbltopping.Size = new System.Drawing.Size(460, 43);
                        lbltopping.TabIndex = 0;
                        lbltopping.Text = reader["Topping"].ToString();
                        panelOwnerContext.Controls.Add(lbltopping);

                        panelSpacer.Location = new System.Drawing.Point(330, 20 + itr);
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("The event list is empty", "No Data");
                    }

                    conn.Close();
                }

            } else
            {
                Console.WriteLine("No Toppings in Database");
            }
            conT.Close();
        }

        private void btnOwner_Click(object sender, EventArgs e)
        {
            panelOwner.Visible = true;
            panelChef.Visible = false;
            panelCreatePizza.Visible = false;
            panelUpdate.Visible = false;
        }

        private void btnChef_Click(object sender, EventArgs e)
        {
            panelChef.Visible = true;
            panelOwner.Visible = false;
            panelCreatePizza.Visible = false;
            panelUpdate.Visible = false;
            lblError.Visible = false;
            btnCreatePizza.Visible = true;
            btnConfirmUpdate.Visible = false;
            lblHeader1.Text = "What toppings do you want?";
            lblHeader1.Location = new Point(24, 4);
            cbToppings.Items.Clear();
        }

        private void btnNewTopping_Click(object sender, EventArgs e)
        {
            if (tbTopping.Text == "") {
                tbTopping.Text = "EMPTY FIELD";
                tbTopping.ForeColor = Color.Red;
            } else
            {
                tbTopping.ForeColor = Color.Black;
                string topping = tbTopping.Text;
                bool flag = checkTopping(topping);

                if (flag != true)
                {
                    SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-AK16H71\SQLEXPRESS;Initial Catalog=Pizza;Integrated Security=True");
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("");
                    cmd = new SqlCommand("insert into ToppingList(Topping)values('" + tbTopping.Text + "')", conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("The topping " + topping + " has been created.", "Successfully Created");

                    //Resets/Updates the topping printed list
                    panelOwnerContext.Controls.Clear();
                    itr = 0;
                    loadOwnerpage();
                }
                else
                {
                    MessageBox.Show("The topping " + topping + " already exists.", "Error Message");
                }
            }
        }

        public bool checkTopping(String topping)
        {
            //check if topping exists
            SqlConnection conTL = new SqlConnection(@"Data Source=DESKTOP-AK16H71\SQLEXPRESS;Initial Catalog=Pizza;Integrated Security=True");
            conTL.Open();
            SqlCommand cmdTL = new SqlCommand("");
            cmdTL.CommandText = ("Select * from ToppingList");
            cmdTL.Connection = conTL;
            bool flag = false;
            SqlDataReader rd = cmdTL.ExecuteReader();

            while (rd.Read())
            {
                if (rd["Topping"].ToString().ToLower() == topping.ToLower())
                {
                    flag = true;
                    break;
                }
            }

            return flag;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            string topping = tbTopping.Text;
            deleteTopping(topping);
        }

        private void deleteTopping(string strTopping)
        {
            bool flag = checkTopping(strTopping);

            //conditionally if Event w/ valid ID exists
            if (flag == true)
            {
                SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-AK16H71\SQLEXPRESS;Initial Catalog=Pizza;Integrated Security=True");
                con.Open();
                SqlCommand cmdD = new SqlCommand("Delete from ToppingList where Topping=@Topping", con);
                cmdD.Parameters.AddWithValue("@Topping", strTopping);
                cmdD.ExecuteNonQuery();
                con.Close();

                if (strTopping != "")
                {
                    MessageBox.Show("The topping " + strTopping + " has been deleted.", "Successfully Deleted");
                }

                //Resets/Updates the panel-interface
                panelOwnerContext.Controls.Clear();
                itr = 0;
                loadOwnerpage();
            }
            else
            {
                //removes any empty strings inside the topping list, avoids showing messages upon first load
                if (count > 0)
                {
                    MessageBox.Show("The topping " + strTopping + " does not exist.", "Error Message");
                    flag = false;
                }

                flag = false;
                count++;
            }
        }

        private void btnUpdateTopping_Click(object sender, EventArgs e)
        {
            panelUpdate.Visible = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            panelUpdate.Visible = false;
        }

        //UPDATE TOPPING IN TOPPINGLIST
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            string topping = tbTopping.Text;
            bool flag = checkTopping(topping);

            if (tbTopping.Text == "" || tbUpdated.Text == "")
            {
                MessageBox.Show("Missing one or more text fields", "Error Message");
            } else
            {
                if (flag == true)
                {
                    SqlConnection conU = new SqlConnection(@"Data Source=DESKTOP-AK16H71\SQLEXPRESS;Initial Catalog=Pizza;Integrated Security=True");
                    conU.Open();
                    SqlCommand cmdD = new SqlCommand("Update ToppingList set Topping = '" + tbUpdated.Text + "' where Topping=@Topping", conU);
                    cmdD.Parameters.AddWithValue("@Topping", topping);
                    cmdD.ExecuteNonQuery();
                    conU.Close();
                    MessageBox.Show(topping + " has been updated to " + tbUpdated.Text, "Updated Successfully");

                    //Resets/Updates the panel-interface
                    panelOwnerContext.Controls.Clear();
                    itr = 0;
                    loadOwnerpage();
                }
                else
                {
                    MessageBox.Show("The topping " + topping + " does not exist.", "Error Message");
                    flag = false;
                }
            }
        }

        private void btnNewPizza_Click(object sender, EventArgs e)
        {
            panelCreatePizza.Visible = true;
            btnCreatePizza.Visible = true;
            btnConfirmUpdate.Visible = false;
            lblHeader1.Text = "What toppings do you want?";
            lblHeader1.Location = new Point(24, 4);
            btnCreatePizza.Text = "Confirm";
            lblError.Visible = false;
            tbToppingsList.Text = "";
            cbToppings.Items.Clear();
            pizzCreator();
        }

        private void btnCanel_Click(object sender, EventArgs e)
        {
            lblError.Visible = false;
            panelCreatePizza.Visible = false;
            btnCreatePizza.Visible = true;
            btnConfirmUpdate.Visible = false;
            cbToppings.Items.Clear();
        }

        private void tbTopping_MouseClick(object sender, MouseEventArgs e)
        {
            if (tbTopping.Text == "EMPTY FIELD")
            {
                tbTopping.Text = "";
                tbTopping.ForeColor = Color.Black;
            }
        }

        private void pizzCreator()
        {
            loadChefpage();

            SqlConnection conT = new SqlConnection(@"Data Source=DESKTOP-AK16H71\SQLEXPRESS;Initial Catalog=Pizza;Integrated Security=True");
            string queryS = "Select * from ToppingList";
            SqlCommand cmdT = new SqlCommand();
            cmdT.Connection = conT;
            cmdT.CommandText = queryS;
            conT.Open();
            SqlDataReader rdr = cmdT.ExecuteReader();

            //creates Toppign radio buttons with data pulled from SQL database
            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-AK16H71\SQLEXPRESS;Initial Catalog=Pizza;Integrated Security=True");
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("Select Topping from ToppingList where Topping=@Topping", conn);
                    cmd.Parameters.AddWithValue("Topping", rdr["Topping"]);
                    SqlDataReader reader;
                    reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        cbToppings.Items.Add(rdr["Topping"].ToString());

                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("The event list is empty", "No Data");
                    }
                    conn.Close();
                }
            }
            else
            {
                Console.WriteLine("No Pizza List data");
            }

        }

        //CREATE DATA INTO PIZZALISTT.dbo
        private void btnCreatePizza_Click(object sender, EventArgs e)
        {
            if(tbToppingsList.Text == "")
            {
                lblError.Text = "*Please add a topping*";
                lblError.Visible = true;
            } else
            {
                lblError.Visible = false;
                Console.WriteLine("Pizza has: " + toppingsG);

                SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-AK16H71\SQLEXPRESS;Initial Catalog=Pizza;Integrated Security=True");
                connection.Open();
                SqlCommand command = new SqlCommand("insert into PizzaListt(PizzaToppings)values('" + toppingsG + "')", connection);
                command.ExecuteNonQuery();
                connection.Close();

                tbToppingsList.Text = "";
                loadChefpage();
                MessageBox.Show("The Pizza has been created", "Successfull Created");
                panelCreatePizza.Visible = false;
                toppingsG = "";

            }
        }

        private void btnAddTopping_Click(object sender, EventArgs e)
        {
            if(cbToppings.Text == "" || cbToppings.Text == "none") 
            {
                lblError.Text = "*Please select a topping*";
                lblError.Visible = true;
            } else
            {
                if(cbToppings.Items.Count <= 1)
                {
                    cbToppings.Items.Remove(cbToppings.Text);
                    btnAddTopping.Enabled = false;
                    lblError.Visible = false;

                    toppingsG += cbToppings.Text + ", ";
                    tbToppingsList.Text += cbToppings.Text + ", ";
                } else
                {
                    btnAddTopping.Enabled = true;
                    lblError.Visible = false;
                    toppingsG += cbToppings.Text + ", ";
                    tbToppingsList.Text += cbToppings.Text + ", ";
                    cbToppings.Items.Remove(cbToppings.Text);
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            btnAddTopping.Enabled = true;
            tbToppingsList.Clear();
            cbToppings.Items.Clear();
            toppingsG = "";
            pizzCreator();
        }

        private void btnRemovePizza_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(tbPizza.Text);
            bool flag = checkPizza(id);

            //conditional if PizzaId exists
            if (flag == true)
            {
                panelCreatePizza.Visible = false;

                SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-AK16H71\SQLEXPRESS;Initial Catalog=Pizza;Integrated Security=True");
                con.Open();
                SqlCommand cmdD = new SqlCommand("Delete from PizzaListt where id=@id", con);
                cmdD.Parameters.AddWithValue("@id", id);
                cmdD.ExecuteNonQuery();
                con.Close();

                loadChefpage();
                MessageBox.Show("Pizza#" + id + " has been deleted.", "Successfully Deleted");
            }
            else
            {
                MessageBox.Show("Pizza#" + id + " does not exist.", "Error Message");
            }
        }

        public bool checkPizza(int pizzaID)
        {
            //check if pizza exists
            SqlConnection conTL = new SqlConnection(@"Data Source=DESKTOP-AK16H71\SQLEXPRESS;Initial Catalog=Pizza;Integrated Security=True");
            conTL.Open();
            SqlCommand cmdTL = new SqlCommand("");
            cmdTL.CommandText = ("Select * from PizzaListt");
            cmdTL.Connection = conTL;
            bool flag = false;

            SqlDataReader rd = cmdTL.ExecuteReader();
            while (rd.Read())
            {
                int dbID = Convert.ToInt32(rd["id"].ToString());

                if (dbID == pizzaID)
                {
                    flag = true;
                    break;
                }
            }

            return flag;
        }

        private void tbPizza_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;

            if(!char.IsDigit(ch) && ch != 8)
            {
                e.Handled = true;
            }
        }

        private void btnUpdatePizza_Click(object sender, EventArgs e)
        {
            if (tbPizza.Text == "")
            {
                MessageBox.Show("Missing the Pizza #ID.", "Error Message");
            }
            else
            {
                int id = Convert.ToInt32(tbPizza.Text);
                bool flag = checkPizza(id);

                if (flag == true)
                {
                    tbToppingsList.Text = "";
                    panelCreatePizza.Visible = true;    //update pizza
                    lblHeader1.Text = "Update Pizza";
                    lblHeader1.Location = new Point(87, 4);
                    btnCreatePizza.Visible=false;
                    btnConfirmUpdate.Visible=true;

                    SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-AK16H71\SQLEXPRESS;Initial Catalog=Pizza;Integrated Security=True");
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("Select PizzaToppings from PizzaListt where id=@id", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader;
                    reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        cbToppings.Items.Clear();
                        pizzCreator();
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("The event list is empty", "No Data");
                    }
                    conn.Close();
                }
                else
                {
                    MessageBox.Show("Pizza#" + id + " does not exist.", "Error Message");
                    flag = false;
                }
            }
        }

        //UPDATE PIZZA CONFIRM
        private void btnConfirmUpdate_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(tbPizza.Text);

            SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-AK16H71\SQLEXPRESS;Initial Catalog=Pizza;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand("Select PizzaToppings from PizzaListt where id=@id", conn);
            cmd.Parameters.AddWithValue("@id", id);
            SqlDataReader reader;
            reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                oldToppingsG = reader["PizzaToppings"].ToString();
            }

            if (oldToppingsG == tbToppingsList.Text)
            {
                MessageBox.Show("No update found, add new topping", "Error Message");

            } else
            {
                SqlConnection connc = new SqlConnection(@"Data Source=DESKTOP-AK16H71\SQLEXPRESS;Initial Catalog=Pizza;Integrated Security=True");
                connc.Open();
                SqlCommand cmdd = new SqlCommand("Update PizzaListt set PizzaToppings = '" + tbToppingsList.Text + "' where id=@id", connc);
                cmdd.Parameters.AddWithValue("@id", id);
                cmdd.ExecuteReader();
                connc.Close();
                loadChefpage();
                MessageBox.Show("Pizza#" + id + " has been updated", "Successfully Updated");
                toppingsG = "";
            }

            conn.Close();
            panelCreatePizza.Visible = false;
        }
    }
}
