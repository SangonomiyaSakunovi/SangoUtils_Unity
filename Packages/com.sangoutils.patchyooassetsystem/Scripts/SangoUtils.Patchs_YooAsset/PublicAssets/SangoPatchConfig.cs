using System;
using YooAsset;

namespace SangoUtils.Patchs_YooAsset
{
    public class SangoPatchConfig
    {
        //The CDN PathDef as following, you can change it in PatchOperationOP_InitializePackage.GetHostServerURL()
        //{hostServerIP}/CDN/Editor/Unity/{appID}/Patch/PC/{appVersion}

        public string HostServerIP { get; set; }
        public string AppID { get; set; }
        public string AppVersion { get; set; }

        public string PackageName { get; set; }
        public EPlayMode PlayMode { get; set; }
        public EDefaultBuildPipeline BuildPipeline { get; set; }

        public Action OnUpdaterDone { get; set; }
    }
}
