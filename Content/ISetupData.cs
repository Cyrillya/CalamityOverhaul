﻿namespace CalamityOverhaul.Content
{
    /// <summary>
    /// 提供一个通用的资源加载、卸载途径
    /// </summary>
    internal interface ISetupData
    {
        /// <summary>
        /// 该方法在CWRLoad中的最后调用
        /// </summary>
        public void SetupData() { }
        /// <summary>
        /// 该方法在CWRLoad前行调用
        /// </summary>
        public void LoadData() { }
        /// <summary>
        /// 该方法在CWRUnLoad最后调用
        /// </summary>
        public void UnLoadData() { }
    }
}
