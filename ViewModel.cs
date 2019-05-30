using DevExpress.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace rozn
{
    class ViewModel:ViewModelBase
    {
        //ApplicationContext db;
        /// <summary>
        /// Интервал проверки связи с УТМами в секундах
        /// </summary>
        public int PingInterval { get; set; } = 10;
        public int Clicks { get; set; }
        //public IEnumerable<tovar> Tovary { get; set; }
        //public DataSet1 UTMs { get; set; }

        public int TimeOut { get; set; } = 150;

        /// <summary>
        /// Признак пингования УТМ-ов
        /// </summary>
        public bool ScanIP { get; set; } = true;

        /// <summary>
        /// Выбранный магазин
        /// </summary>
        public DataTable MyProperty { get; set; }
        public DataSet1 dataSet1 { get; set; }
        public CollectionViewSource CR_EGAIS_MONITOR3ViewSource { get; set; }
        public CollectionViewSource cR_EGAIS_TRANSPORT_SERVICEViewSource { get; set; }



        public IEnumerable FilterTable { get; set; }



        public ICommand SetFilterTable {
            get {
                return new DelegateCommand(
                    () => {
                        FilterTable = dataSet1.CR_EGAIS_MONITOR3.AsEnumerable()
                        .Where(d => (d.МАГ == 1)).ToList();
                        }
                    );
            }
        }
        public ICommand AddClick {
            get {
                return new DelegateCommand(() => { Clicks++; });
            }
        }

        public ViewModel()
        {
            dataSet1 = new DataSet1();
            // Загрузить данные в таблицу CR_EGAIS_MONITOR3. Можно изменить этот код как требуется.
            rozn.DataSet1TableAdapters.CR_EGAIS_MONITOR3TableAdapter dataSet1CR_EGAIS_MONITOR3TableAdapter = new rozn.DataSet1TableAdapters.CR_EGAIS_MONITOR3TableAdapter();
            dataSet1CR_EGAIS_MONITOR3TableAdapter.Fill(dataSet1.CR_EGAIS_MONITOR3);

            DataColumn dataGridColumnPing = dataSet1.CR_EGAIS_MONITOR3.Columns.Add("pingReply");
            DataColumn dataGridColumnUTM = dataSet1.CR_EGAIS_MONITOR3.Columns.Add("UTMReply");

            CR_EGAIS_MONITOR3ViewSource = new CollectionViewSource();
            CR_EGAIS_MONITOR3ViewSource.Source = dataSet1CR_EGAIS_MONITOR3TableAdapter.GetData();
            CR_EGAIS_MONITOR3ViewSource.View.MoveCurrentToFirst();
            
            Thread thread = new Thread(new ThreadStart(MonitorIP));
            thread.Start();
            Thread threadUTM = new Thread(new ThreadStart(MonitorUTM));
            threadUTM.Start();
        }

        void MonitorIP()
        {
            Ping png = new Ping();
            int rc = dataSet1.CR_EGAIS_MONITOR3.Rows.Count;
            while (true)
            {
                for (int i = 0; (i < rc) & (ScanIP); i++)
                {
                    if (dataSet1.CR_EGAIS_MONITOR3.Rows[i]["STATUS"].ToString() == "1")
                    {
                        string ip = dataSet1.CR_EGAIS_MONITOR3.Rows[i].ItemArray[2].ToString();
                        if (ip.Length > 0)
                        {
                            ip = ip.Remove(ip.Length - 5);
                            PingReply pingReply = png.Send(ip, TimeOut);
                            dataSet1.CR_EGAIS_MONITOR3.Rows[i]["pingReply"] = pingReply.Status;
                        }
                    }
                }
                //Thread.Sleep(500);
            }
        }

        void MonitorUTM()
        {
            int rc = dataSet1.CR_EGAIS_MONITOR3.Rows.Count;
            while (true)
            {
                for (int i = 0; (i < rc) & (ScanIP); i++)
                {
                    if (dataSet1.CR_EGAIS_MONITOR3.Rows[i]["STATUS"].ToString() == "1")
                    {
                        string ip = dataSet1.CR_EGAIS_MONITOR3.Rows[i].ItemArray[2].ToString();
                        if (ip.Length > 0)
                        {
                            //ip = ip.Remove(ip.Length - 5);
                            WebRequest request = WebRequest.Create("http://yandex.ru");
                            HttpWebResponse response = null;
                            try
                            {
                                response = (HttpWebResponse)request.GetResponse();
                                dataSet1.CR_EGAIS_MONITOR3.Rows[i]["UTMReply"] = response.StatusCode;
                                response.Close();
                            }
                            catch (WebException e)
                            {
                                dataSet1.CR_EGAIS_MONITOR3.Rows[i]["UTMReply"] = e.Status;
                                //throw;
                            }
                        }
                    }
                }
                Thread.Sleep(300000);
            }
        }
    }
}
