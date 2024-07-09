using UnityEngine;
using UnityEngine.Events;
using YooAsset;

namespace SangoUtils.Patchs_YooAsset
{
    internal class SangoPatchConfig : MonoBehaviour
    {
        //The CDN PathDef as following, you can change it in PatchOperationOP_InitializePackage.GetHostServerURL()
        [SerializeField]
        [Tooltip("{hostServerIP}/CDN/Editor/Unity/{appID}/Patch/PC/{appVersion}")]
        private string _hostServerIP = "https://";
        [SerializeField]
        private string _appID = "Sango";
        [SerializeField]
        private string _appVersion = "1.0";
        [SerializeField]
        private string _packageName = "DefaultPackage";
        [SerializeField]
        private EPlayMode _playMode = EPlayMode.HostPlayMode;
        [SerializeField]
        private EDefaultBuildPipeline _buildPipeline = EDefaultBuildPipeline.BuiltinBuildPipeline;
        [SerializeField]
        private UnityEvent _onUpdaterDone;


        public string HostServerIP { get => _hostServerIP; }
        public string AppID { get => _appID; }
        public string AppVersion { get => _appVersion; }

        public string PackageName { get => _packageName; }
        public EPlayMode PlayMode { get => _playMode; }
        public EDefaultBuildPipeline BuildPipeline { get => _buildPipeline; }

        public UnityEvent OnUpdaterDone { get => _onUpdaterDone; }

        internal string PackageVersion { get; set; }
        internal ResourceDownloaderOperation ResourceDownloaderOperation { get; set; }
    }
}
