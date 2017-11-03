﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using K12.Data;
namespace Counsel_System.Forms
{
    public partial class SetUserDefineDataForm : FISCA.Presentation.Controls.BaseForm 
    {
        DAO.LogTransfer _LogTransfer;
        StudentRecord _studRec;

        public SetUserDefineDataForm()
        {
            InitializeComponent();
            this.MinimumSize = this.MaximumSize = this.Size;
            _LogTransfer = new DAO.LogTransfer();
        }
        K12.Data.Configuration.ConfigData cd = K12.Data.School.Configuration[Global.CounselUserDefineDataRootName];
        /// <summary>
        /// 取得設定資料顯示到畫面
        /// </summary>
        /// <returns></returns>
        private void GetDataToForm()
        {
            int row = 0;
            dgv.Rows.Clear();
            foreach (KeyValuePair<string, string> data in Global.CounselXMLToDictP1(cd[Global.CounselUserDefineDataName]))
            {
                row=dgv.Rows.Add();
                dgv.Rows[row].Cells[0].Value = data.Key;
                //dgv.Rows[row].Cells[1].Value = data.Value;                
            }        
        }

        /// <summary>
        /// 儲存畫面上設定資料
        /// </summary>
        /// <param name="data"></param>
        private void SetDataFromForm()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            int ErrorCount = 0;

            // 清除 Error
            foreach (DataGridViewRow row in dgv.Rows)
                foreach (DataGridViewCell cell in row.Cells)
                    cell.ErrorText = "";

            // 檢查是否有空白欄位
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow)
                    continue;

                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value == null)
                    {
                        cell.ErrorText = "不允許空白!";
                        ErrorCount++;
                    }
                    else
                    {
                        if (cell.Value.ToString() == string.Empty)
                        {
                            cell.ErrorText = "不允許空白!";
                            ErrorCount++;
                        }
                    }
                }
            }

            if (ErrorCount > 0)
            {
                FISCA.Presentation.Controls.MsgBox.Show("有資料有空白無法儲存.");
                return;
            }

            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow)
                    continue;

               if(row.Cells[0].Value !=null )
               {
                   string FName=row.Cells[0].Value.ToString ();
                   //string FType=row.Cells[1].Value.ToString ();
                   if (!data.ContainsKey(FName))
                       data.Add(FName, "string");
                   //data.Add(FName, FType);
                   else
                   {
                       row.Cells[0].ErrorText = "資料重複!";
                       ErrorCount++;
                   }
               }
            }
            // 儲存
            if (ErrorCount > 0)
            {
                FISCA.Presentation.Controls.MsgBox.Show("有資料重複無法儲存.");
                return;
            }
            
            cd[Global.CounselUserDefineDataName]=Global.CounselXMLToDictP1(data);
            cd.Save();
            FISCA.Presentation.Controls.MsgBox.Show("儲存成功.");
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SetDataFromForm();           
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SetUserDefineDataForm_Load(object sender, EventArgs e)
        {
            GetDataToForm();

            //if (Global._CounselUserDefSelectItemList.Count == 0)
            //    Global.StartDefaultValue();

            //foreach (KeyValuePair<string, string> data in Global._CounselUserDefSelectItemList)
            //{
            //    KeyValuePair<string, string> item = new KeyValuePair<string, string>(data.Key, data.Value);
            //    DataType.Items.Add(item);            
            //}                        
            //DataType.DisplayMember = "Key";
            //DataType.ValueMember = "Value";
            _LogTransfer.Clear();
        }

        private void dgv_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex > -1 && e.RowIndex >-1)
            dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].ErrorText = "";
        }

    }
}
