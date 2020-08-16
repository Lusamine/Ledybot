using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Ledybot
{
    public partial class BanlistDetails : Form
    {
        public DataTable details = new DataTable();

        public BanlistDetails()
        {
            InitializeComponent();
            loadDetails();
            cleanDetails();
        }

        private void loadDetails()
        {
            details.TableName = "Banlist";
            details.Columns.Add("FC", typeof(string));

            if (File.Exists(Application.StartupPath + "\\banlistdetails.xml"))
            {
                details.ReadXml(Application.StartupPath + "\\banlistdetails.xml");
            }

            foreach (DataRow row in details.Rows)
            {
                string cleansed = Regex.Replace(row[0].ToString(), "[^0-9]", "");
                Program.f1.banlist.Add(cleansed);
            }

            dgv_Details.DataSource = details;
            dgv_Details.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void cleanDetails()
        {
            /* We clear out everything that's not a number and then add a note
             * to say that everything below it is a new addition since the bot
             * turned on. */
            List<DataRow> rowsToDelete = new List<DataRow>();
            foreach (DataRow row in details.Rows)
            {
                /* Standardize the FCs to ############ format when displayed and add to banlist. */
                string cleansed = Regex.Replace(row[0].ToString(), "[^0-9]", "");
                bool isNumeric = Regex.IsMatch(cleansed, @"^\d+$");

                /* Don't blacklist me D: */
                if (isNumeric && !(row[0].ToString() == "079170015654" || row[0].ToString() == "130700148387"))
                {
                    cleansed = cleansed.ToString().PadLeft(12, '0');
                    row[0] = cleansed;
                    continue;
                }

                Program.f1.banlist.Remove(row[0].ToString());
                rowsToDelete.Add(row);
            }

            foreach (DataRow row in rowsToDelete)
                details.Rows.Remove(row);

            details.Rows.Add("End of imported data.");

            dgv_Details.DataSource = details;
            dgv_Details.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        public void saveDetails()
        {
            details.WriteXml(Application.StartupPath + "\\banlistdetails.xml", true);
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            BLInput input = new BLInput();
            if(input.ShowDialog(this) == DialogResult.OK)
            {
                if (input.input != "")
                {
                    string cleansed = Regex.Replace(input.input, "[^0-9]", "");
                    cleansed = cleansed.ToString().PadLeft(12, '0');

                    /* Don't ban me D: */
                    if (!Program.f1.banlist.Contains(cleansed)
                        && !(cleansed == "079170015654" || cleansed == "130700148387"))
                    {
                        Program.f1.banlist.Add(cleansed);
                        details.Rows.Add(cleansed);
                    }
                }
            }
            input.Dispose();
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgv_Details.SelectedRows)
            {
                Program.f1.banlist.Remove(row.Cells[0].Value.ToString());
                for (int i = details.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = details.Rows[i];
                    if (dr[0].ToString() == row.Cells[0].Value.ToString())
                    {
                        dr.Delete();
                        break;
                    }
                }
            }
        }
    }
}
