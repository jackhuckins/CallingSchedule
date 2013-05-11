using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;

namespace CallingSchedule
{
    public partial class Default2 : System.Web.UI.Page
    {
        private const string CLIENTTIME_SCRIPT_ID = "__CLIENTTIMEJS";
        private const string CLIENTTIME_FIELD = "__CLIENTTIME";
        public string ClientTime
        {
            get { return this.Request.Form[CLIENTTIME_FIELD]; }
        }
        protected override void OnLoad(EventArgs e)
        {
            ClientScript.RegisterHiddenField(CLIENTTIME_FIELD, "");
            if (!ClientScript.IsOnSubmitStatementRegistered(typeof(string), CLIENTTIME_SCRIPT_ID))
            {
                ClientScript.RegisterOnSubmitStatement(typeof(string), CLIENTTIME_SCRIPT_ID, "document.getElementById('" + CLIENTTIME_FIELD + "').value=new Date().toLocaleTimeString();");
            }
            base.OnLoad(e);
        }
        private DateTime WorkStart = new DateTime();
        DataTable WorkTimes = new DataTable();
        private string filePath = string.Empty;
        private string CurDate = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadInfo();
        }
        private void LoadInfo()
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
            {
                filePath = "" + Request.PhysicalApplicationPath + "\\ScheduledTimesWkend.xml";
            }
            else
            {
                filePath = "" + Request.PhysicalApplicationPath + "\\ScheduledTimes.xml";
            }
            GetStartTimes();
            ReadXML();
            //CurDate = ClientTime.InnerText;
        }
        protected void UpdateTimer_Tick(object sender, EventArgs e)
        {
            DateTime Local;
            DateTime Remote;
            DateTime Client = Convert.ToDateTime(ClientTime);
            DateTime Server = DateTime.Now;
            int ClientHour = Client.Hour;
            int ServerHour = Server.Hour;
            if (ClientHour == ServerHour)
            {
                Local = Convert.ToDateTime(ClientTime);
                Remote = Convert.ToDateTime(ClientTime).AddHours(1);
            }
            else
            {
                Local = Convert.ToDateTime(ClientTime).AddHours(-1);
                Remote = Convert.ToDateTime(ClientTime);
            }
            lblDate.Text = Local.ToLongDateString();
            lblLocalTime.Text = Local.ToString("hh:mm:ss tt");
            lblRemoteTime.Text = Remote.ToString("hh:mm:ss tt");
            lblCurrentEvent.Text = GetCurrentEvent(lblLocalTime.Text);
            LoadInfo();
            lblLocalTimeLabel.Visible = true;
            lblRemoteTimeLabel.Visible = true;
            lblTotalTimeLabel.Visible = true;
        }
        private void ChangeColor()
        {
            System.Drawing.Color DateColor = System.Drawing.Color.Green;
            System.Drawing.Color RemoteColor = System.Drawing.Color.Green;
            System.Drawing.Color LocalColor = System.Drawing.Color.Green;
            System.Drawing.Color CurrentColor = System.Drawing.Color.Green;
            System.Drawing.Color NextColor = System.Drawing.Color.Green;
            System.Drawing.Color TotalColor = System.Drawing.Color.Green;
            lblDate.ForeColor = DateColor;
            lblRemoteTimeLabel.ForeColor = System.Drawing.Color.Green;
            lblRemoteTime.ForeColor = System.Drawing.Color.Green;
            lblRemoteTimeLabel.ForeColor = System.Drawing.Color.Green;
            lblLocalTime.ForeColor = System.Drawing.Color.Green;
            lblLocalTimeLabel.ForeColor = System.Drawing.Color.Green;
        }
        #region XML
        private void GetStartTimes()
        {
            using (XmlReader reader = XmlReader.Create(filePath))
            {
                reader.MoveToContent();
                while (reader.Read())
                {
                    string StartTime = ReadNode(reader, "StartTime");
                    if (reader.Name == "StartTime")
                    {
                        WorkStart = Convert.ToDateTime(StartTime);
                    }
                }
            }
        }
        private void ReadXML()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("Event");
            dt.Columns.Add("Duration");
            dt.Columns.Add("Start");
            dt.Columns.Add("Finish");
            using (XmlReader reader = XmlReader.Create(filePath))
            {
                int CurrentID = 0;
                string TimeID = string.Empty;
                string TimeEvent = string.Empty;
                string TimeDuration = string.Empty;
                string TimeStart = string.Empty;
                string TimeFinish = string.Empty;
                reader.MoveToContent();
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element
                        && reader.Name == "Times")
                    {
                        TimeID = ReadNode(reader, "id");
                        CurrentID = Convert.ToInt32(TimeID);
                        TimeEvent = ReadNode(reader, "event");
                        TimeDuration = ReadNode(reader, "duration");
                        if (TimeID == "0")
                        {
                            TimeStart = WorkStart.ToShortTimeString();
                            TimeFinish = Convert.ToDateTime(TimeStart).AddMinutes(Convert.ToInt32(TimeDuration)).ToShortTimeString();
                        }
                        else if (TimeID == "1")
                        {
                            TimeStart = Convert.ToDateTime(dt.Rows[(CurrentID - 1)].ItemArray[3].ToString()).ToShortTimeString();
                            TimeFinish = Convert.ToDateTime(TimeStart).AddMinutes(Convert.ToInt32(TimeDuration)).ToShortTimeString();
                        }
                        else
                        {
                            TimeStart = Convert.ToDateTime(dt.Rows[(CurrentID - 1)].ItemArray[4].ToString()).ToShortTimeString();
                            TimeFinish = Convert.ToDateTime(TimeStart).AddMinutes(Convert.ToInt32(TimeDuration)).ToShortTimeString();
                        }
                        dt.Rows.Add(TimeID,TimeEvent,TimeDuration,TimeStart,TimeFinish);
                    }
                }
            }
            WorkTimes = dt;
            UpdateGrid();
        }
        private string ReadNode(XmlReader reader, string Node)
        {
            string Result = string.Empty;
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element &&
                    reader.Name == Node)
                {
                    Result = reader.ReadString();
                    break;
                }
            }
            return Result;
        }
        #endregion
        private void UpdateGrid()
        {
            //GridView1.DataSource = WorkTimes;
            //GridView1.DataBind();
        }
        private string GetCurrentEvent(string CurrentTime)
        {
            string Result = string.Empty;
            if (Variables.CurrentID <= 12)
            {
                DateTime Current = Convert.ToDateTime(CurrentTime);
                DateTime WorkFinish = Convert.ToDateTime(WorkTimes.Rows[0].ItemArray[4].ToString());
                DateTime EventStartTime = Convert.ToDateTime(WorkTimes.Rows[Variables.CurrentID].ItemArray[3].ToString());
                DateTime EventFinishTime = Convert.ToDateTime(WorkTimes.Rows[Variables.CurrentID].ItemArray[4].ToString());
                if (Current > EventFinishTime)
                {
                    Variables.CurrentID = Variables.CurrentID + 1;
                    string EventName = WorkTimes.Rows[Variables.CurrentID].ItemArray[1].ToString();
                    EventStartTime = Convert.ToDateTime(WorkTimes.Rows[Variables.CurrentID].ItemArray[3].ToString());
                    EventFinishTime = Convert.ToDateTime(WorkTimes.Rows[Variables.CurrentID].ItemArray[4].ToString());
                    EventName = "" + EventName + " <br /> " + EventStartTime.ToShortTimeString() + " - " + EventFinishTime.ToShortTimeString() + "";
                    WorkFinish = Convert.ToDateTime(WorkTimes.Rows[0].ItemArray[4].ToString());
                    lblNextEvent.Text = EventFinishTime.ToString("hh:mm:ss tt");
                    lblNextEventLabel.Text = "Next Event:";
                    lblTimeLeft.Text = EventFinishTime.Subtract(Current).ToString();
                    lblWorkLeft.Text = WorkFinish.Subtract(Current).ToString();
                    Result = EventName;
                }
                else
                {
                    string EventName = WorkTimes.Rows[Variables.CurrentID].ItemArray[1].ToString();
                    EventStartTime = Convert.ToDateTime(WorkTimes.Rows[Variables.CurrentID].ItemArray[3].ToString());
                    EventFinishTime = Convert.ToDateTime(WorkTimes.Rows[Variables.CurrentID].ItemArray[4].ToString());
                    EventName = "" + EventName + " <br /> " + EventStartTime.ToShortTimeString() + " - " + EventFinishTime.ToShortTimeString() + "";
                    WorkFinish = Convert.ToDateTime(WorkTimes.Rows[0].ItemArray[4].ToString());
                    lblNextEvent.Text = EventFinishTime.ToString("hh:mm:ss tt");
                    lblNextEventLabel.Text = "Next Event:";
                    lblTimeLeft.Text = EventFinishTime.Subtract(Current).ToString();
                    lblWorkLeft.Text = WorkFinish.Subtract(Current).ToString();
                    Result = EventName;
                }
            }
            return Result;
        }
    }
}