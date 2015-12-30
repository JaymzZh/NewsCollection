using System;
using System.ComponentModel;
using NewLife.Xml;

namespace NewsCollection.Service
{
    [DisplayName("历史数据采集时间")]
    [XmlConfigFile("Config/TimeConfig.config")]
    public class TimeConfig : XmlConfig<TimeConfig>
    {
        #region 属性

        private DateTime? _dealTime;
        /// <summary>历史数据采集时间点</summary>
        [DisplayName("历史数据采集时间点")]
        public DateTime DealTime
        {
            get
            {
                if (_dealTime == null) _dealTime = DateTime.Now;
                return _dealTime.Value;
            }
            set { _dealTime = value; }
        }

        #endregion
    }
}
