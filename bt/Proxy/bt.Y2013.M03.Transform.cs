//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.18010
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: global::System.Reflection.AssemblyVersionAttribute("1.0.0.0")]
[assembly: global::System.Reflection.AssemblyProductAttribute("bt")]
[assembly: global::System.Reflection.AssemblyTitleAttribute("bt")]
[assembly: global::Microsoft.Dss.Core.Attributes.ServiceDeclarationAttribute(global::Microsoft.Dss.Core.Attributes.DssServiceDeclaration.Transform, SourceAssemblyKey="bt.Y2013.M03, Version=1.0.0.0, Culture=neutral, PublicKeyToken=6f86af2ac3621ba2")]
[assembly: global::System.Security.SecurityTransparentAttribute()]
[assembly: global::System.Security.SecurityRulesAttribute(global::System.Security.SecurityRuleSet.Level1)]

namespace Dss.Transforms.Transformbt {
    
    
    public class Transforms : global::Microsoft.Dss.Core.Transforms.TransformBase {
        
        static Transforms() {
            Register();
        }
        
        public static void Register() {
            global::Microsoft.Dss.Core.Transforms.TransformBase.AddProxyTransform(typeof(global::bt.Proxy.btState), new global::Microsoft.Dss.Core.Attributes.Transform(bt_Proxy_btState_TO_bt_btState));
            global::Microsoft.Dss.Core.Transforms.TransformBase.AddSourceTransform(typeof(global::bt.btState), new global::Microsoft.Dss.Core.Attributes.Transform(bt_btState_TO_bt_Proxy_btState));
            global::Microsoft.Dss.Core.Transforms.TransformBase.AddProxyTransform(typeof(global::bt.Proxy.ConnectRequest), new global::Microsoft.Dss.Core.Attributes.Transform(bt_Proxy_ConnectRequest_TO_bt_ConnectRequest));
            global::Microsoft.Dss.Core.Transforms.TransformBase.AddSourceTransform(typeof(global::bt.ConnectRequest), new global::Microsoft.Dss.Core.Attributes.Transform(bt_ConnectRequest_TO_bt_Proxy_ConnectRequest));
            global::Microsoft.Dss.Core.Transforms.TransformBase.AddProxyTransform(typeof(global::bt.Proxy.SetActiveRequest), new global::Microsoft.Dss.Core.Attributes.Transform(bt_Proxy_SetActiveRequest_TO_bt_SetActiveRequest));
            global::Microsoft.Dss.Core.Transforms.TransformBase.AddSourceTransform(typeof(global::bt.SetActiveRequest), new global::Microsoft.Dss.Core.Attributes.Transform(bt_SetActiveRequest_TO_bt_Proxy_SetActiveRequest));
            global::Microsoft.Dss.Core.Transforms.TransformBase.AddProxyTransform(typeof(global::bt.Proxy.UpdateProcessingRequest), new global::Microsoft.Dss.Core.Attributes.Transform(bt_Proxy_UpdateProcessingRequest_TO_bt_UpdateProcessingRequest));
            global::Microsoft.Dss.Core.Transforms.TransformBase.AddSourceTransform(typeof(global::bt.UpdateProcessingRequest), new global::Microsoft.Dss.Core.Attributes.Transform(bt_UpdateProcessingRequest_TO_bt_Proxy_UpdateProcessingRequest));
            global::Microsoft.Dss.Core.Transforms.TransformBase.AddProxyTransform(typeof(global::bt.Proxy.AddToPathRequest), new global::Microsoft.Dss.Core.Attributes.Transform(bt_Proxy_AddToPathRequest_TO_bt_AddToPathRequest));
            global::Microsoft.Dss.Core.Transforms.TransformBase.AddSourceTransform(typeof(global::bt.AddToPathRequest), new global::Microsoft.Dss.Core.Attributes.Transform(bt_AddToPathRequest_TO_bt_Proxy_AddToPathRequest));
            global::Microsoft.Dss.Core.Transforms.TransformBase.AddProxyTransform(typeof(global::bt.Proxy.TrainKeypointRequest), new global::Microsoft.Dss.Core.Attributes.Transform(bt_Proxy_TrainKeypointRequest_TO_bt_TrainKeypointRequest));
            global::Microsoft.Dss.Core.Transforms.TransformBase.AddSourceTransform(typeof(global::bt.TrainKeypointRequest), new global::Microsoft.Dss.Core.Attributes.Transform(bt_TrainKeypointRequest_TO_bt_Proxy_TrainKeypointRequest));
        }
        
        private static global::bt.Proxy.btState _cachedInstance0 = new global::bt.Proxy.btState();
        
        private static global::bt.btState _cachedInstance = new global::bt.btState();
        
        public static object bt_Proxy_btState_TO_bt_btState(object transformFrom) {
            return _cachedInstance;
        }
        
        public static object bt_btState_TO_bt_Proxy_btState(object transformFrom) {
            return _cachedInstance0;
        }
        
        private static global::bt.Proxy.ConnectRequest _cachedInstance2 = new global::bt.Proxy.ConnectRequest();
        
        private static global::bt.ConnectRequest _cachedInstance1 = new global::bt.ConnectRequest();
        
        public static object bt_Proxy_ConnectRequest_TO_bt_ConnectRequest(object transformFrom) {
            return _cachedInstance1;
        }
        
        public static object bt_ConnectRequest_TO_bt_Proxy_ConnectRequest(object transformFrom) {
            return _cachedInstance2;
        }
        
        private static global::bt.Proxy.SetActiveRequest _cachedInstance4 = new global::bt.Proxy.SetActiveRequest();
        
        private static global::bt.SetActiveRequest _cachedInstance3 = new global::bt.SetActiveRequest();
        
        public static object bt_Proxy_SetActiveRequest_TO_bt_SetActiveRequest(object transformFrom) {
            return _cachedInstance3;
        }
        
        public static object bt_SetActiveRequest_TO_bt_Proxy_SetActiveRequest(object transformFrom) {
            return _cachedInstance4;
        }
        
        public static object bt_Proxy_UpdateProcessingRequest_TO_bt_UpdateProcessingRequest(object transformFrom) {
            global::bt.UpdateProcessingRequest target = new global::bt.UpdateProcessingRequest();
            global::bt.Proxy.UpdateProcessingRequest from = ((global::bt.Proxy.UpdateProcessingRequest)(transformFrom));
            target.Processing = from.Processing;
            return target;
        }
        
        public static object bt_UpdateProcessingRequest_TO_bt_Proxy_UpdateProcessingRequest(object transformFrom) {
            global::bt.Proxy.UpdateProcessingRequest target = new global::bt.Proxy.UpdateProcessingRequest();
            global::bt.UpdateProcessingRequest from = ((global::bt.UpdateProcessingRequest)(transformFrom));
            target.Processing = from.Processing;
            return target;
        }
        
        private static global::bt.Proxy.AddToPathRequest _cachedInstance6 = new global::bt.Proxy.AddToPathRequest();
        
        private static global::bt.AddToPathRequest _cachedInstance5 = new global::bt.AddToPathRequest();
        
        public static object bt_Proxy_AddToPathRequest_TO_bt_AddToPathRequest(object transformFrom) {
            return _cachedInstance5;
        }
        
        public static object bt_AddToPathRequest_TO_bt_Proxy_AddToPathRequest(object transformFrom) {
            return _cachedInstance6;
        }
        
        private static global::bt.Proxy.TrainKeypointRequest _cachedInstance8 = new global::bt.Proxy.TrainKeypointRequest();
        
        private static global::bt.TrainKeypointRequest _cachedInstance7 = new global::bt.TrainKeypointRequest();
        
        public static object bt_Proxy_TrainKeypointRequest_TO_bt_TrainKeypointRequest(object transformFrom) {
            return _cachedInstance7;
        }
        
        public static object bt_TrainKeypointRequest_TO_bt_Proxy_TrainKeypointRequest(object transformFrom) {
            return _cachedInstance8;
        }
    }
}
