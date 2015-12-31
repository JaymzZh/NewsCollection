using System;
using System.Threading.Tasks;
using XAgent;

namespace NewsCollection.Service
{
    /// <summary>能管系统数据整理代理服务。</summary>
    class AgentService : AgentServiceBase<AgentService>
    {
        #region 构造函数

        /// <summary>实例化一个代理服务</summary>
        public AgentService()
        {
            // 一般在构造函数里面指定服务名
            ServiceName = "CollectNewsServiceAgent";
        }

        #endregion

        #region 属性

        /// <summary>线程数</summary>
        public override int ThreadCount
        {
            get { return 2; }
        }

        /// <summary>显示名</summary>
        public override string DisplayName
        {
            get { return "新闻采集代理服务"; }
        }

        /// <summary>描述</summary>
        public override string Description
        {
            get { return "定时采集新闻数据，分析整理并入库。"; }
        }

        #endregion

        #region 核心

        readonly Collection _collectToday = new Collection();
        readonly Collection _collectHistory = new Collection();

        /// <summary>核心工作方法。调度线程会定期调用该方法</summary>
        /// <param name="index">线程序号</param>
        /// <returns>是否立即开始下一步工作。某些任务能达到满负荷，线程可以不做等待</returns>
        public override bool Work(int index)
        {
            // XAgent将开启ThreadCount个线程，0<index<ThreadCount，本函数即为每个任务线程的主函数，间隔Interval循环调用
            switch (index)
            {
                case 0:
                    //采集今天的实时数据
                    CollectToday();
                    break;
                case 1:
                    //采集历史数据
                    CollectHistory();
                    break;
            }

            return false;
        }

        private void CollectToday()
        {
            if (_collectToday.IsDealing)
                return;

            var dateNow = DateTime.Now;
            _collectToday.GetDriverNewsByDate(dateNow);//不判断是否已完成
        }

        private void CollectHistory()
        {
            var timeConfig = TimeConfig.Current;
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
                    Task.Delay(1000);
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

        public override void StartWork()
        {
            //干你想干的事
            var timeConfig = TimeConfig.Current;
            if (timeConfig.IsDealt)
            {
                timeConfig.IsDealt = false;
                timeConfig.Save();
            }

            base.StartWork();
        }

        /*public override void StopWork()
        {
            //释放一些资源等

            base.StopWork();
        }*/

        #endregion
    }
}
