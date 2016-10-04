﻿using System;

namespace ICanPay
{
    /// <summary>
    /// 支付网关的Get与Post的数据
    /// </summary>
    public class GatewayParameter
    {

        #region 私有字段

        string name;

        #endregion


        #region 构造函数

        public GatewayParameter()
        {
        }


        public GatewayParameter(string parameterName, string parameterValue, GatewayParameterType parameterType)
        {
            this.name = parameterName;
            Value = parameterValue;
            Type = parameterType;
        }

        #endregion


        #region 属性

        /// <summary>
        /// 参数名
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new ArgumentNullException("parameterName", "参数名不能为空");
                }

                name = value;
            }
        }


        /// <summary>
        /// 参数值
        /// </summary>
        public string Value { get; set; }


        /// <summary>
        /// 参数类型
        /// </summary>
        public GatewayParameterType Type { get; set; }

        #endregion

    }
}
