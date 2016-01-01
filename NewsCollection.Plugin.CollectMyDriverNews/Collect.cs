using System;
using System.Threading;

namespace NewsCollection.Plugin.CollectMyDriverNews
{
    public class Collect : ICollect
    {
        readonly CollectMyDriverNews _collectToday = new CollectMyDriverNews();
        readonly CollectMyDriverNews _collectHistory = new CollectMyDriverNews();

        public Collect()
        {
            //每次启动时重置已处理标志
            var timeConfig = MyDriverTimeConfig.Current;
            if (timeConfig.IsDealt)
            {
                timeConfig.IsDealt = false;
                timeConfig.Save();
            }
        }

        /// <summary>
        /// 采集今天是新闻
        /// </summary>
        public void CollectToday()
        {
            if (_collectToday.IsDealing)
                return;

            var dateNow = DateTime.Now;
            _collectToday.GetDriverNewsByDate(dateNow);//不判断是否已完成
        }

        /// <summary>
        /// 采集历史新闻
        /// </summary>
        public void CollectHistory()
        {
            var timeConfig = MyDriverTimeConfig.Current;
            //历史数据不循环采集，只执行一次
            if (timeConfig.IsDealt)
                return;

            timeConfig.IsDealt = true;
            timeConfig.Save();

            while (true)
            {
                if (_collectHistory.IsDealing)
                {
                    //避免还没采集结束
                    Thread.Sleep(1200);
                    continue;
                }

                var date = timeConfig.DealTime;
                //目前只采集近10年数据
                if (date < DateTime.Now.AddYears(-10))
                    return;

                //不采集今天的数据
                if (date.ToString("yyyy-MM-dd")
                    .Equals(DateTime.Now.ToString("yyyy-MM-dd"), StringComparison.OrdinalIgnoreCase))
                {
                    date = date.AddDays(-1);
                    timeConfig.DealTime = date;
                    timeConfig.Save();
                }

                _collectHistory.GetDriverNewsByDate(date);
                timeConfig.DealTime = date.AddDays(-1);
                timeConfig.Save();
            }
        }
    }
}